Shader "MileShader/ParticleTop" {
    Properties {
        _Color1("Color1", Color) = (1, 1, 1, 1)
        _Color2("Color2", Color) = (1, 1, 1, 1)
        _Color3("Color3", Color) = (1, 1, 1, 1)
        [Toggle]_ANOTHER("Another Prop", Float) = 0
    }
        SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        
        Pass {
            
            Stencil {
                Ref 2
                Comp Always
                Pass Replace
                //ZFail DecrWrap
            }

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target {
                return half4(1,0,0,0.1);
            }
            ENDCG
        }
    }

}
