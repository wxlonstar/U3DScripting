Shader "MileShader/TwoPasses" {
	Properties {
		_Width("Width", Range(0, 1)) = 0
	}
	SubShader {
	
		Pass {
			name "FirstPass"
			Color(0, 1, 0, 1)
		}
		
		Pass {
			Cull Front
			ZWrite On
			name "SecondPass"
			Tags {"LightMode" = "UniversalForward"}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			half _Width;

			struct a2v {
				float4 vertex :POSITION;
				half3 normal :NORMAL;
			};

			struct v2f {
				float4 positionCS :SV_POSITION;
			};

			v2f vert(a2v v) {
				v2f o;
				o = (v2f)0;
				VertexNormalInputs vni = GetVertexNormalInputs(v.normal.xyz);
				_Width = _Width * 0.01;
				v.vertex.xyz += v.normal.xyz * _Width;

				VertexPositionInputs vpi = GetVertexPositionInputs(v.vertex.xyz);
				

				o.positionCS = vpi.positionCS;
				return o;
			}

			half4 frag(v2f i) : SV_Target {
				return half4(1, 0, 0, 1);
			}
			ENDHLSL
		}
		
	}
}