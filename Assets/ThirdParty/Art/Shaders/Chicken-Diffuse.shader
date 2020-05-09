Shader "Chicken/Diffuse" {
	Properties {
		[NoScaleOffset]_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Toggle(_USE_VERTEX_DIFFUSE_MAINLIGHT)] _USE_VERTEX_DIFFUSE_MAINLIGHT("Vertex MainLight Diffuse", float) = 0
		_ShadowColor ("ShadowColor", Color) = (0, 0, 0, 0)
		//_BackLightColor ("BackLightColor", Color) = (0, 0, 0, 0)
		// [Toggle(_USE_LIGHTMAP)] _USE_LIGHTMAP("Light Map", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Back

		Pass {
            Name "ForwardLit"
            Tags { "LightMode" = "LightweightForward" }
			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0
			#pragma vertex SimpleLitVertex
			#pragma fragment SimpleLitFragment
			#pragma multi_compile_fog
			// Custom shader features, always enabled
			// #pragma shader_feature _USE_MAINTEX
			// Lightweight Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _USE_VERTEX_DIFFUSE_MAINLIGHT
			// #pragma multi_compile _ _USE_LIGHTMAP
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#define _USE_MAINTEX 1
			#define _USE_SHADOW_COLOR 1
			//#define _USE_BACK_LIGHTING 1
			//#define _USE_VERTEX_DIFFUSE_MAINLIGHT 1

			//// test code
			//#include "Chicken-Common.hlsl"
			//#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
			//*TEXTURE2D_SAMPLER2D(_MainTex,sampler_MainTex)
			//float4 _MainTex_ST;*/
			//struct Attribute {
			//	float4 vertex : POSITION;
			//	half3 normal : NORMAL;
			//};
			//struct Varyings {
			//	float4 positionCS:SV_POSITION;
			//	float3 color:TEXCOORD0;
			//};
			//Varyings SimpleLitVertex(Attribute input) {
			//	Varyings output;
			//	VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			//	VertexNormalInputs normalInput = GetVertexNormalInputs(input.normal);
			//	output.positionCS = vertexInput.positionCS;
			//	Light mainLight = GetMainLight();
			//	output.color = dot(mainLight.direction, normalInput.normalWS)*mainLight.color;
			//	return output;
			//}
			//half4 SimpleLitFragment(Varyings input) :SV_TARGET {
			//	return half4(input.color, 1.0);
			//}

			#include "Chicken-SimpleLitPass.hlsl"
			ENDHLSL
		}

        Pass {
            Name "ShadowCaster"
            Tags{ "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex ShadowCasterVertex
			#pragma fragment ShadowCasterFragment

			#include "Chicken-ShadowCasterPass.hlsl"
            ENDHLSL
        }
	}
}
