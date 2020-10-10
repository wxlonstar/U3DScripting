Shader "SoFunny/SPToon/SPToonDisplace"
{
    Properties
    {
        [Header(MainToon)]
        _MainTex ("主贴图 Main Tex", 2D) = "white" {}
        _DispTex ("移位贴图 _DispTex", 2D) = "gray" {}
        _Displacement ("移位值 _Displacement", Range(0, 2.0)) = 1.0
        _FlowSpeed ("流动速度 _FlowSpeed",Range(-3,3))=1
        [Space(10)]
        _Color ("主颜色 Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("自发光色 Emission Color", Color) = (0, 0, 0, 1)
        _HighlightColor("亮部色 Highlight Color",Color)=(1.0,1.0,1.0,1)
        _ShadowColor("暗部色 Shadow Color",Color)=(0.6,0.6,0.6,1)
        [Space(10)]
        _LightThreshold("亮暗部阈值 Light Threshold",Range(-1,1))=0.65
        _ShadowIntensity("阴影强度 Shadow Intensity",Range(0,3))=1.0
        _TransitionIntensity("过渡强度 Transition Intensity",Range(0,3))=0.2
        //[Header(Ambient)]
        // [Toggle(USE_AMBIENTLIGHT)] _UseAmbientLight("计算环境光 Use Ambient Light",Float)=1
        //_MainAmbientSensitivity("主环境光敏感度 Main Ambient Sensitivity", Range(0,1)) = 0.4
        [Header(Rim)]
        [Toggle(DRAW_RIM)] _Rim("开启边缘光 Draw Rim",Float)=0
        _RimColor ("边缘光颜色 Rim Color", Color) = (0.5,0.5,0.5,0.5)
        _FPower("幂 FPower Fresnel", Range(0,10)) = 4.0
        _F0("反射系数 F0 Fresnel", Float) = 0.05
        [Header(LightMap)]
        _LightmapIntensity("光照贴图强度 Lightmap Intensity",Range(-2,2))=0.5
        _LightmapAdd("光照贴图增益 Lightmap Add",Range(-2,2))=0.5
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            //Tags {"LightMode"="ForwardBase"}
            Tags {"LightMode" = "UniversalForward"}
            LOD 100
            //CGPROGRAM
            HLSLPROGRAM
            #pragma multi_compile_fog
            //#pragma multi_compile __ LIGHTMAP_ON
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile __ DRAW_RIM
            // #pragma multi_compile __ USE_AMBIENTLIGHT
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile_fwdbase
            // #include "UnityCG.cginc"
            // #include "Lighting.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            sampler2D _DispTex;
            
            CBUFFER_START(UnityPerMaterial)

            half4 _MainTex_ST;
            half4 _MainTex_TexelSize;
            
            half4 _DispTex_ST;
            half4 _DispTex_TexelSize;
            half _Displacement;
            half _FlowSpeed;

            half _OutlineThreshold;
            half _LightThreshold;
            half _SampleDistance;
            half4 _EdgeColor;
            half4 _BackgroundColor;
            half _NThreshold;
            half _ZThreshold;
            half4 _Color;
            half _LightmapIntensity;
            half _LightmapAdd;

            half4 _HighlightColor;
            half4 _ShadowColor;
            half _HighlightIntensity;
            half _ShadowIntensity;
            half _TransitionIntensity;

            half4 _EmissionColor;

            half4 _RimColor;
            half _FPower;
            half _F0;

            //half _MainAmbientSensitivity;

            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal:NORMAL;
                float2 uv_MainTex : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                half4 color : COLOR;
                float2 lightmapUV : TEXCOORD3;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv_MainTex : TEXCOORD1;
                float4 screenPos:TEXCOORD2;
                float3 worldNormal:TEXCOORD3;
                float3 worldPos:TEXCOORD4;
                half4 color : COLOR;
                //float2 lightMap:TEXCOORD5;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 5);
                // SHADOW_COORDS(6)
                // UNITY_FOG_COORDS(7)
                //UNITY_FOG_COORDS(6)
                float fogCoord : TEXCOORD6;
            };

            v2f vert (appdata v)
            {
                
                v2f o;
                //UNITY_INITIALIZE_OUTPUT(v2f,o);

                float d = saturate(tex2Dlod(_DispTex, float4(v.uv.xy*_DispTex_ST.xy-float2(0,_Time.x*_FlowSpeed),0,0)).b * _Displacement);
                v.vertex.xyz += (v.normal *  (1-pow(d,4)));

                //o.vertex = UnityObjectToClipPos(v.vertex);
                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.vertex = TransformWorldToHClip(positionWS);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //光照贴图
                //光照贴图使用uv2，因此这里用uv_MainTex
                // #if LIGHTMAP_ON
                //     o.lightMap = v.uv_MainTex.xy*unity_LightmapST.xy+unity_LightmapST.zw;
                // #endif

                //雾
                //UNITY_TRANSFER_FOG(o,o.vertex);
                o.fogCoord = ComputeFogFactor(o.vertex.z);

                // 用逆矩阵转换法线至世界空间
                //o.worldNormal = mul(v.normal, (half3x3)unity_WorldToObject);
                //o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal.xyz = TransformObjectToWorldNormal(v.normal);
                o.worldPos = positionWS;
                o.color = v.color;
                #if DRAW_RIM
                    //half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    half3 viewDir = normalize(TransformWorldToObject(_WorldSpaceCameraPos) - v.vertex);
                    half fresnel = saturate(1.0 - dot(v.normal, viewDir));
                    fresnel = pow(fresnel, _FPower);
                    
                    fresnel = _F0 + (1.0 - _F0) * fresnel;
                    o.color *= fresnel;
                #endif

                OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);

                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                Light mainLight = GetMainLight();
                half4 originColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);;

                //光照贴图
                #if LIGHTMAP_ON
                    //half3 lm=DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,i.lightMap.xy));
                    //originColor.rgb*=lm*_LightmapIntensity+_LightmapAdd;
                    float3 lm = SAMPLE_GI(i.lightmapUV, i.vertexSH, normalize(i.worldNormal.xyz));
                    originColor.rgb *= lm * _LightmapIntensity + _LightmapAdd;
                #endif

                //亮部强度根据阴影强度来计算
                _HighlightIntensity=_ShadowIntensity;
                _ShadowIntensity=_ShadowIntensity*0.6;

                //half3 N=normalize(i.worldNormal);
                //half3 L=normalize(UnityWorldSpaceLightDir(i.worldPos));
                float3 N = normalize(i.worldNormal.xyz);
                float3 L = mainLight.direction;

                half dotResult=(1-dot(N,L));
                half dotLightStep=step((1-_LightThreshold),dotResult)+dotResult*_TransitionIntensity;
                //前有叠加，防止值超出1造成阴影颜色过深
                dotLightStep=saturate(dotLightStep);

                //_ShadowColor = lerp(_HighlightColor, _ShadowColor, _LightColor0*_ShadowIntensity*0.5);
                //_HighlightColor = lerp(_HighlightColor, _ShadowColor , -_LightColor0*_HighlightIntensity*0.5);
                _ShadowColor = lerp(_HighlightColor, _ShadowColor, half4(mainLight.color,1) * _ShadowIntensity * 0.3);
                _HighlightColor = lerp(_HighlightColor, _ShadowColor, -half4(mainLight.color,1) * _ShadowIntensity * 0.5);

                // #if USE_AMBIENTLIGHT
                //     half4 ambientEquator=unity_AmbientEquator*0.2+0.8;
                //     half4 ambientGround=unity_AmbientGround*0.2+0.8;
                //     half4 outputColor=originColor*(_HighlightColor*(1-dotLightStep)*ambientEquator+_ShadowColor*dotLightStep*ambientGround)*0.7;
                // #else
                //     half4 outputColor=originColor*(_HighlightColor*(1-dotLightStep)+_ShadowColor*dotLightStep)*0.7;
                // #endif
                half4 outputColor=originColor*(_HighlightColor*(1-dotLightStep)+_ShadowColor*dotLightStep)*0.7;

                half4 result=outputColor*_Color*(_EmissionColor+2);

                #if DRAW_RIM
                    result=saturate(2.0f * i.color * _RimColor) + result;
                #endif

                //环境光拟合
                //result=result*(UNITY_LIGHTMODEL_AMBIENT*_MainAmbientSensitivity+(1-_MainAmbientSensitivity));
                //result=result*(_LightColor0*0.2+0.8);
                result = result * (half4(mainLight.color,1) * 0.2 + 0.8);
                    

                //雾（在最后添加比较美观）
                //UNITY_APPLY_FOG(i.fogCoord, result);
                result = half4(MixFog(result, i.fogCoord),1);
                return result;
            }
            //ENDCG
            ENDHLSL
        }
    }
}
