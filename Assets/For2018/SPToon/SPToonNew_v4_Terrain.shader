Shader "SoFunny/SPToon/SPToonNew_v4_Terrain"
{
    Properties
    {
        [Header(MainToon)]
        _MainTex("主贴图 Main Tex", 2D) = "white" {}
        _ShadowRange("实时阴影范围 Shadow Range",Range(0,1)) = 0.5
        _ShadowStrength("实时阴影强度 Shadow Strength",Range(0,1)) = 0.65
        
        [Space(10)]
        _Color("主颜色 Color", Color) = (1, 1, 1, 1)
        _EmissionColor("自发光色 Emission Color", Color) = (0, 0, 0, 1)
        _HighlightColor("亮部色 Highlight Color",Color) = (1.0,1.0,1.0,1)
        _ShadowColor("暗部色 Shadow Color",Color) = (0.5,0.5,0.5,1)

        // [Header(SecTex)]
        // [Toggle(USE_SECTEX)] _UseSecTex("使用第二贴图 Use Sec Tex",Float) = 0
        // _SecTex("第二贴图 Sec Tex", 2D) = "black" {}
        // _SecTexStrength("第二贴图强度 SecTexStrength", Range(0,1))=0.25
        // _SecTexXSpeed("第二贴图X速度 SecTexXSpeed", Range(-1,1))=0.1
        // _SecTexYSpeed("第二贴图Y速度 SecTexYSpeed", Range(-1,1))=0.1

        // [Space(10)]
        // [Toggle(FRAG_LIGHT)] _FragLight("使用逐像素光照 Frag Light",Float) = 0
        // _LightThreshold("亮暗部阈值 Light Threshold",Range(-1,1)) = 0.65
        // _ShadowIntensity("阴影强度 Shadow Intensity",Range(0,3)) = 1.0
        // _TransitionIntensity("过渡强度 Transition Intensity",Range(0,3)) = 0.2

        [Header(Rim)]
        [Toggle(DRAW_RIM)] _Rim("开启边缘光 Draw Rim",Float) = 0
        _RimColor("边缘光颜色 Rim Color", Color) = (0.5,0.5,0.5,0.5)
        _FPower("幂 FPower Fresnel", Range(0,10)) = 4.0
        _F0("反射系数 F0 Fresnel", Float) = 0.05

        // [Header(LightMap)]
        // _LightmapIntensity("光照贴图强度 Lightmap Intensity",Range(-2,2)) = 0.5
        // _LightmapAdd("光照贴图增益 Lightmap Add",Range(-2,2)) = 0.5
        // [Toggle(USE_LM_SHADOWCOLOR)] _UseLightmapShadowColor("使用光照贴图阴影色 Use Lightmap Shadow Color",Float) = 0
        // _LightmapShadowColor("光照贴图阴影色 Lightmap Shadow Color", Color) = (1, 1, 1, 1)

        // [Header(Leaf Sway)]
        // [Toggle(USE_LEAFSWAY)] _UseLeafSway("开启树叶摇摆 Use Leaf Sway",Float)=0
        // _SwayFrequncy("摇摆频率 Sway Frequncy", Range(0, 10)) = 0.4
        // _SwayStrength("摇摆强度 Sway Strength", Range(0, 10)) = 0.6
        // _SwayPosition("摇摆位置 Sway Position", Range(0, 10)) = 0.5
        // _SwayDirection("摇摆方向(角度) Sway Direction", Range(0, 360)) = 15


        [Toggle(USE_VERTEXCOLOR)] _UseVertexColor("使用顶点色 Use Vertex Color",Float)=0
        [Toggle(ONLY_VERTEXCOLOR)] _OnlyVertexColor("仅显示顶点色 Only Vertex Color",Float)=0


        [Space(10)]
        [Header(T4M_Layer1)]

        _Splat0 ("Layer 1", 2D) = "black" {}
	    _Splat1 ("Layer 2", 2D) = "black" {}
	    _Splat2 ("Layer 3", 2D) = "black" {}
	    _Splat3 ("Layer 4", 2D) = "black" {}
	    _Control ("Control (RGBA)", 2D) = "black" {}

        [Space(10)]
        [Header(T4M_Layer2)]

        _Splat4 ("Layer 5", 2D) = "black" {}
        _Splat5 ("Layer 6", 2D) = "black" {}
        _Splat6 ("Layer 7", 2D) = "black" {}
        _Splat7 ("Layer 8", 2D) = "black" {}
        _Control2 ("Control2 (RGBA)", 2D) = "black" {}
	    _MainTex ("Never Used", 2D) = "white" {}
        _LMvalue ("LM Value" , Range(0.7, 1)) = 0.8

    }
        SubShader
        {
            Tags {"RenderType" = "Opaque" "Queue" = "Geometry+20" }
            UsePass "Universal Render Pipeline/Simple Lit/DepthOnly"
            Pass
            {
                NAME "SPToon"
                Tags {"RenderType" = "Opaque" "LightMode" = "UniversalForward"}

                //Tags {"LightMode" = "ForwardBase"}
                LOD 100
                HLSLPROGRAM
                #pragma multi_compile_instancing
                #pragma multi_compile_fog
                // #pragma multi_compile __ LIGHTMAP_ON
                #pragma multi_compile _ LIGHTMAP_ON

                #pragma multi_compile __ DRAW_RIM
                #pragma multi_compile __ USE_SECTEX
                #pragma multi_compile __ FRAG_LIGHT
                #pragma multi_compile __ USE_VERTEXCOLOR
                #pragma multi_compile __ ONLY_VERTEXCOLOR
                //#pragma multi_compile __ USE_LM_SHADOWCOLOR
                //#pragma multi_compile __ USE_LEAFSWAY
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			    #pragma multi_compile _ _SHADOWS_SOFT


                // #include "UnityCG.cginc"
                // #include "Lighting.cginc"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


// #if USE_LEAFSWAY
//                 #include "CustomFeatures.cginc"
//                 half _SwayFrequncy;
//                 half _SwayStrength;
//                 half _SwayPosition; 
//                 half _SwayDirection;
// #endif
                sampler2D _MainTex;
                float4 _MainTex_ST;
                half _ShadowRange;
                half _ShadowStrength;

                sampler2D _Control;
                sampler2D _Control2;
                sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
                float4 _Splat0_ST,_Splat1_ST,_Splat2_ST,_Splat3_ST;

                sampler2D _Splat4,_Splat5,_Splat6,_Splat7;
                float4 _Splat4_ST,_Splat5_ST,_Splat6_ST,_Splat7_ST;

#if USE_SECTEX
                sampler2D _SecTex;
                float4 _SecTex_ST;
                half _SecTexStrength;
                half _SecTexXSpeed;
                half _SecTexYSpeed;
#endif

                half4 _Color;
                half4 _HighlightColor;
                half4 _EmissionColor;
                half4 _ShadowColor;
                half4 _RimColor;

                half _OutlineThreshold;
                half _LightThreshold;
                half _ShadowIntensity;
                half _TransitionIntensity;

                half4 _LightmapShadowColor;

                //阴影用
                half _ReceiveShadowBias;
                half _ReceiveShadowAtten;

                float _LMvalue;

                // #if LIGHTMAP_ON
                //     half _LightmapIntensity;
                //     half _LightmapAdd;
                // #endif 
                #if DRAW_RIM
                    half _FPower;
                    half _F0;
                #endif 


                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    // #if LIGHTMAP_ON
                    //     float2 uv_LightMap : TEXCOORD1;
                    // #endif
                    float2 lightmapUV : TEXCOORD1;
                    float3 normal:NORMAL;
                    #if USE_VERTEXCOLOR
                    float4 vColor:COLOR;
                    #endif
                    //float4 normalOS:NORMAL;
                    
                    //UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    // #if LIGHTMAP_ON
                    //     float2 lightMap : TEXCOORD1;
                    // #endif 
                    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

                    #if FRAG_LIGHT
                        float4 worldNormal:TEXCOORD2; // xyz:normal w:fresnel
                        float3 worldPos:TEXCOORD3;
                    #else
                        float4 worldNormal : TEXCOORD2; // xyz:normal w:fresnel
                        float3 worldLight : TEXCOORD3;
                    #endif
                    
                    //UNITY_FOG_COORDS(4)
                    float fogCoord  : TEXCOORD4;

                    float2 uv2 : TEXCOORD5;
                    #if USE_VERTEXCOLOR
                    float4 vColor:TEXCOORD6;
                    #endif
                    #ifdef _MAIN_LIGHT_SHADOWS
				        float4 shadowCoord: TEXCOORD7;
                    #endif
                    //float3 normalWS:TEXCOORD6;
                    //UNITY_VERTEX_INPUT_INSTANCE_ID

                    //float2 uv_Control : TEXCOORD8;
                    float2 uv_Splat0 : TEXCOORD8;
                    float2 uv_Splat1 : TEXCOORD9;
                    float2 uv_Splat2 : TEXCOORD10;
                    float2 uv_Splat3 : TEXCOORD11;
                    float2 uv_Splat4 : TEXCOORD12;
                    float2 uv_Splat5 : TEXCOORD13;
                    float2 uv_Splat6 : TEXCOORD14;
                    float2 uv_Splat7 : TEXCOORD15;


                    
                };

                v2f vert(appdata v) {
                    UNITY_SETUP_INSTANCE_ID(v);
                    v2f o;
                    // #if USE_LEAFSWAY
                    //     v.vertex = windEffectDir_Stem(v.vertex, _SwayFrequncy, _SwayStrength, _SwayPosition, _SwayDirection);
                    // #endif

                    //o.vertex = UnityObjectToClipPos(v.vertex);
                    o.vertex = TransformObjectToHClip(v.vertex);

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    o.uv_Splat0 = TRANSFORM_TEX(v.uv, _Splat0);
                    o.uv_Splat1 = TRANSFORM_TEX(v.uv, _Splat1);
                    o.uv_Splat2 = TRANSFORM_TEX(v.uv, _Splat2);
                    o.uv_Splat3 = TRANSFORM_TEX(v.uv, _Splat3);
                    o.uv_Splat4 = TRANSFORM_TEX(v.uv, _Splat4);
                    o.uv_Splat5 = TRANSFORM_TEX(v.uv, _Splat5);
                    o.uv_Splat6 = TRANSFORM_TEX(v.uv, _Splat6);
                    o.uv_Splat7 = TRANSFORM_TEX(v.uv, _Splat7);

                    
                    #if USE_SECTEX
                    o.uv2 = TRANSFORM_TEX(v.uv, _SecTex);
                    #endif

                    // #if LIGHTMAP_ON
                    //     o.lightMap = v.uv_LightMap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    // #endif

                    //UNITY_TRANSFER_FOG(o,o.vertex);
                    o.fogCoord = ComputeFogFactor(o.vertex.z);

                    Light mylight=GetMainLight();

                    #if FRAG_LIGHT
                        //o.worldNormal = float4(mul(v.normal, (half3x3)unity_WorldToObject),1.0);
                        o.worldNormal = float4(TransformObjectToWorldNormal(v.normal),1);
                        o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    #else
                        //o.worldNormal = float4(normalize(mul(v.normal, (half3x3)unity_WorldToObject)),1.0);
                        o.worldNormal = float4(TransformObjectToWorldNormal(v.normal),1);
                        //o.worldLight = WorldSpaceLightDir(v.vertex);
                        o.worldLight =normalize(mylight.direction);
                    #endif
                    
                    #if DRAW_RIM
                        //half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                        half3 viewDir = normalize(_WorldSpaceCameraPos.xyz-TransformObjectToWorld(v.vertex.xyz));
                        half fresnel = saturate(1.0 - dot(v.normal, viewDir));
                        fresnel = pow(fresnel, _FPower);
                        fresnel = _F0 + (1.0 - _F0) * fresnel;
                        o.worldNormal.w = fresnel;
                    #endif

                    #if defined(_MAIN_LIGHT_SHADOWS) 
                                    float3 worldPos = TransformObjectToWorld(v.vertex);
                                    o.shadowCoord = TransformWorldToShadowCoord(worldPos);
                                    o.shadowCoord.z -= _ReceiveShadowBias;
                    #endif

                    OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);

                    #if USE_VERTEXCOLOR
                    //顶点色
                    o.vColor=v.vColor;
                    #endif



                    



                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    

                    #if defined(_MAIN_LIGHT_SHADOWS)
                                    Light mainLight = GetMainLight(i.shadowCoord);
                    #else
                                    Light mainLight = GetMainLight();
                    #endif


                    #if FRAG_LIGHT
                        float3 N = normalize(i.worldNormal.xyz);
                        float3 L = normalize(UnityWorldSpaceLightDir(i.worldPos));
                        float dotResult = saturate(1.0 - dot(N,L));
                    #else
                        float dotResult = saturate(1.0 - dot(i.worldNormal.xyz,i.worldLight));
                    #endif
                    
                    float dotLightStep = step((1.0 - _LightThreshold),dotResult) + dotResult * _TransitionIntensity;
                    dotLightStep = saturate(dotLightStep);



                    half4 splat_control = tex2D (_Control, i.uv).rgba;
		
                    half3 lay1 = tex2D (_Splat0, i.uv_Splat0);
                    half3 lay2 = tex2D (_Splat1, i.uv_Splat1);
                    half3 lay3 = tex2D (_Splat2, i.uv_Splat2);
                    half3 lay4 = tex2D (_Splat3, i.uv_Splat3);

                    //half4 originColor = tex2D(_MainTex, i.uv);
                    half4 originColor = half4((lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a),1);

                    half4 splat_control2 = tex2D (_Control2, i.uv).rgba;

                    half3 lay5 = tex2D (_Splat4, i.uv_Splat4);
                    half3 lay6 = tex2D (_Splat5, i.uv_Splat5);
                    half3 lay7 = tex2D (_Splat6, i.uv_Splat6);
                    half3 lay8 = tex2D (_Splat7, i.uv_Splat7);

                    //half4 originColor = tex2D(_MainTex, i.uv);
                    originColor += half4((lay5 * splat_control2.r + lay6 * splat_control2.g + lay7 * splat_control2.b + lay8 * splat_control2.a),0);

                    
                    
                    //_ShadowColor = lerp(_HighlightColor, _ShadowColor, _LightColor0 * _ShadowIntensity * 0.3);
                    //_HighlightColor = lerp(_HighlightColor, _ShadowColor, -_LightColor0 * _ShadowIntensity * 0.5);

                    #if USE_SECTEX
                    half4 secColor = tex2D(_SecTex, i.uv2+_Time.y*half2(_SecTexXSpeed,_SecTexYSpeed));
                    #endif

                    // #if LIGHTMAP_ON
                    //     float3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightMap.xy));
                    //     originColor.rgb *= lm * _LightmapIntensity + _LightmapAdd;
                    //     #if USE_LM_SHADOWCOLOR
                    //         half3 lmShadow= 1-saturate(lm);
                    //         originColor.rgb = originColor.rgb*(1-lmShadow*_LightmapShadowColor.a) + _LightmapShadowColor * lmShadow*_LightmapShadowColor.a;
                    //     #endif
                    // #endif

                    

                    //half4 outputColor = originColor * (_HighlightColor*(1.0 - dotLightStep) + _ShadowColor * dotLightStep)*0.7;
                    half4 outputColor = originColor * (_HighlightColor*(1.0 - dotLightStep) + _ShadowColor * dotLightStep);
                    //half4 result = outputColor * _Color * (_EmissionColor + 2.0);
                    half4 result = outputColor * _Color * (_EmissionColor + 1.0);

                    #if DRAW_RIM
                        result = saturate(2.0f * i.worldNormal.w * _RimColor) + result;
                    #endif
                    //return _LightColor0;


                    Light mylight=GetMainLight();
                    //新版光照贴图
                    #if LIGHTMAP_ON
                    result *= half4(SAMPLE_GI(i.lightmapUV, i.vertexSH, normalize(i.worldNormal.xyz)),1);

                    #else

                    result = result * half4(mylight.color,1);

                    #endif


                    // 主光阴影
				    half att = 1;
                    #if defined(_MAIN_LIGHT_SHADOWS)
                                    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
                                    att = SampleShadowmap(i.shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, 1, false);
                    #endif
                    half shadowAtten = saturate(att + _ReceiveShadowAtten);
                    //return half4(shadowAtten,shadowAtten,shadowAtten,1);


                    #if LIGHTMAP_ON
                    //单独采lightmap做阴影融合
                        half3 LMcolor = saturate(SAMPLE_GI(i.lightmapUV, i.vertexSH, normalize(i.worldNormal.xyz)));
                        half LMVal = saturate(LMcolor.r * 0.333 +LMcolor.g * 0.33 + LMcolor.b * 0.33) * _LMvalue;
                        LMVal = smoothstep(0.6 , 1 ,LMVal) * _ShadowStrength;
                        shadowAtten = shadowAtten*LMVal+(1-LMVal);
                        shadowAtten = pow(shadowAtten , 0.25);
                        result *= shadowAtten;
                    #else
                        shadowAtten=shadowAtten*_ShadowStrength+(1-_ShadowStrength);
                        shadowAtten=pow(shadowAtten,0.25);
                    //    result*=shadowAtten;
                    #endif


                    #if USE_SECTEX
                    result = result + secColor * 0.25;
                    #endif
                    
                    #if USE_VERTEXCOLOR
                    //顶点色计算
                    result*=i.vColor;
                    #if ONLY_VERTEXCOLOR
                    return i.vColor;
                    #endif
                    #endif
                    
                    
                    //雾效计算
                    //UNITY_APPLY_FOG(i.fogCoord, result);
                    result = half4(MixFog(result, i.fogCoord),1);
                    
                    
                    return result;

                }
                ENDHLSL
            }
            UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        }
}
