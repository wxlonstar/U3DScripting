Shader "Sofunny/shd_water_v1" {
	Properties {
		_TotalScale ("Total Scale", float) = 1800
		_ColorGradient("Gradient", 2D) = "white" {}
		_GradientDistance("Color Gradient Distance", float) = 4.5
		
		_FoamColor ("Foam Color", color) = (0.94, 0.96, 0.9725, 1)
		_PlayerFoamColor ("Player Foam Color", color) = (0.94, 0.96, 0.9725, 1)
		_FoamMainTex("Foam Main Texture", 2D) = "black" {}
		
		_FoamDetailTex("Foam Detail Texture", 2D) = "black" {}
		_NoiseTex("Noise Texture", 2D) = "black"{}
		_DissolveRange("Dissolve Range", float) = 0.7

		_FoamMoveDistance("Foam Move Distance", Float) = 1.5
		_PlayerFoamMoveDistance("Player Foam Move Distance", Float) = 0.65

		_FoamWidth("Foam Width", Float) = 6
		_PlayerFoamWidth("Player Foam Width", Float) = 4

		_CausticsTex ("Caustics Texture", 2D) = "Black" {}

		_NormalTex("Normal Texture", 2D) = "bump" {}
		_NormalScale ("NormalScale", range(0, 1)) = 0.2
		
		_Fresnel("Fresnel", float) = 3.04
		_Shininess("Shininess", Range(0.1, 1)) = 0.1
		[HDR]_SpecColor ("Specular Color", color) = (0.75,0.75,0.75,1)
       
		_WindSpeed("WindSpeed", float) = 1.2
		_WaveSpeed("WaveSpeed", float) = 12
		_ShakeStep ("Shake Step", float) = 3
		_ShakeSpeed ("Shake Speed", Range (0, 0.1)) = 0.06
	
		_CubeTex("Reflection Texture", Cube) = "" {} 
		_BlendCol ("Blend Color", Color) = (0.416, 0.827, 0.97, 1)
		_Near("Near", float) = 47.4
		_Far("Far", float) = 600
		
		_CullCol ("CullColor", COLOR) = (0.1019, 0.38, 0.73, 1)
	}
	SubShader {
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"IgnoreProjector" = "true"}

		Blend srcalpha oneminussrcalpha
		ZWrite Off
		Cull Off

		Pass {
			Tags {"LightMode" = "LightweightForward"}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _CULL_OFF _CULL_ON
			#pragma multi_compile _FOAM _NOTFOAM
			#pragma multi_compile _CAUSTICS _NOTCAUSTICS
			#include "UnityCG.cginc"

			CBUFFER_START(UnityPerMaterial)
			
			sampler2D _CameraDepthTexture;
			sampler2D _CameraOpaqueTexture;
	
			half _TotalScale;

			sampler2D _NormalTex; float4 _NormalTex_ST;
			half _NormalScale;

			sampler2D _ColorGradient;
			sampler2D _CausticsTex; float4 _CausticsTex_ST;

			half _WindSpeed;
			half _WaveSpeed;
			half _Fresnel;
			half _ShakeSpeed;

			float _Shininess;
			fixed4 _SpecColor;

			sampler2D _FoamDetailTex; float4 _FoamDetailTex_ST;
			sampler2D _FoamMainTex; float4 _FoamMainTex_ST;
			fixed4 _FoamColor;
			fixed4 _PlayerFoamColor;
			half _FoamMoveDistance;
			half _PlayerFoamMoveDistance;
			half _PlayerFoamWidth;
			half _FoamWidth;

			sampler2D _NoiseTex; float4 _NoiseTex_ST;
			samplerCUBE _CubeTex;

			fixed4 _BlendCol;
			half _Far,_Near;
			half _ShakeStep;
			half _GradientDistance;

			float _DissolveRange;

			fixed4 _CullCol;
			CBUFFER_END

			float Highlights(float roughness, float3 normalWS, float3 viewDirectionWS,float3 lightDir) {
				float roughness2 = roughness * roughness;
				roughness2 = roughness2 * 0.00001;
				float3 halfDir =normalize(lightDir + viewDirectionWS);
				float NoH = saturate(dot(normalWS, halfDir));
				float LoH = saturate(dot(lightDir, halfDir));
				float d = NoH * NoH * (roughness2 - 1) + 1;
				float LoH2 = LoH * LoH;
				float specularTerm = roughness2 / ((d * d) * max(0.1, LoH2) * (roughness + 0.5) * 4);
				return  specularTerm;  
			}

			float Remap(float In, float2 InMinMax, float2 OutMinMax) {
				float outValue = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
				return outValue;
			}

			fixed3 RGBSplit(float split, sampler2D tex, float2 uv) {
				float2 uvR = uv + float2(split, split);
				float2 uvG = uv + float2(split, -split);
				float2 uvB = uv + float2(-split, -split);

				fixed r = tex2D(tex, uvR).r;
				fixed g = tex2D(tex, uvG).g;
				fixed b = tex2D(tex, uvB).b;

				return fixed3(r,g,b);
			}

			fixed3 CausticsTexture(float2 uv, float split, float strength, sampler2D tex) {
				fixed3 texture_1 = RGBSplit(split * 0.01, tex, uv);
				fixed3 texture_combined = texture_1;
				return texture_combined;
			}

			inline float LinearEyeDepthNew(float z)
			{
				return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
			}

			struct appdata {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos :SV_POSITION;
				float4 uv2 :TEXCOORD0;
				float4 uv :TEXCOORD2;
				#if _CULL_OFF
				float4 uv3 :TEXCOORD3;
				#endif
				float4 screenUV :TEXCOORD4;
				float3 lightDir :TEXCOORD5;
				float3 viewDir :TEXCOORD6;
				half3 t2w[3] :TEXCOORD7;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.uv ;
				o.uv2.zw = TRANSFORM_TEX(v.uv, _NormalTex)* _TotalScale;
				COMPUTE_EYEDEPTH(o.uv.z);

				#if _CULL_OFF
				o.uv2.xy = TRANSFORM_TEX(v.uv, _NoiseTex) * _TotalScale;
				o.uv3.xy = TRANSFORM_TEX(v.uv, _CausticsTex) * _TotalScale;
				o.uv3.z = TRANSFORM_TEX(v.uv, _FoamMainTex) * _TotalScale;
				o.uv3.w = TRANSFORM_TEX(v.uv, _FoamDetailTex) * _TotalScale;
				#endif

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				half3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				o.t2w[0] = half3(worldTangent.x, worldBinormal.x, worldNormal.x);
				o.t2w[1] = half3(worldTangent.y, worldBinormal.y, worldNormal.y);
				o.t2w[2] = half3(worldTangent.z, worldBinormal.z, worldNormal.z);

				o.screenUV = ComputeScreenPos(o.pos);
				o.lightDir =  WorldSpaceLightDir(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				// Dir
				float3 lightDir = normalize(i.lightDir);
				float3 viewDir = normalize(i.viewDir);

				float windTimer = frac(_Time.x * _WindSpeed);
				float timeX = _Time.x % 100;

				// Normal 
				half3 normalVar01 = UnpackNormal(tex2D(_NormalTex, i.uv2.zw - float2(0.5, frac(_Time.x * _WindSpeed * 0.2))));
				half3 normalVar02 = UnpackNormal(tex2D(_NormalTex, i.uv2.zw + float2(0, windTimer)));
				half3 finalNormal = normalize(float3(normalVar01.rg + normalVar02.rg, normalVar01.b * normalVar02.b));
				finalNormal = half3(finalNormal.rg * _NormalScale, lerp(1, finalNormal.b, saturate(_NormalScale)));
				fixed3 worldNormal = normalize(fixed3(dot(i.t2w[0], finalNormal.xyz), dot(i.t2w[1], finalNormal.xyz), dot(i.t2w[2], finalNormal.xyz)));

				// Fresnel
				float fresnel = saturate(dot( worldNormal, viewDir));
				fresnel = exp(-_Fresnel * fresnel);

				// OpaqueTexture
				fixed4 colorTexVar = tex2Dproj(_CameraOpaqueTexture , UNITY_PROJ_COORD(i.screenUV));
				fixed signMask =saturate(Remap(colorTexVar.a, float2(0.5, 1), float2(1, 0)));
				fixed cullMask = saturate(colorTexVar.a * 2 + (1 - signMask));

				// Depth
				#if _CULL_OFF
					float4 depthTexVar = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenUV));
					float depthDifference = LinearEyeDepthNew(depthTexVar.r) - ( i.screenUV.w);
					float moveDistance = lerp(_PlayerFoamWidth, _FoamWidth, signMask) ;
					float foamDepthDifference = saturate(depthDifference / moveDistance);
					foamDepthDifference = 1 - pow(1 - foamDepthDifference, 5);
				#endif
				
				// Wave
				#if _CULL_OFF
					float shake = i.uv.x * i.uv.y * _TotalScale * _ShakeStep;
					shake += timeX * 25 * _ShakeSpeed * _WaveSpeed;
					float waveShake = abs(1 - pow((1 - sin(shake)), 1.5) - sin(shake * 0.1)); 
					waveShake *= 0.5;
					waveShake = saturate(waveShake);
				#endif

				// Main Color
				#if _CULL_OFF
					float colorDistance = saturate(depthDifference / _GradientDistance);
					colorDistance = 1 - pow(1 - colorDistance, 5);
					fixed4 mainCol = tex2D(_ColorGradient, float2(colorDistance, 1)); 
				#else
					fixed4 mainCol = _CullCol; 
				#endif
			
				// Specular 
				fixed3 Specular = Highlights(_Shininess, worldNormal, viewDir, lightDir) * _SpecColor;

				// Reflect
				float3 reflectDir = normalize(reflect(-viewDir, worldNormal));
				half3 reflection = texCUBE(_CubeTex, reflectDir).rgb; 

				// Foam
				#if _CULL_OFF
					#if _FOAM
						float coastMask = step(foamDepthDifference, 0.99);
						float waveTimer = sin(timeX * _WaveSpeed)- sin(timeX + 0.5); 
						waveTimer *= 0.5;
						half foamMoveDis = lerp(_PlayerFoamMoveDistance, _FoamMoveDistance, signMask);

						float waveShakeTimer =  waveTimer - waveShake * 0.35;
						waveShakeTimer = clamp(waveShakeTimer, -1, 1);
						float waveMoveFactor =  foamMoveDis * waveShakeTimer * (1 - foamDepthDifference);
						float waveFactor = foamDepthDifference - waveMoveFactor * cullMask;
						waveFactor = lerp(foamDepthDifference, waveFactor, waveTimer);

						float foamMainVar = tex2D(_FoamMainTex, float2(waveFactor, i.uv3.z)).r * cullMask;
						fixed noiseTexVar = tex2D(_NoiseTex, float2(waveFactor, i.uv2.x) + float2(frac(_Time.x * 1.5 * _WindSpeed), 0));
						fixed foamDetailVar = tex2D(_FoamDetailTex , float2(waveFactor, i.uv3.w) + float2(frac(_Time.x * _WindSpeed * 2), 0)); 
						float foamMask =1 - saturate(pow(abs(Remap(waveFactor,float2(0,1), float2(-1,1))),_DissolveRange));
						foamMask = step(noiseTexVar, foamMask);

						float foamDetail = foamDetailVar * foamMask * coastMask;
						foamDetail *= 1 - foamDepthDifference;
						float finalfoam = pow( max(foamMainVar.r,foamDetail) , Remap(abs(waveTimer), float2(0, 1),float2(1,5))) * (1 - foamDepthDifference);;
						fixed3 foamCol = lerp(finalfoam * mainCol.rgb * mainCol.a, lerp(_PlayerFoamColor, _FoamColor, signMask), finalfoam * 2);
					#endif
				#endif


				// Caustics
				#if _CULL_OFF
					#if _CAUSTICS
						fixed3 Caustics = CausticsTexture(i.uv3.xy  + fresnel * 0.8 - float2(0,windTimer) ,waveShake, 1, _CausticsTex) * (1 - mainCol.a);
						#if _FOAM
						Caustics *= 1 - finalfoam;
						#endif
					#endif
				#endif

				// Alpha
				fixed alpha;
				#if _CULL_OFF
					#if _FOAM
						alpha = saturate(step(0, waveFactor) * (mainCol.a + finalfoam * max(0.1, 1 - abs(waveTimer) * cullMask) * _FoamColor.a));
				    #else
						alpha = 1;
					#endif
				#else
					alpha = 1;
				#endif

				// Blend
				half tintFactor = saturate((i.uv.z - _Near) / (_Far - _Near));

				// Out Color
				fixed4 finalCol; 
				finalCol.rgb = mainCol + Specular ;
				
				#if _CULL_OFF
					#if _CAUSTICS
					    finalCol.rgb += Caustics * 0.7;
					#endif
				#endif

				finalCol.rgb = lerp(finalCol.rgb, reflection, fresnel);

				#if _CULL_OFF
					#if _FOAM
					    finalCol.rgb += foamCol;
					#endif
				#endif

				finalCol.rgb = lerp(finalCol.rgb, _BlendCol, tintFactor);
				finalCol.a = alpha;

				return finalCol;
			}
			ENDHLSL
		}
	}
	FallBack "Unlit/Texture"
}
