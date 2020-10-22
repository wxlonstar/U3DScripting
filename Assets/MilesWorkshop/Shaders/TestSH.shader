Shader "MileShader/TestSH" {
    Properties {
    /*
        _Color1("Color1", Color) = (1, 1, 1, 1)
        _Color2("Color2", Color) = (1, 1, 1, 1)
        _Color3("Color3", Color) = (1, 1, 1, 1)
        [Toggle]_ANOTHER("Another Prop", Float) = 0
        */
    }
    SubShader {
        Tags {"RenderType" = "Opaque" "Queue" = "Geometry" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        Pass {
            Name "SimpleLit"
            Tags {"LightMode" = "UniversalForward"}
            //Blend SrcAlpha OneMinusSrcAlpha
            //ZWrite Off
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
            half4 _Color1;
            CBUFFER_END

            struct a2v {
                float4 positionOS : POSITION;
                half3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 positionCS : SV_POSITION;
                half3 normalWS : TEXCOORD2;
            };


            v2f vert(a2v v) {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = vpi.positionCS;
                VertexNormalInputs vni = GetVertexNormalInputs(v.normalOS);
                o.normalWS = vni.normalWS;
                return o;
            }
            
            half4 frag(v2f i) : SV_TARGET {
                half3 sh = SampleSH(i.normalWS);
                return half4(sh, 1);
                return _Color1;
            }
            
            ENDHLSL
            
        }
        /*
        Pass {
            Name "SecondPass"
            //Blend SrcAlpha OneMinusSrcAlpha
            //ZWrite Off
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
            half4 _Color2;
            CBUFFER_END

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
                VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = vpi.positionCS;
                return o;
            }

            half4 frag(v2f i) : SV_TARGET {
                return _Color2;
                //return half4(0, 1, 0, 1);
            }
            
            ENDHLSL
        }
        */
    }
}
