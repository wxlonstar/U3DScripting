Shader "Unlit/GrassShaderWind_Lightmap"
{
    Properties
    {
        _Color ("Color", Color)=(1,1,1,1)
        _LightStrengh("LightStrength", Range(0,1))=0.2
        _Gloss("Gloss", Range(0,4))=1.5
        _WindSpeed("WindSpeed", Range(0,5))=2
        _GrassShakePos("GrassShakePos",Range(0,1))=0.5
        //_MainTex ("Texture", 2D) = "white" {}
        _VerticalBillboarding("Vertical Restraints", Range(0, 1)) = 1
        //风
        _Wind("Wind",2D)="white"{}//突变风采样纹理
        _WindSpecular("WindSpecular",2D)="white"{}//风高光采样纹理
        _Windvector("Windvector",Vector)=(1,0,0,0)//风的方向（世界空间）
		_TimeScale("TimeScale",float)=1//时间频数
		_Mapwidth("Mapwidth",float)=80//地图大小（用来控制突变风效果）
        _WindStrength("WindStrength",float)=1

        _GrassColor("GrassColor",2D)="white"{}//草地颜色采样纹理
        _GrassTexColorBlend("GrassTexColorBlend", Range(0,1.5))=1

        [Header(LightMap)]
        _LightmapIntensity("光照贴图强度 Lightmap Intensity",Range(-2,2)) = 0.5
        _LightmapAdd("光照贴图增益 Lightmap Add",Range(-2,2)) = 0.5
        [Toggle(USE_LM_SHADOWCOLOR)] _UseLightmapShadowColor("使用光照贴图阴影色 Use Lightmap Shadow Color",Float) = 0
        _LightmapShadowColor("光照贴图阴影色 Lightmap Shadow Color", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile __ LIGHTMAP_ON
            #pragma multi_compile __ USE_LM_SHADOWCOLOR

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
                float3 normal : NORMAL;
                #if LIGHTMAP_ON
                    float2 uv_LightMap : TEXCOORD1;
                #endif
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldNormal : TEXCOORD2; // xyz:normal w:fresnel
                float3 worldLight : TEXCOORD3;
                //float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD4;
                float mapVal: TEXCOORD5;
                float3 grassCol: TEXCOORD6;
                #if LIGHTMAP_ON
                    float2 lightMap : TEXCOORD7;
                #endif
            };
            sampler2D _Wind;
            sampler2D _GrassColor;
            sampler2D _WindSpecular;
            //sampler2D _MainTex;
            
            CBUFFER_START(UnityPerMaterial)

            //float4 _MainTex_ST;
            
            float4 _GrassColor_ST;
            fixed4 _Color;
            fixed _LightStrengh;
            float4 _Windvector;
			float _TimeScale;
			float _Mapwidth;
            fixed _VerticalBillboarding;
            half _WindStrength;
            half _Gloss;
            half _WindSpeed;
            half _GrassTexColorBlend;

            fixed _GrassShakePos;

            half _LightmapIntensity;
            half _LightmapAdd;
            half _UseLightmapShadowColor;
            fixed4 _LightmapShadowColor;


            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv=v.uv;

                #if LIGHTMAP_ON
                    o.lightMap = v.uv_LightMap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif

                o.uv=v.vertex.xyz;
                
                o.worldNormal = float4(normalize(mul(v.normal, (half3x3)unity_WorldToObject)),1.0);
                o.worldLight = WorldSpaceLightDir(v.vertex);

                //-------------
                float3 center = float3(0, 0, 0);
				float3 viewer = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1));
				float3 normalDir = viewer - center;
				normalDir.y = normalDir.y * _VerticalBillboarding;
				normalDir = normalize(normalDir);
				float3 upDir = abs(normalDir.y) > 0.9999 ? float3(0, 0, 1) : float3(0, 1, 0);
				float3 rightDir = normalize(cross(upDir, normalDir));
                //float3 rightDir = normalize(cross(upDir, float3(0,0,1)));
				upDir = float3(0,1,0);
                float4 worldpos=mul(unity_ObjectToWorld,float4(center,1));
                float4 vertexWorldpos=mul(unity_ObjectToWorld,v.vertex);
                //-------------
				float3 centerOffs = v.vertex.xyz - center;
				float3 localPos = center + rightDir * centerOffs.x + upDir * centerOffs.y+ normalDir * centerOffs.z;//根据顶点相对锚点的偏移与正交基矢量相乘得到新定点位置
				
                //float3 localPos = center + rightDir * centerOffs.x + upDir * centerOffs.y+ centerOffs.z;
                //---风
				float windmutation =1-tex2Dlod(_Wind,float4(worldpos.x/_Mapwidth+_Time.x*_WindSpeed,worldpos.z/_Mapwidth,0,0)).b;
                //---云
                //float cloudmutation =1-tex2Dlod(_Wind,float4(worldpos.x/_Mapwidth+_Time.x,worldpos.z/_Mapwidth,0,0)).b;
                //---草地
                fixed3 grasscolor = tex2Dlod(_GrassColor,float4(vertexWorldpos.x/_GrassColor_ST.x+_GrassColor_ST.z,vertexWorldpos.z/_GrassColor_ST.y+_GrassColor_ST.w,0,0)).rgb;
				
                float time=(_Time.y)*(_TimeScale);
				float4 localwindvector=_Windvector;//normalize(mul(unity_WorldToObject,_Windvector));

                //旧版风动
                //localPos+=sin(time+windmutation*10)*cos(time*2/3+1+windmutation*10)*localwindvector.xyz*clamp(v.vertex.y-_GrassShakePos,0,1);//---因为草的根部是不摆动的，风吹草不会左右摆，而是向左/右摆动，然后回到中间所以用cos修正
				
				localPos+=sin(time+windmutation*10+worldpos.x+worldpos.z)*cos(time*2/3+1+windmutation*10)*localwindvector.xyz*clamp(v.vertex.y-_GrassShakePos,0,1);//---因为草的根部是不摆动的，风吹草不会左右摆，而是向左/右摆动，然后回到中间所以用cos修正
				//o.vertex = UnityObjectToClipPos(float4(localPos, 1));
                

                v.vertex.xz+=v.vertex.y*_WindStrength*localPos*(0.6+windmutation*0.4);
                //v.vertex.xyz = v.vertex.xyz+((localPos)*_WindStrength);
                o.vertex = UnityObjectToClipPos(v.vertex);

				o.worldPos = mul(unity_ObjectToWorld,float4(localPos,1));

                //高光风场
                float windmutationSpecular =1-tex2Dlod(_WindSpecular,float4(worldpos.x/_Mapwidth+_Time.x*_WindSpeed,worldpos.z/_Mapwidth,0,0)).b;

                o.mapVal = windmutationSpecular;
                o.grassCol = grasscolor;
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;



            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv)*i.uv.y;
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz-i.worldPos.xyz);
                fixed3 halfDir = normalize(worldLightDir + viewDir);
                fixed3 specular = pow(max(0, dot(worldNormal, halfDir)), (5-_Gloss));


                float dotResult = saturate(1.0 - dot(i.worldNormal.xyz,i.worldLight));
                fixed4 col = (_Color*saturate(1-_GrassTexColorBlend)+fixed4(i.grassCol,1)*_GrassTexColorBlend);
                col*=((1-_LightStrengh)+dotResult*_LightStrengh);
                col+=(i.mapVal*_Gloss*0.25*i.uv.y);


                #if LIGHTMAP_ON
                    float3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightMap.xy));
                    col.rgb *= lm * _LightmapIntensity + _LightmapAdd;
                    #if USE_LM_SHADOWCOLOR
                        fixed3 lmShadow= 1-saturate(lm);
                        col.rgb = col.rgb*(1-lmShadow*_LightmapShadowColor.a) + _LightmapShadowColor * lmShadow*_LightmapShadowColor.a;
                    #endif
                #endif

                

                //col+=fixed4(specular*i.mapVal,1);
                //fixed4 col = _Color+fixed4(specular,1);
                //return fixed4(i.mapVal,i.mapVal,i.mapVal,1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //col=_Color*(0.2+i.uv.y*0.8);
                //col*=(1.0+pow(i.uv.y,6)*0.2);
                return col;
                
                //return col*(0.85+(i.mapVal)*0.3);
            }
            ENDCG
        }
    }
}
