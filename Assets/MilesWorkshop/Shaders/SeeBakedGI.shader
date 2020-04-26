Shader "MileShader/SeeBakedGI" {
  Properties {
    _MainTex("Albedo (RGB)", 2D) = "white" {}
    _Lightmap("Lightmap", 2D) = "white" {}
    _SpecMap("SepcularMap", 2D) = "white" {}
    _Gloss("Glossiness", Range(0.1, 200)) = 0.5
    _Specular("Specular", Range(0, 1)) = 0.5
    _FakeLightDir("Light Direction", Vector) = (0.25, 1, -0.25, 0)
    //[HideInInspector]_TempST("UU", Vector) = (0.2633236, 0.2633236, -0.001028608, 0.6786589)
  }
  SubShader {
    Tags{"Queue" = "Geometry+1"}
    Pass {
        Tags{"LightMode" = "UniversalForward"}
      HLSLPROGRAM

     #pragma prefer_hlslcc gles
     #pragma exclude_renderers d3d11_9x
     #pragma target 2.0

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
      #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
      #pragma multi_compile _ _SHADOWS_SOFT
      #pragma shader_feature _RECEIVE_SHADOWS_OFF
      #pragma multi_compile _ LIGHTMAP_ON
      #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

      #pragma multi_compile_fog
      #pragma multi_compile_instancing

      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

      sampler2D _MainTex;
      CBUFFER_START(UnityPerMaterial)
      float4 _MainTex_ST;
      // This is created by unity light baking system
      sampler2D _Lightmap;
      float4 _Lightmap_ST;
      // other sampler2D won't need _ST, all share _MainTex_ST
      sampler2D _SpecMap;
      float _Gloss;
      half _Specular;
      half4 _FakeLightDir;
      float4  _TempST;
      CBUFFER_END
      
      struct a2v {
        float4 vertex :POSITION;
        float2 uv0 :TEXCOORD0;
        float2 uv1 :TEXCOORD1;
        half3 normal :NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f {
        float4 positionCS :SV_POSITION;
        float3 positionWS :TEXCOORD5;
        DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
        float2 uv :TEXCOORD0;
        //float2 uvlm :TEXCOORD1;
        half4 fogFactorAndVertexLight :TEXCOORD4;
        half3 normal_World :TEXCOORD2;   
        half3 view_World :TEXCOORD3;       
        #ifdef _MAIN_LIGHT_SHADOWS
        float4 shadowCoord :TEXCOORD6;
        #endif      
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
      };
      
      void InitializeInputData(v2f o, out InputData inputData) {
        inputData.positionWS = o.positionWS;
        inputData.normalWS = NormalizeNormalPerPixel(o.normal_World);
        inputData.viewDirectionWS = SafeNormalize(o.view_World);
        
        #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
            inputData.shadowCoord = o.shadowCoord;
        #else
            inputData.shadowCoord = float4(0, 0, 0, 0);
        #endif
        
        inputData.fogCoord = o.fogFactorAndVertexLight.x;
        inputData.vertexLighting = o.fogFactorAndVertexLight.yzw;
        // this is where the SH or Lightmap look will be calculated.
        inputData.bakedGI = SAMPLE_GI(o.lightmapUV, o.vertexSH, inputData.normalWS);
      }
      
      v2f vert(a2v v) {
        v2f o;
        o = (v2f)0;
        VertexPositionInputs vpi = GetVertexPositionInputs(v.vertex.xyz);
        o.positionCS = vpi.positionCS;
        VertexNormalInputs vni = GetVertexNormalInputs(v.normal);
        o.normal_World = vni.normalWS;
        //o.uvlm = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;     
        o.view_World = GetCameraPositionWS() - vpi.positionWS;
        half3 vertexLight = VertexLighting(vpi.positionWS, o.normal_World);
        half fogFactor = ComputeFogFactor(vpi.positionCS.z);
        o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
        o.positionWS = vpi.positionWS;
        o.normal_World = NormalizeNormalPerPixel(vni.normalWS);
        // initialize o.lightmapUV from v2f
        OUTPUT_LIGHTMAP_UV(v.uv1, unity_LightmapST, o.lightmapUV);
        // initialize o.vertexSH from v2f
        OUTPUT_SH(o.normal_World.xyz, o.vertexSH);
        o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
        #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
        o.shadowCoord = GetShadowCoord(vpi);
        #endif
        
        return o;
      }
      
      half4 frag(v2f i) : SV_Target {
          UNITY_SETUP_INSTANCE_ID(input);
          UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
          //i.normal_World = normalize(i.normal_World);
          // environment reflection
          half3 reflectVector = reflect(-i.view_World, i.normal_World);
          // input reflect vector, roughness, occlusion
          half3 reflectionEnv = GlossyEnvironmentReflection(reflectVector, 0.5, 1);
          //half3 lmColor = SampleLightmap(i.uvlm, i.normal_World);
          InputData inputData;
          InitializeInputData(i, inputData);
          Light mainLight = GetMainLight(inputData.shadowCoord);
       
          // inputData.bakeGI will be calculated with realtime shadow now
          MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));


          half4 c = half4(inputData.bakedGI, 1);
          return c;
          //return inputData.shadowCoord;
        }
      ENDHLSL
    }
    //UsePass "Universal Render Pipeline/Lit/DepthOnly"
    
    Pass {
        Tags {"LightMode" = "DepthOnly"}
    }
    
  }
  Fallback "Diffuse"
}