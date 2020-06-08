Shader "MileShader/MilesVolumeBox" {
	Properties {
		[HDR]_Color("Color", COLOR) = (1, 1, 1, 1)
		_Lower("Lower", Range(0, 1)) = 0
		_Upper("Upper", Range(0, 4)) = 1

		[Toggle(_ORTHOVIEW)]_OrthoView("Ortho View", FLOAT) = 0
		[Toggle(_USEFOG)]_UseFog("Use Fog", FLOAT) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest", Int) = 8
		[Enum(UnityEngine.Rendering.CullMode)]_Cull("Culling", float) = 1
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent+50" }
		Pass {
			Name "VolumeBoxUnlit"
			Tags { "LightMode" = "UniversalForward" }
			Blend SrcAlpha OneMinusSrcAlpha

			Cull [_Cull]
			ZTest [_ZTest]
			ZWrite Off

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

			#pragma shader_feature_local _USEFOG
			#pragma shader_feature_local _ORTHOVIEW

			#if defined(_USEFOG)
				#pragma multi_compile_fog
				#pragma shader_feature_local _HQFOG
			#endif

			#pragma multi_compile_instancing

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			CBUFFER_START(UnityPerMaterial)
				half4 _Color;
				half _Lower;
				half _Upper;
			CBUFFER_END

			#if defined(SHADER_API_GLES)
                TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
            #else
				TEXTURE2D_X_FLOAT(_CameraDepthTexture);
			#endif

			float4 _CameraDepthTexture_TexelSize;

			struct a2v {
				float4 vertex: POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 positionCS :SV_POSITION;
				float3 positionWS :TEXCOORD1;
				// porjectedPosition
				float2 positionPS: TEXCOORD2;
				float3 cameraPosOS: TEXCOORD3;
				float scale: TEXCOORD4;

				#if defined(_USEFOG)
					half fogCoord: TEXCOORD5;
				#endif

				float4 viewRayOS: TEXCOORD6;

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			// Packages/Core/ShaderLibrary/GeometricTools.hlsl
			float IntersectRayBox(float3 rayOrigin, float3 rayDirection, out float tEntry, out float tExit) {
				// rcp(FLT_EPS) = 16,777,216
				float3 rayDirInverse = clamp(rcp(rayDirection), -rcp(FLT_EPS), rcp(FLT_EPS));
				float3 boxMin = float3(-0.5, -0.5, -0.5);
				float3 boxMax = float3(0.5, 0.5, 0.5);

				float3 t0 = boxMin * rayDirInverse - (rayOrigin * rayDirInverse);
				float3 t1 = boxMax * rayDirInverse - (rayOrigin * rayDirInverse);

				float3 tSlabEntr =  min(t0, t1);
				float3 tSlabExit = max(t0, t1);

				tEntry = Max3(tSlabEntr.x, tSlabEntr.y, tSlabEntr.z);
				tExit = Min3(tSlabExit.x, tSlabExit.y, tSlabExit.z);

				tEntry = max(0.0f, tEntry);
				return tExit - tEntry;
			}

			v2f vert(a2v v) {
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				VertexPositionInputs vpi = GetVertexPositionInputs(v.vertex.xyz);
				o.positionCS = vpi.positionCS;

				o.positionPS = vpi.positionNDC.xy;
				o.positionWS = vpi.positionWS;

				o.cameraPosOS = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1)).xyz;

				float4x4 ObjectToWorldMatrix = GetObjectToWorldMatrix();
				// get scale from matrix
				float3 worldScale = float3(
					length(ObjectToWorldMatrix._m00_m10_m20),
					length(ObjectToWorldMatrix._m01_m11_m21),
					length(ObjectToWorldMatrix._m02_m12_m22)
				);

				o.scale = 1.0f / max(worldScale.x, max(worldScale.y, worldScale.z));

				#if defined(_USEFOG)
					o.fogCoord = ComputeFogFactor(o.positionCS.z);
				#endif

				//float4 positionVS = mul(UNITY_MATRIX_MV, v.vertex);

				float3 positionVS = vpi.positionVS;
				float3 viewRayVS = positionVS.xyz;

				float4x4 ViewToObjectMatrix = mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V);
				o.viewRayOS.xyz = mul((float3x3)ViewToObjectMatrix, -viewRayVS).xyz;
				o.viewRayOS.w = positionVS.z;
				return o;
			}

			half GetFogFactor(float z) {
				float clipZ_01 = UNITY_Z_0_FAR_FROM_CLIPSPACE(z);
				#if defined(FOG_LINEAR)
					float fogFactor = saturate(clipZ_01 * unity_FogParams.z + unity_FogParams.w);
					return half(fogFactor);
				#elif defined(FOG_EXP) || defined(FOG_EXP2)
					return half(unity_FogParams.x * clipZ_01);
				#else
					return 0.0h;
				#endif
			}

			inline float GetOrthoDepthFromZBuffer(float rawDepth) {
				#if defined(UNITY_REVERSED_Z)
					#if UNITY_REVERSED_Z == 1
						rawDepth = 1.0f - rawDepth;
					#endif
				#endif
				return lerp(_ProjectionParams.y, _ProjectionParams.z, rawDepth);
			}
			
			inline float GetProperEyeDepth(float rawDepth) {
				#if defined(_ORTHOVIEW)
					float perspSceneDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
					float orthoSceneDepth = GetOrthoDepthFromZBuffer(rawDepth);
					return lerp(perspSceneDepth, orthoSceneDepth, unity_OrthoParams.w);
				#else
					return LinearEyeDepth(rawDepth, _ZBufferParams);
				#endif
			}
			
			half4 frag(v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				half4 color = half4(1, 1, 1, 0);
				
				float3 rayDir = i.viewRayOS.xyz / i.viewRayOS.w;
				float3 rayStart = i.cameraPosOS;
			

				float2 screenUV = i.positionPS.xy / i.positionCS.w;
				
				#if defined(UNITY_SINGLE_PASS_STEREO)
					screenUV.x = screenUV.x * 0.5f + (float)unity_StereoEyeIndex * 0.5f;
				#endif

				

				#if defined(SHADER_API_GLES)
					float sceneZ = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV, 0);
				#else
					float sceneZ = LOAD_TEXTURE2D_X(_CameraDepthTexture, _CameraDepthTexture_TexelSize.zw * screenUV).x;
				#endif
				sceneZ = GetProperEyeDepth(sceneZ);

				

				float near;
				float far;
				// out near, out far
				float thickness = IntersectRayBox(rayStart, rayDir, near, far);

				

				float3 entryOS = rayStart + rayDir * near;
				float distanceToEntryOS = length(entryOS - i.cameraPosOS);

				

				float sceneDistanceOS = length(sceneZ * rayDir);
				float sceneToEntry = sceneDistanceOS - distanceToEntryOS;

				clip(sceneToEntry);
				
				float3 exitOS = rayStart + rayDir * far;

				float maxTravel = distance(exitOS, entryOS);
				float denom = min(sceneToEntry, maxTravel);
				float percentage = maxTravel / denom;

				percentage = rcp(percentage);

				float alpha = thickness * i.scale * percentage;

				alpha = smoothstep(_Lower, _Upper, alpha);

				color.a = saturate(alpha);

				color *= _Color;

				#if defined(_USEFOG)
					color.rgb = MixFog(color.rgb, i.fogCoord);
				#endif

				return color;
			}

			ENDHLSL
		}
	}
}