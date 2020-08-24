Shader "Hidden/Radial Blur"
{
	Properties
	{
		_MainTex ("Input", 2D) = "white" {}
    	_BlurStrength ("Blur Strength", Float) = 0.5
    	_BlurWidth ("Blur Width", Float) = 0.5
    	_Center ("Center", Vector) = (0.5, 0.5, 0, 0 )
	}
	SubShader
	{
		ZTest Off Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 dirVal : TEXCOORD1;
        		float distVal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float _BlurStrength;
    		float _BlurWidth;
    		float4 _Center;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				float2 dir = _Center.xy - o.uv.xy;
				float dist = sqrt(dir.x*dir.x + dir.y*dir.y);
				//dir = dir/dist;
				o.dirVal = dir *  _BlurWidth;
				o.distVal = dist * _BlurStrength;

				return o;
			}
			
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv.xy);

				float4 sum = col;
				sum += tex2D(_MainTex, i.uv.xy -0.08 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy -0.05 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy -0.03 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy -0.02 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy -0.01 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy +0.01 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy +0.02 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy +0.03 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy +0.05 * i.dirVal);
				sum += tex2D(_MainTex, i.uv.xy +0.08 * i.dirVal);
				sum *= 1.0/11.0;

				float t = saturate(i.distVal);
				
				return lerp(col, sum, t);
			}
			ENDCG
		}
	}
}
