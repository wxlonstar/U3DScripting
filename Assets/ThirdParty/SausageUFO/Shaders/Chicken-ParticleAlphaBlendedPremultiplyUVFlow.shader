Shader "Chicken/ParticleAlphaBlendedPremultiplyUVFlow"
{
    Properties
    {
        _TintColor("Tint Color",Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex ("Texture", 2D) = "white" {}
        _FlowVector("Flow Vector", Vector) = (0.0, 0.0, 0.0, 0.0)
        _FlowSpeed("Flow Speed", Float) = 1.0
    }
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IGNOREPROJECTOR" = "true"  "PreviewType" = "Plane" }

		Pass
		{
			Tags { "LightModel" = "LightweightForward" }
			Cull Off
			ZWrite Off
			Blend One OneMinusSrcAlpha
			
			HLSLPROGRAM
			#pragma target 2.0
			#pragma prefer_hlslcc gles
			#pragma multi_compile_instancing
			#pragma vertex VertParticle
			#pragma fragment FragParticle
			
            #define _USE_PREMULTIPLY_ALPHA 1
            #define _USE_UV_FLOW 1

			#include "Chicken-ParticleCommon.hlsl"
			ENDHLSL
		}
	}
}
