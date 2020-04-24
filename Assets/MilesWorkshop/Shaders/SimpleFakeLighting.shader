Shader "MileShader/SimpleFakeLighting" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Brightness("Brightness", Range(0, 1)) = 0.4
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_FakeLightDir("Light Direction", Vector) = (0.25, 1, -0.25, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			float4 _Color;
			float _Brightness;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _FakeLightDir;

			struct a2v {
				float4 vertex :POSITION;
				float2 uv :TEXCOORD0;
				half3 normal :NORMAL;
			};

			struct v2f {
				float4 pos :SV_POSITION;
				float2 uv :TEXCOORD0;
				half3 normal_World :TEXCOORD2;
			};

			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float4x4 modelMatrixInverse = unity_WorldToObject;
				o.normal_World = mul(half4(v.normal, 0.0), modelMatrixInverse).xyz;
				return o;
			}

			half4 frag(v2f i) : SV_Target {
				half4 col = tex2D(_MainTex, i.uv);

				
				return col;
			}
			ENDCG
		}
	}
}