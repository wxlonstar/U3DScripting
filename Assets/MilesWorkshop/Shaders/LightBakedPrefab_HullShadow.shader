Shader "MileShader/LightBakedPrefab_HullShadow" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Brightness("Brightness", Range(0, 1)) = 0.4
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_FakeLightDir("Light Direction", Vector) = (0.25, 1, -0.25, 0)
	}
	SubShader {		
		Pass {
			Tags {"Queue" = "Geometry+1"}
			Name "ShadowCaster"
			Stencil {
				Ref 2
				Comp equal
				Pass keep
			}
			Tags {"LightMode" = "ShadowCaster"}
			ZWrite Off
			ZTest LEqual
		}
	
	}
}