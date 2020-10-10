Shader "SoFunny/SPToon/SPToonNew_v2_RAS"
{
    Properties
    {
        [Header(Main)]
        _MainTex("主贴图 Main Tex", 2D) = "white" {}
        _RampTex("色阶图 Ramp Tex", 2D) = "white" {}

        [Header(Normal)]
        _NormalTex("Normal",2D)="bump"{}
        _NormalScale("NormalScale",Range(0,1))=1

        [Header(Ramp AO Specular)]
        _RASTex("RAS", 2D) = "white" {}
        

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

        [Header(Speculaer)]
        [Toggle(USE_SPEC)] _UseSpec("使用高光 Use Spec",Float) = 0
        _SpecColor("高光颜色 Spec Color",Color) = (0.2,0.2,0.2,1)
        _SpecPow("高光次幂 Spec Pow",Range(1,200)) = 20

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
        [Toggle(NO_LIGHTMAP)] _NoLightmap("不显示光照贴图 No Lightmap",Float) = 0

    }
        SubShader
        {
            Tags {"RenderType" = "Opaque" "Queue" = "Geometry+20" }
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
                #pragma multi_compile __ USE_SPEC
                #pragma multi_compile __ USE_VERTEXCOLOR
                #pragma multi_compile __ ONLY_VERTEXCOLOR
                #pragma multi_compile __ NO_LIGHTMAP
                //#pragma multi_compile __ USE_LM_SHADOWCOLOR
                // #pragma multi_compile __ USE_LEAFSWAY
                #pragma vertex vert
                #pragma fragment frag
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

                sampler2D _RampTex;

                sampler2D _NormalTex;
                float4 _NormalTex_ST;
                float _NormalScale;

                sampler2D _RASTex;

#if USE_SECTEX
                sampler2D _SecTex;
                float4 _SecTex_ST;
                half _SecTexStrength;
                half _SecTexXSpeed;
                half _SecTexYSpeed;
#endif

#if USE_SPEC
                half4 _SpecColor;
                half _SpecPow;
#endif

                half4 _RimColor;

                half _OutlineThreshold;
                half _LightThreshold;
                half _ShadowIntensity;
                half _TransitionIntensity;

                half4 _LightmapShadowColor;
                // #if LIGHTMAP_ON
                //     half _LightmapIntensity;
                //     half _LightmapAdd;
                // #endif 
                #if DRAW_RIM
                    half _FPower;
                    half _F0;
                #endif 


                struct appdata {
                    float3 vertex : POSITION;
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
                    float4 tangentOS:TANGENT;
                    
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

                    float4 tangentWS:TANGENT;
                    float4 BtangentWS:TEXCOORD7;
                    float4 normalWS:NORMAL;
                    float2 uvNormal : TEXCOORD8;
                    half3 viewDir : TEXCOORD9;
                    //float3 normalWS:TEXCOORD6;
                    //UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                v2f vert(appdata v) {
                    //UNITY_SETUP_INSTANCE_ID(v);
                    v2f o;
                    // #if USE_LEAFSWAY
                    //     v.vertex = windEffectDir_Stem(v.vertex, _SwayFrequncy, _SwayStrength, _SwayPosition, _SwayDirection);
                    // #endif

                    //o.vertex = UnityObjectToClipPos(v.vertex);
                    o.vertex = TransformObjectToHClip(v.vertex);

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uvNormal = TRANSFORM_TEX(v.uv, _NormalTex);
                    
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

                    half3 viewDir = normalize(TransformWorldToObject(_WorldSpaceCameraPos) - v.vertex);
                    o.viewDir=viewDir;
                    
                    #if DRAW_RIM
                        //half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                        half fresnel = saturate(1.0 - dot(v.normal, viewDir));
                        fresnel = pow(fresnel, _FPower);
                        fresnel = _F0 + (1.0 - _F0) * fresnel;
                        o.worldNormal.w = fresnel;
                    #endif

                    //法线贴图
                    o.normalWS.xyz=TransformObjectToWorldNormal(v.normal);
                    o.tangentWS.xyz=normalize(mul((float3x3)unity_ObjectToWorld, normalize(v.tangentOS.xyz)));
                    o.BtangentWS.xyz=cross(o.normalWS.xyz,o.tangentWS.xyz)*v.tangentOS.w;
                    float3 positionWS=TransformObjectToWorld(v.vertex);
                    o.tangentWS.w=positionWS.x;
                    o.BtangentWS.w=positionWS.y;
                    o.normalWS.w=positionWS.z;
                    


                    OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);

                    #if USE_VERTEXCOLOR
                    //顶点色
                    o.vColor=v.vColor;
                    #endif
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {

                    Light mylight=GetMainLight();
                    
                    //法线贴图
                    half4 NormalTex=tex2D(_NormalTex, i.uvNormal);
                    float3 bump=UnpackNormal(NormalTex);
                    bump.xy*=_NormalScale;
                    bump.z=sqrt(1.0-saturate(dot(bump.xy, bump.xy)));
                    bump = normalize(
                        half3(
                            dot(half3(i.tangentWS.x,i.BtangentWS.x,i.normalWS.x),bump),
                            dot(half3(i.tangentWS.y,i.BtangentWS.y,i.normalWS.y),bump),
                            dot(half3(i.tangentWS.z,i.BtangentWS.z,i.normalWS.z),bump)
                            )
                    );

                    half diffTotal = max(0, dot(bump, mylight.direction))*0.5+0.5;
                    half diffDiffuse = max(0, dot(i.normalWS, mylight.direction))*0.5+0.5;
                    half diff=diffTotal+saturate(1-diffDiffuse);

                    //return i.worldNormal;

                    //RAS贴图
                    half4 RASTex=tex2D(_RASTex, i.uv);
                    
                    
                    #if FRAG_LIGHT
                        float3 N = normalize(i.worldNormal.xyz);
                        float3 L = normalize(UnityWorldSpaceLightDir(i.worldPos));
                        float dotResult = saturate(1.0 - dot(N,L));
                    #else
                        float dotResult = saturate(1.0 - dot(i.worldNormal.xyz,i.worldLight));
                    #endif
                    
                    float dotLightStep = step((1.0 - _LightThreshold),dotResult) + dotResult * _TransitionIntensity;
                    dotLightStep = saturate(dotLightStep);

                    half4 originColor = tex2D(_MainTex, i.uv);

                    //AO贴图计算

                    originColor.rgb*=RASTex.g;

                    //法线贴图计算

                    originColor.rgb*=diff;

                    //高光计算
                    #if USE_SPEC
                        float3 WorldNormal = normalize(i.worldNormal.xyz);
                        float3 LightDir = mylight.direction;
                        float3 ViewDir = i.viewDir;
                        half3 HalfDir = normalize(LightDir+ViewDir);
                        
                    #endif
                    
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

                    

                    //half4 outputColor = originColor * (_HighlightColor*(1.0 - dotLightStep) + _ShadowColor * dotLightStep);
                    half4 outputColor = originColor * tex2D(_RampTex,half2(dot(i.worldNormal.xyz,i.worldLight)*RASTex.r,0.5));
                    
                    half4 result = outputColor;

                    #if DRAW_RIM
                        result = saturate(2.0f * i.worldNormal.w * _RimColor) + result;
                    #endif
                    //return _LightColor0;

                    //新版光照贴图
                    #if LIGHTMAP_ON
                        #if NO_LIGHTMAP

                        #else
                            result *= half4(SAMPLE_GI(i.lightmapUV, i.vertexSH, normalize(i.worldNormal.xyz)),1);  
                        #endif

                    #else

                    
                    result = result * half4(mylight.color,1);

                    #endif
                    
                    #if USE_SPEC
                        //half Spec = smoothstep(_SpecThreshold-_SpecTransitionRange,_SpecThreshold+_SpecTransitionRange,dot(WorldNormal,HalfDir))*_SpecStrength;
                        half3 Spec = _SpecColor.rgb*pow(max(0,dot(WorldNormal,HalfDir)),_SpecPow);
                        result.rgb +=Spec*RASTex.b;
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
        }
}
