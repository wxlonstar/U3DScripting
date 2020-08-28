Shader "Unlit/TestGPUInstance"
{
    Properties
    {
    }
    SubShader
    {
       Pass {
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct a2v {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
            };

            v2f vert(a2v v) {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                o.positionCS = GetVertexPositionInputs(v.positionOS.xyz).positionCS;
                return o;
            }

            half4 frag(v2f i) : SV_TARGET {
                return half4(0, 1, 0, 1);
            }

            ENDHLSL
       }
    }
    CustomEditor "ShaderSupportGPUInstance"
}
