Shader "MileShader/shd_layeredTerrain_v2" {
	Properties {
		[Toggle(_CONTROLMAP_SHOW)]_ShowControlMap("Show Control Map", float) = 0
		_ControlMap("ControlMap", 2D) = "white" {} 
		_Weight("Blend Weight", Range(0.001, 1)) = 0.05
		//_ControlMapTilling("Tilling ControlMap", Range(1, 70)) = 1

		[Header(Layer Background)]
		[Space(5)]
		_Layer01Tilling("Tilling Layer 01", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer01("Layer01 Albedo (A for AO)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer01_Normal("Layer01 Normal (A for Specular)", 2D) = "bump" {}
		_TexLayer01_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 02)]
		[Space(5)]
		_Layer02Tilling("Tilling Layer 02", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer02("Layer02 Albedo (A for AO)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer02_Normal("Layer02 Normal (A for Specular)", 2D) = "bump" {}
		_TexLayer02_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 03)]
		[Space(5)]
		_Layer03Tilling("Tilling Layer 03", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer03("Layer03 Albedo (A for AO)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer03_Normal("Layer03 Normal (A for Specular)", 2D) = "bump" {}
		_TexLayer03_Normal_Intensity("Normal Scale", Range(0, 2)) = 1
		[Space(20)]

		[Header(Layer 04)]
		[Space(5)]
		_Layer04Tilling("Tilling Layer 04", Range(1, 70)) = 1
		[NoScaleOffset]_TexLayer04("Layer04 Albedo (A for AO)", 2D) = "white" {}
		[NoScaleOffset][Normal]_TexLayer04_Normal("Layer04 Normal (A for Specular)", 2D) = "bump" {}
		_TexLayer04_Normal_Intensity("Normal Scale", Range(0, 2)) = 1

	}

	SubShader {
		Tags {"RenderType" = "Opaque" "Queue" = "Geometry" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
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
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 positionCS : SV_POSITION;
				float2 uvForControlMap : TEXCOORD0;
				float4 uvForLayer01AndLayer02 : TEXCOORD1;
				float4 uvForLayer03AndLayer04 : TEXCOORD2;
				
				#ifdef _MAIN_LIGHT_SHADOWS
					float4 shadowCoord : TEXCOORD4;
				#endif
				
			};

			v2f vert(a2v v) {
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v)
				VertexPositionInputs vpi = GetVertexPositionInputs(v.posOS.xyz);
				o.positionCS = vpi.positionCS;
				o.uvForControlMap = TRANSFORM_TEX(v.texcoord, _ControlMap);
				
				o.uvForLayer01AndLayer02.xy = v.texcoord.xy * _Layer01Tilling;
				o.uvForLayer01AndLayer02.zw = v.texcoord.xy * _Layer02Tilling;

				o.uvForLayer03AndLayer04.xy = v.texcoord.xy * _Layer03Tilling;
				o.uvForLayer03AndLayer04.zw = v.texcoord.xy * _Layer04Tilling;

				#ifdef _MAIN_LIGHT_SHADOWS
					o.shadowCoord = GetShadowCoord(vpi);
				#endif

				return o;
			}

			half3 GetMixedAlbedo(half4 controlColor, half3 albedo01, half3 albedo02, half3 albedo03, half3 albedo04) {
				albedo01 = albedo01 * controlColor.r;
				albedo02 = albedo02 * controlColor.g;
				albedo03 = albedo03 * controlColor.b;
				albedo04 = albedo04 * controlColor.a;
				return albedo01 + albedo02 + albedo03 + albedo04;
			}

			half4 frag(v2f i) : SV_TARGET {
				half4 controlColor = SAMPLE_TEXTURE2D(_ControlMap, sampler_ControlMap, i.uvForControlMap);
				#ifdef _CONTROLMAP_SHOW
					return controlColor;
				#endif

				half4 color_Layer01_Albedo = SAMPLE_TEXTURE2D(_TexLayer01, sampler_TexLayer01, i.uvForLayer01AndLayer02.xy);
				half4 color_Layer02_Albedo = SAMPLE_TEXTURE2D(_TexLayer02, sampler_TexLayer02, i.uvForLayer01AndLayer02.zw);
				half4 color_Layer03_Albedo = SAMPLE_TEXTURE2D(_TexLayer03, sampler_TexLayer03, i.uvForLayer03AndLayer04.xy);
				half4 color_Layer04_Albedo = SAMPLE_TEXTURE2D(_TexLayer04, sampler_TexLayer04, i.uvForLayer03AndLayer04.zw);

				#ifdef _MAIN_LIGHT_SHADOWS
					Light mainLight = GetMainLight(i.shadowCoord);
				#else
					Light mainLight = GetMainLight();
				#endif

				half3 finalAlbedo = GetMixedAlbedo(controlColor, color_Layer01_Albedo.rgb, color_Layer02_Albedo.rgb, color_Layer03_Albedo.rgb, color_Layer04_Albedo.rgb);



				return half4(finalAlbedo, 1);
			}

			ENDHLSL
		}
	}
}