Shader "SoFunny/SPToon_Terrain2018_v1" {
	Properties {
		[Toggle(_CONTROLMAP_SHOW)]_ShowControlMap("Show Control Map", float) = 0
		_ControlMap("ControlMap", 2D) = "white" {} 
		_Weight("Blend Weight", Range(0.001, 1)) = 0.05
		[Toggle(_VERTEXCOLOR_SHOW)]_ShowVertexColor("Show Vertex Color", float) = 0
		[Toggle(_USE_VERTEXCOLOR)]_UseVertexColor("Use Vertex Color", float) = 0
		_VertexColorIntensity("VertexColor Intensity", Range(0, 1)) = 1
		//_Specular("Specular Intensity", Range(0, 1)) = 1
		//_Gloss("Gloss Intensity", Range(64, 512)) = 64
		//_SpecColor("Specular Color", Color) = (0, 0, 0, 0)
		_MainColorEffectEnvironment("Main Color", Color) = (1, 1, 1, 1)
		_EnvironmentLightingIntensity("Environment Lighting Intensity", Range(0, 1)) = 0.5
		_RimColor("Rim Color", Color) = (1, 1, 1, 1)
		_RimArea("Rim Area", Range(0, 1)) = 0.8
		
		//_ControlMapTilling("Tilling ControlMap", Range(1, 70)) = 1

		[Header(Layer Background)]
		[Space(5)]
		//_Layer01Tilling("Tilling Layer 01", Range(1, 70)) = 1
		_TexLayer01("Layer01 Albedo (A for Spec)", 2D) = "white" {}
		//[NoScaleOffset][Normal]_TexLayer01_Normal("Layer01 Normal", 2D) = "bump" {}
		//_TexLayer01_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 02)]
		[Space(5)]
		//_Layer02Tilling("Tilling Layer 02", Range(1, 70)) = 1
		_TexLayer02("Layer02 Albedo (A for Spec)", 2D) = "white" {}
		//[NoScaleOffset][Normal]_TexLayer02_Normal("Layer02 Normal", 2D) = "bump" {}
		//_TexLayer02_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 03)]
		[Space(5)]
		//_Layer03Tilling("Tilling Layer 03", Range(1, 70)) = 1
		_TexLayer03("Layer03 Albedo (A for Spec)", 2D) = "white" {}
		//[NoScaleOffset][Normal]_TexLayer03_Normal("Layer03 Normal", 2D) = "bump" {}
		//_TexLayer03_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 04)]
		[Space(5)]
		//_Layer04Tilling("Tilling Layer 04", Range(1, 70)) = 1
		_TexLayer04("Layer04 Albedo (A for Spec)", 2D) = "white" {}
		//[NoScaleOffset][Normal]_TexLayer04_Normal("Layer04 Normal", 2D) = "bump" {}
		//_TexLayer04_Normal_Intensity("Normal Scale", Range(0, 2)) = 1

		[Space(5)]
		[Toggle(_CUSTOMFOG)]_CustomFog("Custom Fog", float) = 0
		_FogColor("Fog Color", Color) = (0, 0, 0, 0)

	}
	SubShader {
		Tags {"RenderType" = "Opaque" "Queue" = "Geometry"}

		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature _CONTROLMAP_SHOW
			#pragma shader_feature _VERTEXCOLOR_SHOW
			#pragma shader_feature _USE_VERTEXCOLOR
			#pragma shader_feature _CUSTOMFOG

			#pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			struct a2v {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				half3 normalOS : NORMAL;
				half3 tangentOS : TANGENT;
				half4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uvForControlMap : TEXCOORD0;
				float4 ambientOrLightmapUV : TEXCOORD1;
				float4 uvForLayer01AndLayer02 : TEXCOORD2;
				float4 uvForLayer03AndLayer04 : TEXCOORD3;
				half3 normalWS : TEXCOORD4;
				half4 vertexColor : TEXCOORD5;
				float4 positionWS : TEXCOORD6;
				SHADOW_COORDS(7)
				UNITY_FOG_COORDS(8)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _ControlMap;
			float4 _ControlMap_ST;
			half4 _MainColorEffectEnvironment;
			half4 _FogColor;
			half unityFogFactor;
			half _Weight;
			half _Specular;
			half _Gloss;
			half _RimArea;
			half4 _RimColor;
			half _VertexColorIntensity;
			half _EnvironmentLightingIntensity;

			sampler2D _TexLayer01;
			float4 _TexLayer01_ST;
			sampler2D _TexLayer02;
			float4 _TexLayer02_ST;
			sampler2D _TexLayer03;
			float4 _TexLayer03_ST;
			sampler2D _TexLayer04;
			float4 _TexLayer04_ST;

			inline half4 VertexGI(a2v v, float3 posWorld, half3 normalWorld) {
				half4 ambientOrLightmapUV = 0;
				// Static lightmaps
				#ifdef LIGHTMAP_ON
					ambientOrLightmapUV.xy = v.uv2.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					ambientOrLightmapUV.zw = 0;
				// Sample light probe for Dynamic objects only (no static or dynamic lightmaps)
				#elif UNITY_SHOULD_SAMPLE_SH
					#ifdef VERTEXLIGHT_ON
						// Approximated illumination from non-important point lights
						ambientOrLightmapUV.rgb = Shade4PointLights (
							unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
							unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
							unity_4LightAtten0, posWorld, normalWorld);
					#endif

					ambientOrLightmapUV.rgb = ShadeSHPerVertex (normalWorld, ambientOrLightmapUV.rgb);
				#endif

				#ifdef DYNAMICLIGHTMAP_ON
					ambientOrLightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				
				return ambientOrLightmapUV;
			}

			v2f vert(a2v v) {
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.positionWS.xyz = mul(unity_ObjectToWorld, v.vertex);

				o.uvForControlMap = TRANSFORM_TEX(v.uv, _ControlMap);
				o.vertexColor = v.vertexColor;

				o.normalWS.xyz = normalize(UnityObjectToWorldNormal(v.normalOS));
				o.ambientOrLightmapUV = VertexGI(v, o.positionWS.xyz, o.normalWS.xyz);

				o.uvForLayer01AndLayer02.xy = v.uv.xy * _TexLayer01_ST.xy;
				o.uvForLayer01AndLayer02.zw = v.uv.xy * _TexLayer02_ST.xy;
				o.uvForLayer03AndLayer04.xy = v.uv.xy * _TexLayer03_ST.xy;
				o.uvForLayer03AndLayer04.zw = v.uv.xy * _TexLayer04_ST.xy;

				o.positionWS.w = o.pos.z;
				UNITY_TRANSFER_FOG(o,o.pos);
				TRANSFER_SHADOW(o);
				return o;
			}
					
			UnityLight MainLight() {
				UnityLight l;
				l.color = _LightColor0.rgb;
				l.dir = _WorldSpaceLightPos0.xyz;
				return l;
			}
			
			inline UnityGIInput InitializeGI(v2f i, float3 posWS, float2 lightmapUV, half3 ambientColor) {
				UnityGIInput gi;
				gi.light = MainLight();
				gi.light.ndotl = max(0, dot(i.normalWS.xyz, _WorldSpaceLightPos0.xyz));
				gi.worldPos = posWS;
				gi.worldViewDir = normalize(_WorldSpaceCameraPos - posWS);
				UNITY_LIGHT_ATTENUATION(atten, i, posWS);
				gi.atten = atten;
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
					gi.ambient = 0;
					gi.lightmapUV = float4(lightmapUV, 0, 0);
				#else
					gi.ambient = ambientColor;
					gi.lightmapUV = 0;
				#endif

				gi.probeHDR[0] = unity_SpecCube0_HDR;
				gi.probeHDR[1] = unity_SpecCube1_HDR;
				#if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
					gi.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
				#endif
				#ifdef UNITY_SPECCUBE_BOX_PROJECTION
					gi.boxMax[0] = unity_SpecCube0_BoxMax;
					gi.probePosition[0] = unity_SpecCube0_ProbePosition;
					gi.boxMax[1] = unity_SpecCube1_BoxMax;
					gi.boxMin[1] = unity_SpecCube1_BoxMin;
					gi.probePosition[1] = unity_SpecCube1_ProbePosition;
				#endif
				return gi;
			}

			inline SurfaceOutput InitializedMySurface(half3 albedo, half3 normalWS, half3 emissive, half spec, half gloss, half alpha) {
				SurfaceOutput surface;
				surface.Albedo = albedo;
				surface.Normal = normalWS;
				surface.Emission = emissive;
				surface.Specular = spec;
				surface.Gloss = gloss;
				surface.Alpha = alpha;
				return surface;
			}

			// 
			half4 GetMixedAlbedoAndSpecular(half4 controlValue, half4 albedo01, half4 albedo02, half4 albedo03, half4 albedo04) {
				albedo01 = albedo01 * controlValue.r;
				albedo02 = albedo02 * controlValue.g;
				albedo03 = albedo03 * controlValue.b;
				albedo04 = albedo04 * controlValue.a;
				return albedo01 + albedo02 + albedo03 + albedo04;
			}

			// get control value which indicates how r g b a color mixes
			half4 GetControlValue(half weight, half4 controlColor) {
				half4 controlValue;
				controlValue = controlColor;
				half4 maxControl = controlValue - (max(controlValue.r, max(controlValue.g, max(controlValue.b, controlValue.a))));
				half4 withWeight = max(maxControl + weight, half4(0, 0, 0, 0)) * controlColor;
				half4 finalValue = withWeight / (withWeight.r + withWeight.g + withWeight.b + withWeight.a);
				return finalValue;
			}

			half4 frag(v2f i) : SV_TARGET {
				// control map
				half4 ControlMapColor = tex2D(_ControlMap, i.uvForControlMap);

				#ifdef _CONTROLMAP_SHOW
					return ControlMapColor;
				#endif

				half4 controlValue = GetControlValue(_Weight, ControlMapColor);

				#ifdef _VERTEXCOLOR_SHOW
					return i.vertexColor;
				#endif

				// get mixed color
				half4 color_Layer01 = tex2D(_TexLayer01, i.uvForLayer01AndLayer02.xy);
				half4 color_Layer02 = tex2D(_TexLayer02, i.uvForLayer01AndLayer02.zw);
				half4 color_Layer03 = tex2D(_TexLayer03, i.uvForLayer03AndLayer04.xy);
				half4 color_Layer04 = tex2D(_TexLayer04, i.uvForLayer03AndLayer04.zw);
				half4 mixedColor = GetMixedAlbedoAndSpecular(controlValue, color_Layer01, color_Layer02, color_Layer03, color_Layer04);
				
				// environment Lighting
				UnityGIInput giInput = InitializeGI(i, i.positionWS.xyz, i.ambientOrLightmapUV.xy, i.ambientOrLightmapUV.rgb);
				half occlusion = 1.0;
				UnityGI gi = UnityGI_Base(giInput, occlusion, i.normalWS.xyz);
				gi.indirect.diffuse *= _EnvironmentLightingIntensity * _MainColorEffectEnvironment.rgb;

				// environment Specular
				Unity_GlossyEnvironmentData glossyInput = UnityGlossyEnvironmentSetup(1.0, giInput.worldViewDir, i.normalWS.xyz, float3(0, 0, 0));
				gi.indirect.specular = UnityGI_IndirectSpecular(giInput, occlusion, glossyInput);

				// built-in SurfaceOutput will be 
				SurfaceOutput surface = InitializedMySurface( mixedColor, i.normalWS.xyz, half3(0, 0, 0), 0, 0, 1);

				// fresnelTerm
				half fresnelTerm = Pow4(1.0 - saturate(dot(i.normalWS.xyz, giInput.worldViewDir)));
				fresnelTerm = Pow4(smoothstep(_RimArea, 1, fresnelTerm));

				// lambert
				half4 finalLambertColor = LightingLambert(surface, gi);
				//half4 finalBlinnPhong = LightingBlinnPhong(surface, giInput.worldViewDir, gi);
				
				// get rim Color
				_RimColor = _RimColor * fresnelTerm;
				finalLambertColor.rgb += _RimColor.rgb;
				
				// use vertexColor
				#ifdef _USE_VERTEXCOLOR
					i.vertexColor.rgb =  _VertexColorIntensity * i.vertexColor.rgb + ( 1 - _VertexColorIntensity);
					finalLambertColor.rgb *= i.vertexColor.rgb ;
				#endif

				// fog
				#ifdef _CUSTOMFOG
					UNITY_APPLY_FOG_COLOR(i.fogCoord, finalLambertColor, _FogColor);
				#else
					UNITY_APPLY_FOG(i.fogCoord, finalLambertColor);
				#endif

				return finalLambertColor;
			}
			ENDCG
		}
		
		UsePass "Mobile/VertexLit/SHADOWCASTER"
	}
}