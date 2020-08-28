Shader "MileShader/SimpleListWithCubemap" {
	Properties {
		_Roughness("Reflection Roughness", Range(0.0, 1.0)) = 0.5
		_Occlusion("Reflection Occlusion", Range(0.0, 1.0)) = 0.5
	}
	SubShader {
		Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
		Pass {
			Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

			#pragma vertex vert
			#pragma fragment frag

			
			half _Roughness;
			half _Occlusion;
			
			struct a2v {
				float4 positionOS : POSITION;
				half3 normal : NORMAL;
			};

			struct v2f {
				float4 positionCS : SV_POSITION;
				half3 normalWS : TEXCOORD2;
				half3 viewDirWS : TEXCOORD3;
			};

			v2f vert(a2v v) {
				v2f o = (v2f)0;
				VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
				o.positionCS = vpi.positionCS;
				VertexNormalInputs vni = GetVertexNormalInputs(v.normal);
				o.normalWS = normalize(vni.normalWS);
				o.viewDirWS = normalize(GetCameraPositionWS() - vpi.positionWS);
				return o;
			}

			half4 frag(v2f i) : SV_TARGET {
				half3 indirectReflection = GlossyEnvironmentReflection(i.viewDirWS, _Roughness, _Occlusion);
				return half4(indirectReflection, 1);
			}

			ENDHLSL
		}
	}
	FallBack "Universal Render Pipeline/Simple Lit"
}