Shader "SoFunny/Chicken-Terrain_v2" {
    Properties {
        [NoScaleOffset]_Control01("Splatmap01", 2D) = "white" {}
        [NoScaleOffset]_Control02("Splatmap02", 2D) = "white" {}
        /*
        _TexLayer01("Layer01 Texture", 2D) = "white" {}
        _TexLayer02("Layer02 Texture", 2D) = "white" {}
        _TexLayer03("Layer03 Texture", 2D) = "white" {}
        _TexLayer04("Layer04 Texture", 2D) = "white" {}
        _TexLayer05("Layer05 Texture", 2D) = "white" {}
        _TexLayer06("Layer06 Texture", 2D) = "white" {}
        _TexLayer07("Layer07 Texture", 2D) = "white" {}
        _TexLayer08("Layer08 Texture", 2D) = "white" {}
        */
        _Color01("Color01", Color) = (1, 1, 1, 1)
        _Color02("Color02", Color) = (1, 1, 1, 1)
        _Color03("Color03", Color) = (1, 1, 1, 1)
        _Color04("Color04", Color) = (1, 1, 1, 1)
        _Color05("Color05", Color) = (1, 1, 1, 1)
        _Color06("Color06", Color) = (1, 1, 1, 1)
        _Color07("Color07", Color) = (1, 1, 1, 1)
        _Color08("Color08", Color) = (1, 1, 1, 1)
        _Weight("Blend Weight", Range(0.001, 1)) = 0.05
        _Test("Test", Range(0, 1)) = 1
    }
    SubShader {
        Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "LightweightPipeline" "IgnoreProjector" = "True"}
        
        Pass {
            Name "Layer01"
            Tags{"LightMode" = "UniversalForward"}
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma vertex vert
            #pragma fragment frag

        
            #include "ChickenTerrainInputs.hlsl"
            #include "ChickenTerrainPass.hlsl"

            ENDHLSL
            
            
        }
     /*
        Pass {
            Name "Layer02"
            Blend One One
            //Tags{"LightMode" = "LightweightForward"}
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma vertex vert
            #pragma fragment frag

            #include "ChickenTerrainInputs.hlsl"
            #include "ChickenTerrainPassAdd.hlsl"

            ENDHLSL
        }
        */
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}
        }
    }
    Dependency "AddPassShader" = "Hidden/ssUniversal Render Pipeline/Terrain/Lit (Add Pass)"
}
