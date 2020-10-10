Shader "SoFunny/SPToon/SPToonCharacter"
{
    Properties
    {
        [Header(MainToon)]
        _MainTex("主贴图 Main Tex", 2D) = "white" {}
        [Toggle(USE_SECONDTEX)] _UseSecondTex("使用副贴图 Use Second Tex",Float) = 0
        _SecTex("副贴图（控制阴影高光） Sec Tex", 2D) = "white" {}
        [Toggle(USE_THIRDTEX)] _UseThirdTex("使用第三贴图 Use Third Tex",Float) = 0
        _ThirTex("第三贴图（纹身特效） Thir Tex", 2D) = "black" {}
        _ThirTexColor("第三贴图颜色 ThirTexColor", Color) = (1, 1, 1, 1)
        [Toggle] _ThirdTexAddMode("第三贴图Add模式 Third Tex Add Mode",Float) = 0

        [Header(Thir Tex Effect)]
        [Toggle(USE_THIRDTEXEFFECT)] _UseThirdTexEffect("使用第三贴图特效 Use Third Tex Effect",Float) = 0
        _ThirTexMask("第三贴图遮罩 Thir Tex Mask", 2D) = "white" {}
        _ShiningBrightness("闪烁亮度 Shining Brightness",Range(0,2)) = 1
        _ShiningStrength("闪烁强度 Shining Strength",Range(0,2)) = 1
        _ShiningSpeed("闪烁速度 Shining Speed",Range(0,5)) = 2
        _FlowSpeedX("流动速度X Flow Speed X",Range(-5,5)) = 1
        _FlowSpeedY("流动速度Y Flow Speed Y",Range(-5,5)) = 1

        [Header(Specular)]
        [Toggle(USE_TOONSPEC)] _UseToonSpec("使用卡通高光 Use Toon Spec",Float) = 0
        _ToonSpecThreshold("卡通高光阈值 Toon Spec Threshold",Range(0,1)) = 0.8
        _ToonSpecStrength("卡通高光强度 Toon Spec Strength",Range(0,1)) = 0.2
        _ToonSpecTransitionRange("卡通高光过渡范围 Toon Spec Transition Range",Range(0,0.02)) = 0.05
        [Header(HairSpecular)]
        [Toggle(USE_SPECFADE)] _UseSpecFade("头发高光强弱从贴图获取 Use Spec Fade",Float) = 0
        _SpecularRange("头发高光范围 Specular Range",Range(0,1)) = 0.5
        _SpecularColor("头发高光颜色 Specular Color",Color) = (1,1,1,1)
        _SpecularTransitionRange("头发高光过渡范围 SpecularTransitionRange",Range(0,1)) = 0

        [Header(Fourth Tex)]
        [Toggle(USE_FOURTHTEX)] _UseFourthTex("使用第四贴图 Use Fourth Tex",Float) = 0
        _FourthTex("第四贴图（换色遮罩） Fourth Tex", 2D) = "white" {}
        _FourthTexColor("第四贴图染色 Fourth Tex Color", Color) = (1, 1, 1, 1)
        _FourthTexBrightness("亮度调整 Fourth Tex Brightness",Range(-1,1)) = 0

        [Header(Fifth Tex)]
        //溶解特效用
        [Toggle(USE_FIFTHTEX)] _UseFifthTex("使用第五贴图 Use Fifth Tex",Float) = 0
        _FifthTex("第五贴图（溶解图） Fifth Tex", 2D) = "white" {}
        _DissolveValue("溶解值 Dissolve Value",Range(0,1.01)) = 0

        [Header(Sixth Tex)]
        //镭射特效用
        [Toggle(USE_SIXTHTEX)] _UseSixthTex("使用第六贴图 Use Sixth Tex",Float) = 0
        _LaserColor("镭射颜色 Laser Color",Color) = (1,1,1,1)
		_RampTex("渐变纹理 Ramp Texture", 2D) = "white" {}
        _MaskTex("遮罩纹理 Mask Texture", 2D) = "white" {}
		_RampSaturation("渐变饱和度 Ramp Saturation",Range(0,1)) = 1.0
		_RampBrightness("渐变亮度 Ramp Brightness",Range(0,2)) = 1.0
        _RampStrength("渐变强度 Ramp Strength",Range(0,1)) = 0.0
        _RampGloss("渐变光泽度 Ramp Gloss",Range(0,1)) = 1.0
        _RampGlossPower("渐变光泽度幂 Ramp Gloss Power",Range(1,4)) = 4.0
		_RampScale("渐变尺度 Ramp Scale",Range(0,8)) = 4.0

        [Space(10)]
        [Header(Main)]
        _Color("主颜色 Color", Color) = (1, 1, 1, 1)
        _EmissionColor("自发光色 Emission Color", Color) = (0, 0, 0, 1)
        _HighlightColor("亮部色 Highlight Color",Color) = (1.0,1.0,1.0,1)
        _ShadowColor("暗部色 Shadow Color",Color) = (0.6,0.6,0.6,1)

        _LightThreshold("亮暗部阈值 Light Threshold",Range(-1,1)) = 0.65
        _ShadowIntensity("阴影强度 Shadow Intensity",Range(0,3)) = 1.0
        _TransitionRange("过渡范围 Transition Range",Range(0,1)) = 0
        _TransitionIntensity("过渡强度 Transition Intensity",Range(0,3)) = 0.2

        // [Header(Ambient)]
        // [Toggle(USE_AMBIENTLIGHT)] _UseAmbientLight("计算环境光 Use Ambient Light",Float) = 1
        // _MainAmbientSensitivity("主环境光敏感度 Main Ambient Sensitivity", Range(0,1)) = 0.4

        [Header(Rim)]
        [Toggle(DRAW_RIM)] _Rim("开启边缘光 Draw Rim",Float) = 0
        _RimColor("边缘光颜色 Rim Color", Color) = (0.5,0.5,0.5,0.5)
        _FPower("幂 FPower Fresnel", Range(0,15)) = 4.0
        _F0("反射系数 F0 Fresnel", Float) = 0.05
        _RimThreshold("边缘光阈值 RimThreshold",Range(-1,1)) = -1.0

        [Header(SPRim)]
        [Toggle(DRAW_SPRIM)] _SPRim("开启特殊状态边缘光 Draw SPRim",Float) = 0
        _SPRimColor("边缘光颜色 SPRim Color", Color) = (1.0,1.0,0.0,1.0)
        _SPFPower("幂 SPFPower Fresnel", Range(0,15)) = 2.0
        _SPF0("反射系数 SPF0 Fresnel", Float) = 0.0
        _SPRimThreshold("边缘光阈值 SPRimThreshold",Range(-1,1)) = -1.0
        _SPRimAddStrength("叠加程度 SPRimAddStrength", Range(0,1)) = 0.0

        [Header(SPRimShining)]
        [Toggle(SPRIM_SHINING)] _SPRimShining("开启闪烁 SPRimShining",Float) = 0
        _SPRimShiningBaseBrightness("基础亮度 SPRimShiningBaseBrightness", Range(-1,1)) = 0.5
        _SPRimShiningStrength("闪烁强度 SPRimShiningStrength", Range(0,1)) = 0.5
        _SPRimShiningSpeed("闪烁速度 SPRimShiningSpeed", Range(0.5,5)) = 1.0


    }
        SubShader
        {
            Tags {"RenderType" = "Opaque" "Queue" = "Geometry" }
            Pass
            {
                //NAME "SPToon"
                //Tags {"LightMode" = "ForwardBase"}
                LOD 100

                CGPROGRAM
                #pragma multi_compile __ DRAW_RIM
                #pragma multi_compile __ DRAW_SPRIM
                #pragma multi_compile __ USE_SECONDTEX
                #pragma multi_compile __ USE_THIRDTEX
                #pragma multi_compile __ USE_THIRDTEXEFFECT
                #pragma multi_compile __ USE_TOONSPEC
                #pragma multi_compile __ USE_SPECFADE
                #pragma multi_compile __ USE_FOURTHTEX
                #pragma multi_compile __ SPRIM_SHINING
                #pragma multi_compile __ USE_FIFTHTEX
                #pragma multi_compile __ USE_SIXTHTEX
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                sampler2D _MainTex;

                CBUFFER_START(UnityPerMaterial)

                half4 _MainTex_ST;
                // #if USE_SECONDTEX
                //     sampler2D _SecTex;
                // #endif 
                // #if USE_THIRDTEX
                //         sampler2D _ThirTex;
                //         fixed4 _ThirTexColor;
                //     #if USE_THIRDTEXEFFECT
                //         sampler2D _ThirTexMask;
                //     #endif 
                //         fixed _ThirdTexAddMode;
                //         half _FlowSpeedX;
                //         half _FlowSpeedY;
                //         half _ShiningBrightness;
                //         half _ShiningStrength;
                //         half _ShiningSpeed;
                // #endif

                // #if USE_FOURTHTEX
                //     sampler2D _FourthTex;
                //     fixed4 _FourthTexColor;
                //     half _FourthTexBrightness;
                // #endif

                // #if USE_FIFTHTEX
                //     sampler2D _FifthTex;
                //     half _DissolveValue;
                // #endif

                // #if USE_SIXTHTEX
                //     fixed4 _RampColor;
                //     sampler2D _RampTex;
                //     sampler2D _MaskTex;
                //     float4 _MaskTex_ST;
                //     half _RampSaturation;
                //     half _RampStrength;
                //     half _RampBrightness;
                //     half _RampGloss;
                //     half _RampGlossPower;
                //     half _RampScale;
                // #endif

                fixed4 _Color;
                fixed4 _SpecularColor;
                fixed4 _HighlightColor;
                fixed4 _ShadowColor;
                fixed4 _EmissionColor;
                fixed4 _RimColor;
                fixed4 _SPRimColor;

                half _LightThreshold;
                fixed _SPRimShiningBaseBrightness;
                fixed _SPRimAddStrength;
                fixed _SPRimShiningStrength;
                half _SPRimShiningSpeed;
                half _ShadowIntensity;
                half _TransitionRange;
                half _TransitionIntensity;
                // half _MainAmbientSensitivity;
                // #if USE_SECONDTEX
                //     half _SpecularRange;
                //             half _SpecularTransitionRange;
                // #endif

                // #if USE_TOONSPEC
                //     half _ToonSpecThreshold;
                //     half _ToonSpecStrength;
                //     half _ToonSpecTransitionRange;
                // #endif

                #if DRAW_RIM
                    half _RimThreshold;
                    half _FPower;
                    half _F0;
                #endif 
                // #if DRAW_SPRIM
                //     half _SPRimThreshold;
                //     half _SPFPower;
                //     half _SPF0;
                // #endif 
                CBUFFER_END
                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    half3 normal : NORMAL;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 worldNormal : TEXCOORD1;
                    float4 worldPos : TEXCOORD2;

                };

                v2f vert(appdata v) {

                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    // 用逆矩阵转换法线至世界空间
                    o.worldNormal = float4(mul(v.normal, (half3x3)unity_WorldToObject),1.0);
                    o.worldPos = float4(mul(unity_ObjectToWorld, v.vertex).xyz, 1.0);

                    half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));

                    #if DRAW_RIM
                        float fresnel = saturate(1.0 - dot(v.normal, viewDir));
                        fresnel = pow(fresnel, _FPower);
                        fresnel = _F0 + (1.0 - _F0) * fresnel;
                        o.worldNormal.w = fresnel;
                    #endif

                    #if DRAW_SPRIM
                        float spFresnel = saturate(1.0 - dot(v.normal, viewDir));
                        spFresnel = pow(spFresnel, _SPFPower);
                        spFresnel = _SPF0 + (1.0 - _SPF0) * spFresnel;
                        o.worldPos.w = spFresnel;
                    #endif

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // #if USE_FIFTHTEX
                    //     fixed4 dissolveColor = tex2D(_FifthTex, i.uv);
                    //     clip(dissolveColor.r-_DissolveValue);
                    // #endif

                    fixed4 originColor = tex2D(_MainTex, i.uv);

                    // #if USE_FOURTHTEX
                    //     fixed4 fourthTex = tex2D(_FourthTex, i.uv);
                    //     //originColor*=_FourthTexColor;
                    //     fixed4 tempColor = originColor*fixed4(0.299,0.518,0.184,1);
                    //     fixed grayScale = tempColor.r+tempColor.g+tempColor.b;
                    //     originColor=originColor*(1-fourthTex.r)+_FourthTexColor*fourthTex.r*(grayScale+_FourthTexBrightness);
                    // #endif

                    // #if USE_SECONDTEX
                    //     fixed4 secTex = tex2D(_SecTex,i.uv);
                    // #endif

                    // #if USE_THIRDTEX
                    //     #if USE_THIRDTEXEFFECT				    
                    //         fixed4 thirdTexMask = tex2D(_ThirTexMask,i.uv);
                    //         fixed maskValue = (thirdTexMask.r + thirdTexMask.g + thirdTexMask.b) / 3;
                    //         fixed4 thirdTex = tex2D(_ThirTex,i.uv + half2(_Time.y*_FlowSpeedX,_Time.y*_FlowSpeedY))*_ThirTexColor;
                    //         originColor = saturate(thirdTex.a + _ThirdTexAddMode)*maskValue*thirdTex*(_ShiningBrightness + abs(cos(_Time.y*_ShiningSpeed))*_ShiningStrength) + (1 - thirdTex.a*maskValue*(1 - _ThirdTexAddMode))*originColor;
                    //     #else
                    //         fixed4 thirdTex = tex2D(_ThirTex,i.uv)*_ThirTexColor;
                    //         originColor = saturate(thirdTex.a + _ThirdTexAddMode)*thirdTex + (1 - thirdTex.a*(1 - _ThirdTexAddMode))*originColor;
                    //     #endif
                    // #endif

                    //亮部强度根据阴影强度来计算
                    fixed3 N = normalize(i.worldNormal.xyz);
                    fixed3 L = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
                    fixed3 V = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
                    
                    // #if USE_TOONSPEC
                    //     fixed3 HD = normalize(L+V);
                    // #endif

                    half dotResult = (1 - dot(N,L));
                    half dotLightStep = smoothstep((1 - _LightThreshold) - _TransitionRange,(1 - _LightThreshold) + _TransitionRange,dotResult) + dotResult * _TransitionIntensity;

                    // #if USE_SECONDTEX
                    //     dotLightStep = smoothstep(secTex.r - _TransitionRange,secTex.r + _TransitionRange,dotResult) + dotResult * _TransitionIntensity;
                    // #endif


                    //前有叠加，防止值超出1造成阴影颜色过深
                    dotLightStep = saturate(dotLightStep);
                    _ShadowColor = lerp(_HighlightColor, _ShadowColor, _LightColor0*_ShadowIntensity*0.3);
                    _HighlightColor = lerp(_HighlightColor, _ShadowColor, -_LightColor0 * _ShadowIntensity*0.5);


                    fixed4 shadowColor = originColor * _ShadowColor*dotLightStep;
                    fixed4 highlightColor = originColor * _HighlightColor*(1 - dotLightStep);

                    // #if USE_SECONDTEX
                    //     highlightColor = highlightColor * saturate(smoothstep(0.25 - _TransitionRange,0.25 + _TransitionRange,secTex.g) + 0.95);
                    //     shadowColor = shadowColor * saturate(smoothstep(0.25 - _TransitionRange,0.25 + _TransitionRange,secTex.g) + 0.9);
                    // #endif
                    // #if USE_AMBIENTLIGHT
                    //     fixed4 ambientEquator = unity_AmbientEquator * 0.2 + 0.8;
                    //     fixed4 ambientGround = unity_AmbientGround * 0.2 + 0.8;
                    //     fixed4 outputColor = (highlightColor*ambientEquator + shadowColor * ambientGround)*0.7;
                    // #else
                        fixed4 outputColor = (highlightColor + shadowColor) * 0.7;
                    // #endif
                    fixed4 result = outputColor * _Color*(_EmissionColor + 2);
                    #if DRAW_RIM
                        result += saturate((1-step(0.25,dot(N,V)))*(1 - saturate(dotLightStep + _RimThreshold)))*0.25;
                        //result = saturate(2.0f * i.worldNormal.w * _RimColor)*(1 - saturate(dotLightStep + _RimThreshold)) + result;
                    #endif

                    // #if DRAW_SPRIM
                    //     fixed4 SprimCol = saturate(2.0f * i.worldPos.w * _SPRimColor)*(1 - saturate(dotLightStep + _SPRimThreshold));
                    //     #if SPRIM_SHINING
                    //         SprimCol = SprimCol * (abs(sin(_Time.y * _SPRimShiningSpeed) * _SPRimShiningStrength)+ _SPRimShiningBaseBrightness);
                    //     #endif
                    //     half SprimGray = (SprimCol.r+SprimCol.g+SprimCol.b) * 0.3333;
                    //     result = SprimCol * 1 + result * saturate(1-SprimGray*(1-_SPRimAddStrength));
                    // #endif

                    //头发高光计算
                    // #if USE_SECONDTEX
                    //     half NdotV = pow(dot(N,V),6) * 2;
                    //     fixed3 spec = NdotV * secTex.b;
                    //     fixed4 finalSpecular = fixed4(0,0,0,1);
                    //     #if USE_SPECFADE
                    //         half hairSpecular = lerp(0,1,smoothstep(0 - _SpecularTransitionRange,0 + _SpecularTransitionRange,spec + _SpecularRange - 1))*step(0.0001,_SpecularRange);
                    //         finalSpecular = _SpecularColor * hairSpecular*secTex.b;
                    //     #else
                    //         half hairSpecular = lerp(0,1,smoothstep(0 - _SpecularTransitionRange,0 + _SpecularTransitionRange,spec + _SpecularRange - 1))*step(0.0001,_SpecularRange);
                    //         finalSpecular = _SpecularColor * hairSpecular;
                    //     #endif
                    // #endif

                    //环境光拟合
                    // #if USE_AMBIENTLIGHT
                    // result = result * (UNITY_LIGHTMODEL_AMBIENT*_MainAmbientSensitivity + (1 - _MainAmbientSensitivity));
                    // #endif

                    //卡通高光
                    // #if USE_TOONSPEC
                    //     half toonSpec = smoothstep(_ToonSpecThreshold-_ToonSpecTransitionRange,_ToonSpecThreshold+_ToonSpecTransitionRange,dot(N,HD))*_ToonSpecStrength;
                    //     result +=toonSpec;
                    // #endif

                    result = result * (_LightColor0*0.2 + 0.8);

                    // #if USE_SECONDTEX
                    //     result = result + saturate(finalSpecular*saturate(smoothstep(secTex.r * 0.8 - _SpecularTransitionRange, secTex.r * 0.8 + _SpecularTransitionRange, dot(N, L))));
                    // #endif

                    // #if USE_SIXTHTEX
                    //     fixed rampNdotV = dot(N,V);
                    //     fixed rampAlpha = _RampBrightness * (0.5 - rampNdotV * 0.5);
                    //     rampAlpha = _RampBrightness * (0.5 + rampNdotV * 0.5);
                    //     fixed diff = 0.5 + rampNdotV * 0.5 * _RampScale;
                    //     fixed3 diffuse_Ramp = _LightColor0 * tex2D(_RampTex,fixed2(diff,diff)).rgb;
                    //     fixed3 diffuse = _LightColor0 * diff;
                    //     fixed3 finalColor = lerp(diffuse, diffuse_Ramp, _RampSaturation);

                    //     fixed3 maskTex = tex2D(_MaskTex,i.uv);
                        
                    //     //高光部分
                    //     fixed3 reflectDir = normalize(reflect(-L, N));
                    //     //fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                    //     fixed3 specular = pow(saturate(dot(reflectDir,V))*_RampGloss, _RampGlossPower);

                    //     half rampPart = (saturate(rampNdotV-_RampStrength))*maskTex.r;
                    //     result = saturate(rampPart+(1-maskTex.r)) * result + (1-rampPart) * fixed4(_Color.rgb * finalColor*rampAlpha+pow(specular,2), 1)*maskTex.r;
                    // #endif

                    return result;
                }
                ENDCG
            }
            UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        }
}
