Shader "SoFunny/shd_layeredTerrain_v3" {
	Properties {
		[Toggle(_CONTROLMAP_SHOW)]_ShowControlMap("Show Control Map", float) = 0
		_ControlMap("ControlMap", 2D) = "white" {} 
		_Weight("Blend Weight", Range(0.001, 1)) = 0.05
		_SpecularIntensity("Specular Intensity", Range(64, 512)) = 1
		_SpecularColor("Specular Color", Color) = (0, 0, 0, 0)
		//_ControlMapTilling("Tilling ControlMap", Range(1, 70)) = 1

		[Header(Layer Background)]
		[Space(5)]
		_Layer01Tilling("Tilling Layer 01", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer01("Layer01 Albedo (A for Spec)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer01_Normal("Layer01 Normal", 2D) = "bump" {}
		_TexLayer01_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 02)]
		[Space(5)]
		_Layer02Tilling("Tilling Layer 02", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer02("Layer02 Albedo (A for Spec)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer02_Normal("Layer02 Normal", 2D) = "bump" {}
		_TexLayer02_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 03)]
		[Space(5)]
		_Layer03Tilling("Tilling Layer 03", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer03("Layer03 Albedo (A for Spec)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer03_Normal("Layer03 Normal", 2D) = "bump" {}
		_TexLayer03_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 04)]
		[Space(5)]
		_Layer04Tilling("Tilling Layer 04", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer04("Layer04 Albedo (A for Spec)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer04_Normal("Layer04 Normal", 2D) = "bump" {}
		_TexLayer04_Normal_Intensity("Normal Scale", Range(0, 2)) = 1

	}

	SubShader {
		Tags {"RenderType" = "Opaque" "Queue" = "Geometry-100" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
		Pass {
			Tags {"LightMode" = "UniversalForward"}
			Name "Layered Pass"
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

			#pragma multi_compile_instancing

			#pragma vertex vert
            #pragma fragment frag

			#pragma shader_feature _CONTROLMAP_SHOW

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			CBUFFER_START(UnityPerMaterial)
			half _SpecularIntensity;
			half4 _SpecularColor;
			half _Weight;

			float4 _ControlMap_ST;
			half _ControlMapTilling;

			half _TexLayer01_Normal_Intensity;
			half _Layer01Tilling;

			half _TexLayer02_Normal_Intensity;
			half _Layer02Tilling;

			half _TexLayer03_Normal_Intensity;
			half _Layer03Tilling;

			half _TexLayer04_Normal_Intensity;
			half _Layer04Tilling;

			CBUFFER_END

			TEXTURE2D(_ControlMap);       SAMPLER(sampler_ControlMap);

			TEXTURE2D(_TexLayer01);       SAMPLER(sampler_TexLayer01);
			TEXTURE2D(_TexLayer02);       SAMPLER(sampler_TexLayer02);
			TEXTURE2D(_TexLayer03);       SAMPLER(sampler_TexLayer03);
			TEXTURE2D(_TexLayer04);       SAMPLER(sampler_TexLayer04);
			
			TEXTURE2D(_TexLayer01_Normal);       SAMPLER(sampler_TexLayer01_Normal);
			TEXTURE2D(_TexLayer02_Normal);       SAMPLER(sampler_TexLayer02_Normal);
			TEXTURE2D(_TexLayer03_Normal);       SAMPLER(sampler_TexLayer03_Normal);
			TEXTURE2D(_TexLayer04_Normal);       SAMPLER(sampler_TexLayer04_Normal);

			struct a2v {
				float4 posOS : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 lightmapUV : TEXCOORD1;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 positionCS : SV_POSITION;
				float2 uvForControlMap : TEXCOORD0;

				DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

				float4 uvForLayer01AndLayer02 : TEXCOORD2;
				float4 uvForLayer03AndLayer04 : TEXCOORD3;


				half4 normalWSAndViewDirX : TEXCOORD4;
				half4 tangentWSAndViewDirY : TEXCOORD5;
				half4 bitangentWSAndViewDirZ : TEXCOORD6;

				float4 positionWSWithFogFactor : TEXCOORD7;

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					float4 shadowCoord : TEXCOORD8;
				#endif

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};

			v2f vert(a2v v) {
				v2f o = (v2f)0;
				// for enable gpu instance
				UNITY_SETUP_INSTANCE_ID(v)
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				// vertexPositionInputs
				VertexPositionInputs vpi = GetVertexPositionInputs(v.posOS.xyz);
				o.positionCS = vpi.positionCS;
				o.positionWSWithFogFactor.xyz = vpi.positionWS;

				// fog
				o.positionWSWithFogFactor.w = ComputeFogFactor(vpi.positionCS.z);

				o.uvForControlMap = TRANSFORM_TEX(v.texcoord, _ControlMap);
				
				o.uvForLayer01AndLayer02.xy = v.texcoord.xy * _Layer01Tilling;
				o.uvForLayer01AndLayer02.zw = v.texcoord.xy * _Layer02Tilling;
				o.uvForLayer03AndLayer04.xy = v.texcoord.xy * _Layer03Tilling;
				o.uvForLayer03AndLayer04.zw = v.texcoord.xy * _Layer04Tilling;

				// for normal and viewDir
				half3 viewDirWS = GetCameraPositionWS() - vpi.positionWS;
				VertexNormalInputs vni = GetVertexNormalInputs(v.normalOS, v.tangentOS);
				o.normalWSAndViewDirX = half4(vni.normalWS, viewDirWS.x);
				o.tangentWSAndViewDirY = half4(vni.tangentWS, viewDirWS.y);
				o.bitangentWSAndViewDirZ = half4(vni.bitangentWS, viewDirWS.z);

				// shadowCoord
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					o.shadowCoord = GetShadowCoord(vpi);
				#endif

				// GI
				OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);
				OUTPUT_SH(o.normalWSAndViewDirX.xyz, o.vertexSH);

				return o;
			}

			half4 GetControlValue(half weight, half4 controlColor) {
				half4 controlValue;
				controlValue = controlColor;
				half4 maxControl = controlValue - (max(controlValue.r, max(controlValue.g, max(controlValue.b, controlValue.a))));
				half4 withWeight = max(maxControl + weight, half4(0, 0, 0, 0)) * controlColor;
				half4 finalValue = withWeight / (withWeight.r + withWeight.g + withWeight.b + withWeight.a);

				return finalValue;
			}

			half4 GetMixedAlbedoAndSpecular(half4 controlColor, half4 albedo01, half4 albedo02, half4 albedo03, half4 albedo04) {
				albedo01 = albedo01 * controlColor.r;
				albedo02 = albedo02 * controlColor.g;
				albedo03 = albedo03 * controlColor.b;
				albedo04 = albedo04 * controlColor.a;
				return albedo01 + albedo02 + albedo03 + albedo04;
			}

			half3 GetMixedNormalTS(half4 controlColor, half3 normal01, half3 normal02, half3 normal03, half3 normal04) {
				normal01 = normal01 * controlColor.r;
				normal02 = normal02 * controlColor.g;
				normal03 = normal03 * controlColor.b;
				normal04 = normal04 * controlColor.a;
				return normalize(normal01 + normal02 + normal03 + normal04);
			}



			half4 SampleNormalWithScale(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = 1.0h) {
				half4 normalTS = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
				return half4(UnpackNormalScale(normalTS, scale).xyz, normalTS.w);
			}


			void InitializeInputData(v2f i, half3 normalTS, out InputData inputData) {
				inputData = (InputData)0;

				inputData.positionWS = i.positionWSWithFogFactor.xyz;

				half3 viewDirWS = half3(i.normalWSAndViewDirX.w, i.tangentWSAndViewDirY.w, i.bitangentWSAndViewDirZ.w);
				inputData.viewDirectionWS = SafeNormalize(viewDirWS);

				inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(i.tangentWSAndViewDirY.xyz, i.bitangentWSAndViewDirZ.xyz, i.normalWSAndViewDirX.xyz));
				inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					inputData.shadowCoord = i.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					inputData.shadowCoord = TransformWorldToShadowCoord(i.positionWSWithFogFactor.xyz);
				#else
					inputData.shadowCoord = float4(0, 0, 0, 0);
				#endif

				inputData.fogCoord = i.positionWSWithFogFactor.w;
				inputData.vertexLighting = half3(0, 1, 0);

				inputData.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, inputData.normalWS);
			}




			half4 frag(v2f i) : SV_TARGET {
				half4 controlColor = SAMPLE_TEXTURE2D(_ControlMap, sampler_ControlMap, i.uvForControlMap);
				#ifdef _CONTROLMAP_SHOW
					return controlColor;
				#endif

				half4 controlValue = GetControlValue(_Weight, controlColor);

				// get mixed albedo
				half4 color_Layer01_Albedo = SAMPLE_TEXTURE2D(_TexLayer01, sampler_TexLayer01, i.uvForLayer01AndLayer02.xy);
				half4 color_Layer02_Albedo = SAMPLE_TEXTURE2D(_TexLayer02, sampler_TexLayer02, i.uvForLayer01AndLayer02.zw);
				half4 color_Layer03_Albedo = SAMPLE_TEXTURE2D(_TexLayer03, sampler_TexLayer03, i.uvForLayer03AndLayer04.xy);
				half4 color_Layer04_Albedo = SAMPLE_TEXTURE2D(_TexLayer04, sampler_TexLayer04, i.uvForLayer03AndLayer04.zw);
				half4 mixedAlbedoAndSpecular = GetMixedAlbedoAndSpecular(controlValue, color_Layer01_Albedo, color_Layer02_Albedo, color_Layer03_Albedo, color_Layer04_Albedo);
				half3 mixedAlbedo = mixedAlbedoAndSpecular.rgb;
				half mixedSpecular = mixedAlbedoAndSpecular.a;

				// get mixed normal
				half4 color_Layer01_NormalTS = SampleNormalWithScale(i.uvForLayer01AndLayer02.xy, TEXTURE2D_ARGS(_TexLayer01_Normal, sampler_TexLayer01_Normal), _TexLayer01_Normal_Intensity);
				half4 color_Layer02_NormalTS = SampleNormalWithScale(i.uvForLayer01AndLayer02.zw, TEXTURE2D_ARGS(_TexLayer02_Normal, sampler_TexLayer02_Normal), _TexLayer02_Normal_Intensity);
				half4 color_Layer03_NormalTS = SampleNormalWithScale(i.uvForLayer03AndLayer04.xy, TEXTURE2D_ARGS(_TexLayer03_Normal, sampler_TexLayer03_Normal), _TexLayer03_Normal_Intensity);
				half4 color_Layer04_NormalTS = SampleNormalWithScale(i.uvForLayer03AndLayer04.zw, TEXTURE2D_ARGS(_TexLayer04_Normal, sampler_TexLayer04_Normal), _TexLayer04_Normal_Intensity);
				half3 mixedNormalTS = GetMixedNormalTS(controlValue, color_Layer01_NormalTS.rgb, color_Layer02_NormalTS.rgb, color_Layer03_NormalTS.rgb, color_Layer04_NormalTS.rgb);

				InputData inputData;
				InitializeInputData(i, mixedNormalTS, inputData);

				/*
				half NdotL = saturate(dot(i.normalWSAndViewDirX.xyz, mainLight.direction));
				half3 lambert = mainLight.color * NdotL;
				*/

				#ifdef _MAIN_LIGHT_SHADOWS
					Light mainLight = GetMainLight(inputData.shadowCoord);
				#else
					Light mainLight = GetMainLight();
				#endif

				half fresnelTerm = Pow4(1.0 - saturate(dot(inputData.normalWS, inputData.viewDirectionWS)));

				//fresnelTerm =  saturate(1 - pow(fresnelTerm, 30));
				//mixedSpecular = pow(mixedSpecular, 30);

				half3 spec = LightingSpecular(mainLight.color, mainLight.direction, inputData.normalWS, inputData.viewDirectionWS, _SpecularColor * mixedSpecular, _SpecularIntensity);

				MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));
				half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
				half3 finalColor = (inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS)) * mixedAlbedo;
				
				finalColor.rgb = MixFog(finalColor.rgb, inputData.fogCoord);

				return half4(finalColor + spec, 1);
			}

			ENDHLSL
		}
	}
}