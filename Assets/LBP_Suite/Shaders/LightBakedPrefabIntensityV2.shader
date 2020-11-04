Shader "MileShader/LightBakedPrefabIntensityV2" {
	Properties {
		_Color("Color", Color) = (0, 1, 0, 1)
	}
	SubShader {
		Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "LightweightPipeline"}
		Pass {
			Tags {"LightMode" = "LightweightForward"}
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_instancing

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
			CBUFFER_START(UnityPerMaterial)
			half4 _Color;
			CBUFFER_END


			struct Attributes {
				float4 positionOS    : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct Varyings {
				float4 positionCS               : SV_POSITION;
			};

			Varyings vert(Attributes input) {
				Varyings o = (Varyings)0;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				o.positionCS = vertexInput.positionCS;
				return o;
			}

			half4 frag(Varyings i) : SV_TARGET {
				return _Color;
			}

			ENDHLSL
		}
	}

	
}