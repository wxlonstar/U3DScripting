Shader "Effect/shd_light_v1"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", float) = 2
        _TintColor ("Tint Color", COLOR) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        
        _NoiseTex ("Noise Texture", 2D) = "black" {}
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 1
        _NoiseUSpeed ("Gradient U Speed", Float ) = 0
        _NoiseVSpeed ("Gradient V Speed", Float ) = 0

        _FadeOut ("Fade Out", float) = 1
    }
    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass {
            Tags {"LightMode"="UniversalForward"}
            Blend One One
            Cull [_Cull]
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _TintColor;
            
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _NoiseUSpeed;
            float _NoiseVSpeed;
            float _NoiseIntensity;

            float _FadeOut;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                half4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 viewDir : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                half3 worldTangent : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                float3 positionWS = TransformObjectToWorld( v.vertex.xyz);
                o.pos=   TransformWorldToHClip(positionWS);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.viewDir = _WorldSpaceCameraPos - positionWS;
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                VertexNormalInputs vni = GetVertexNormalInputs(v.normal, v.tangent);
                o.worldTangent = vni.bitangentWS;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Dir
                float3 viewDir = normalize(i.viewDir);
                float3 worldNormal = normalize(i.worldNormal);
                worldNormal = worldNormal;
                half3 worldTangent = -1 * normalize(i.worldTangent);

                float fadeOutValue = max(0.0001, _FadeOut);
                float vDotN = saturate(dot(viewDir, worldNormal));
                float fresnel = pow(vDotN, fadeOutValue);
                //half vDotT = saturate(dot(viewDir, worldTangent));

                half4 mainCol = tex2D(_MainTex, i.uv.xy) * _TintColor;
                half4 noiseCol = tex2D(_NoiseTex, i.uv.zw + _Time.y * float2(_NoiseUSpeed, _NoiseVSpeed));
                noiseCol += 1 - _NoiseIntensity;
                noiseCol = saturate(noiseCol);

                half4 finalCol = (mainCol * noiseCol) * fresnel;
                return finalCol;
                //return half4(vDotT, vDotT, vDotT, 1);

            }
            ENDHLSL
        }
    }
}
