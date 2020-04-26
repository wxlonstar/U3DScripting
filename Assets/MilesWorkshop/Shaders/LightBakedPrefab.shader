Shader "MileShader/LightBakedPrefab" {
  Properties {
    _MainTex("Albedo (RGB)", 2D) = "white" {}
    _Lightmap("Lightmap", 2D) = "white" {}
    _FakeHalfDir("Light Direction", Vector) = (0, 1, -0.25, 0)
  }
  SubShader {
    Tags{"Queue" = "Geometry+1"}
    Pass {
        Name "LBP"
        Tags {"LightMode" = "UniversalForward"}
        HLSLPROGRAM
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0

        #pragma vertex vert
        #pragma fragment frag

        // when work with realtime shadows I need these macros
        #pragma shader_feature _RECEIVE_SHADOWS_OFF
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

        #pragma multi_compile_fog
        #pragma multi_compile_instancing

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


        sampler2D _MainTex;
        sampler2D _Lightmap;
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _Lightmap_ST;
        CBUFFER_END

        struct a2v {
            float4 positionOS :POSITION;
            float2 uv1 :TEXCOORD0;
            float2 uv2 :TEXCOORD1;
            half3 normal :NORMAL;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f {
            float4 positionCS :SV_POSITION;
            float2 uvTex :TEXCOORD0;
            float2 uvLightmap :TEXCOORD1; 
            half3 normal_World :TEXCOORD2;
            half3 view_World :TEXCOORD3;
            #ifdef _MAIN_LIGHT_SHADOWS
            float4 shadowCoord :TEXCOORD4;
            #endif
            // for optimization, mix fog factor with positionWS
            float4 positionWSAndFog :TEXCOORD5;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert(a2v v) {
            v2f o;
            // intializing
            o = (v2f)0;
            VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
            o.positionCS = vpi.positionCS;
            
            o.positionWSAndFog.xyz = vpi.positionWS;
            o.positionWSAndFog.w = ComputeFogFactor(vpi.positionCS.z);
            o.uvTex = TRANSFORM_TEX(v.uv1, _MainTex);
            o.uvLightmap = TRANSFORM_TEX(v.uv2, _Lightmap);
            VertexNormalInputs vni = GetVertexNormalInputs(v.normal.xyz);
            o.normal_World = vni.normalWS;
            o.view_World = GetCameraPositionWS() - vpi.positionWS;
            #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
            o.shadowCoord = GetShadowCoord(vpi);
            #endif
            
            return o;
        }

        void InitializeInputData(v2f o, out InputData inputData) {
            inputData.positionWS = o.positionWSAndFog.xyz;
            inputData.normalWS = NormalizeNormalPerPixel(o.normal_World);
            inputData.viewDirectionWS = SafeNormalize(o.view_World);
            #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
            inputData.shadowCoord = o.shadowCoord;
            #else
            inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif
            inputData.fogCoord = o.positionWSAndFog.w;
            inputData.vertexLighting = half3(0, 0, 0);
            inputData.bakedGI = SampleSHVertex(inputData.normalWS);
        }

        half4 frag(v2f i) : SV_Target {
            UNITY_SETUP_INSTANCE_ID(i);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
            
            InputData inputData;
            InitializeInputData(i, inputData);
            Light mainLight = GetMainLight(inputData.shadowCoord);
            // enable realtime shadow
            MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));
            return half4(mainLight.shadowAttenuation, mainLight.shadowAttenuation, mainLight.shadowAttenuation, 1);
        }


        ENDHLSL
    }
  }
}