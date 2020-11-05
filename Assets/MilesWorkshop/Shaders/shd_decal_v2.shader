Shader "MileShader/shd_decal_v2" {
    Properties {
        [Header(Surface Settings)]
        [Toggle(OrthoProjection)]_OrthoProjection ("OrthoProjection", float) = 0
        [HDR]_Color ("Color", Color) = (1, 1, 1, 1)
        _AlphaClip("Alpha Clip", Range(0, 1)) = 0.5
        [NoScaleOffset]_Albedo ("Albedo (A)", 2D) = "white" {}
        [DistanceDrawer]_DistanceFade ("Distance Fade", Vector) = (2500, 0.001, 0, 0)

        [Header(Stencil Settings)]
        [IntRange]_StencilRef ("Stencil Ref", Range(0, 255)) = 0
        [IntRange]_ReadMask ("Read Mask", Range(0, 255)) = 255
        [IntRange]_WriteMask ("Write Mask", Range(0, 255)) = 255
        
        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilCompare ("Stencil Comparison", int) = 6

        [Header(Other Settings)]
        [Toggle(ApplyFog)]_ApplyFog("Enable Fog", float) = 0.0
        [Toggle] _NAGTIVEZ("Nagtive Z", float) = 0.0

    }
    SubShader {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }
            Stencil {
                Ref [_StencilRef]
                ReadMask [_ReadMask]
                WriteMask [_WriteMask]
                Comp [_StencilCompare]
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front
            ZTest Always
            ZWrite Off

            HLSLPROGRAM
            
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature_local OrthoProjection

            #pragma shader_feature_local ApplyFog
            
            #pragma multi_compile_fog

            #pragma multi_compile_instancing

            #pragma shader_feature _NAGTIVEZ_ON

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half _AlphaClip;
                float2 _DistanceFade;
            CBUFFER_END

            #if defined(SHADER_API_GLES)
                TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
            #else
                TEXTURE2D_X_FLOAT(_CameraDepthTexture); //SAMPLER(sampler_PointClamp);
            #endif

            float4 _CameraDepthTexture_TexelSize;

            TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo);

            struct a2v {
                float4 vertex :POSITION;
                float3 normal :NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos :SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO

                float4 rayBlend :TEXCOORD0;
                float4 screenUV :TEXCOORD1;

                #ifdef ApplyFog
                    float fogCoord :TEXCOORD2;
                #endif
                half fade :TEXCOORD3;
            };

            v2f vert(a2v v) {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                /*
                o.rayBlend.xyz = mul(UNITY_MATRIX_MV, float4(v.vertex.xyz, 1)).xyz * float3(-1, -1, 1);
                */
                float3 pos_World = TransformObjectToWorld(v.vertex.xyz);
                o.rayBlend.xyz = TransformWorldToView(pos_World) * float3(-1, -1, 1);
                o.screenUV = ComputeScreenPos(o.pos);

                #ifdef ApplyFog
                    o.fogCoord = ComputeFogFactor(o.pos.z);
                #endif
                float3 worldDistancePos = UNITY_MATRIX_M._m03_m13_m23; // Get Position From Matrix
                float3 diff = (_WorldSpaceCameraPos - worldDistancePos);
                float distance = dot(diff, diff);
                o.fade = saturate((_DistanceFade.x - distance) * _DistanceFade.y);

                return o;
            }

            half Alpha(half albedoAlpha, half4 color, half cutoff) {
                half alpha = albedoAlpha * color.a;
                clip(alpha - cutoff);
                return alpha;
            }

            half4 frag(v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                
                float3 ray = i.rayBlend.xyz * (_ProjectionParams.z / i.rayBlend.z);
                #ifdef OrthoProjection
                    float2 rayOrtho = float2(unity_OrthoParams.xy * (i.screenUV.xy - 0.5) * 2);
                    rayOrtho *= float2(-1, -1);
                #endif

                

                float2 uv = i.screenUV.xy / i.screenUV.w;
                #ifdef UNITY_SINGLE_PASS_STEREO
                    uv.x = uv.x * 0.5f + (float)unity_StereoEyeIndex * 0.5f;
                #endif
                // reconstruct world position by depth
                #if defined(SHADER_API_GLES)
                    float depth = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, uv, 0);
                #else
                    float depth = LOAD_TEXTURE2D_X(_CameraDepthTexture, _CameraDepthTexture_TexelSize.zw * uv).x;
                #endif       

                #ifdef OrthoProjection
                    float depthOrtho = depth;
                    depth = Linear01Depth(depth, _ZBufferParams);
                    #ifdef UNITY_REVERSED_Z
                    // openGL situation
                        #if UNITY_REVERSED_Z == 1
                            depthOrtho = 1.0f - depthOrtho;
                        #endif
                    #endif
                    float4 vertexPos = float4(ray * depth, 1);
                    float4 vertexPosOrtho = float4(rayOrtho, -depthOrtho * _ProjectionParams.z, 1);
                    vertexPos = lerp(vertexPos, vertexPosOrtho, unity_OrthoParams.w);
                #else
                    depth = Linear01Depth(depth, _ZBufferParams);
                    float4 vertexPos = float4(ray * depth, 1);
                #endif

                // transform view to world
                #ifdef _NAGTIVEZ_ON
                    float4x4 nagtiveZ = float4x4(float4(1, 0, 0, 0), float4(0, 1, 0, 0), float4(0, 0, -1, 0), float4(0, 0, 0, 1));
                    unity_CameraToWorld = mul(unity_CameraToWorld, nagtiveZ);
                    float3 worldPos = mul(unity_CameraToWorld, vertexPos).xyz;
                #else
                    float3 worldPos = mul(unity_CameraToWorld, vertexPos).xyz;
                #endif
                
                //return half4(0, 1, 0, 1);
                //return half4(unity_CameraToWorld[0][0], unity_CameraToWorld[0][1], unity_CameraToWorld[0][2], 1);

                #ifdef OrthoProjection
                    worldPos -= _WorldSpaceCameraPos * 2 * unity_OrthoParams.w;
                    worldPos.xzy *= 1 - 2 * unity_OrthoParams.w;
                #endif

                float3 objectPos = mul(GetWorldToObjectMatrix(), float4(worldPos, 1)).xyz;
                // clip decal to volume
                clip(float3(0.5, 0.5, 0.5) - abs(objectPos.xyz));
                float2 textureUV = objectPos.xz + float2(0.5, 0.5);
                half4 col = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, textureUV) * _Color;
                // distance fade

               

                col.a = Alpha(col.a, _Color, _AlphaClip);

                #ifdef OrthoProjection
                    col.a *= ((unity_OrthoParams.w == 1.0h) ? 1.0h : i.fade);
                #else
                    col.a *= i.fade;
                #endif
                

                #ifdef ApplyFog
                    col.rgb = MixFog(col.rgb, i.fogCoord);
                #endif

                //return half4(0, 1, 0, 1);
                return half4(col);
            }

            ENDHLSL

        }
    }
    
}