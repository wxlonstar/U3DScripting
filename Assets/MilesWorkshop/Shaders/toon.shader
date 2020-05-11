Shader "Hidden/toon"
{
    Properties
    {
	    _NormalMap("NormalMap",2D) = ""{}
	//	_LM("IML",2D) = ""{}
	   //diffuse
        _Diffuse("Texture", 2D) = "white" {}      
		_Percent("Percent",Range(0,1)) = 0.5      //正背面比例    
		_FrontColor("FColor",Color)=(1,1,1,1)    //正面色彩倾向
		_BlackColor("BColor",Color)=(0,0,0,1)    //背面色彩倾向
		//spe
		[HDR]_SpeColor("SpeColor",Color)= (1,1,1,1)
		_SpeRange("Spepercent",Range(0,1)) = 0.5  //范围
		_SpeHeight("SpeHeight",Range(0,1)) = 1    //强度
		//outline
		_OutLineColor("OutLineColor",Color)=(1,1,1,1)
		_OutHeight("OutHeight",Range(0,0.2)) = 0.1
		//mask
		_Mask("Mask",2D)=""{}
		//pbr
		_Metallic("Metallic",Range(0,1))= 0
		_Roughtness("Roughtness",Range(0,1)) = 0.5
		_Refection("Recfection",Cube)=""{}
	//	_Refection("Refection",2D) = ""{}
		//Emiss
		[HDR]_Emission("Emission",Color)=(1,1,1,1)
		[HDR]_EmissionTwo("EmissionTwo",Color)=(1,1,1,1)
		_EmissSpeed("EmissSpeed",Range(0,5)) = 1
    }
    SubShader
    {
        
		Tags{"RenderType" = "Opaque" "RenderPipeline" = "LightweightPipeline" "IgnoreProjector" = "True"}
        Pass
        {
		   Name "Forwardlight"
		   Tags{"LightMode" = "LightweightForward"}
		    Cull Back ZWrite On 
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_instancing

            
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
			CBUFFER_START(UnityPerMaterial)
            CBUFFER_END


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 w1 : TEXCOORD1;
				float4 w2 : TEXCOORD2;
				float4 w3 : TEXCOORD3;
				float4 vertcolor : TEXCOORD4;
				 UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex =TransformObjectToHClip(v.vertex.xyz);
				float3 worldnormal = normalize(TransformObjectToWorldNormal(v.normal));
		    	float3 worldtangent =normalize(TransformObjectToWorldDir(v.tangent.xyz));
				float3 BTangent = normalize( cross(worldnormal,worldtangent)* v.tangent.w);
				float3 worldpos = TransformObjectToWorld(v.vertex.xyz);
				o.w1 = float4(worldtangent.x,BTangent.x,worldnormal.x,worldpos.x);
				o.w2 = float4(worldtangent.y,BTangent.y,worldnormal.y,worldpos.y);
				o.w3 = float4(worldtangent.z,BTangent.z,worldnormal.z,worldpos.z);
                o.uv = v.uv;
				o.vertcolor = v.color;
                return o;
            }
			samplerCUBE _Refection;
		//	sampler2D _Refection;
          sampler2D  _Diffuse;
		  sampler2D _NormalMap;
		  float4 _Lightdir = (0,0,-1,0);
		  float _Percent;
		  float4 _BlackColor;
		  float4 _FrontColor;

		  float4 _SpeColor;
		  float _SpeRange;
		  float _SpeHeight;

		  float _Metallic;
		  float _Roughtness;
		  sampler2D _Mask;
		  float4 _Emission;
		  float4 _EmissionTwo;
		  float _EmissSpeed;
		  //D
		  float D(float roughness,float nh ){
		  float SRoughness = pow(roughness,2);
		  return SRoughness/(pow((pow(nh,2)*(SRoughness-1)+1) ,2)*3.14f);
		  }

		  //G
		  float G(float nl , float nv ,float roughness ) {
		  float k = pow((roughness+1),2)/8;
		  float left = nl/lerp(nl,1,k);
		  float right = nv/lerp(nv,1,k);
		  return left * right;

		  }
		  //F
		  float F(float vh,float metallic,float3 albedo){
		  	  float3 f0 = lerp(0,albedo,metallic);
			  return f0+(1-f0)*exp2((-5.55473*vh-6.98316)*vh);
		  }



            half4 frag (v2f i) : SV_Target
            {
			    UNITY_SETUP_INSTANCE_ID(i);

				half4 col = tex2D(_Diffuse, i.uv);

				float3 worldpos = float3(i.w1.w,i.w2.w,i.w3.w);
				float3 viewdir = normalize(_WorldSpaceCameraPos.xyz - worldpos);
				float3 lightdir =normalize( worldpos - _Lightdir.xyz );
				float3 tangentnor =normalize( UnpackNormal(tex2D(_NormalMap,i.uv)));
				float3 worldnormal =normalize( float3(dot(tangentnor,i.w1.xyz),dot(tangentnor,i.w2.xyz),dot(tangentnor,i.w3.xyz)) );
				float nl = saturate(dot(worldnormal,lightdir));
				nl =clamp(step(nl,_Percent)+ (1-i.vertcolor),0,1);
				float4 diffcolor = lerp(_BlackColor,_FrontColor,nl);
				
			//二分高光
			float3 wn = float3(i.w1.z,i.w2.z,i.w3.z);
			float3 h = normalize(lightdir + viewdir);
			float spe = saturate(dot(worldnormal,h) );
			spe =clamp( step(spe,_SpeRange)+(1-i.vertcolor)*0.8f,0,1);
		   float4	SpeColor = _SpeColor*spe*_SpeHeight;

			//pbr高光
			float nv = saturate(dot(worldnormal,viewdir));
			float nh = saturate(dot(worldnormal,h));
			float vh = saturate(dot(viewdir,h));

			float d = D(_Roughtness,nh);
			float g = G(nl,nv,_Roughtness);
			float f = F(vh,_Metallic,col);
			float3 Dspecular =((d*g*f*0.25)/(nv*nl))*_SpeColor*nl *3.14;  //直接高光
			                                       
			float3 reflection = reflect(-viewdir,worldnormal);
		//	Ispecular = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflection, mip);
		    float4 Ispecular = texCUBE(_Refection,reflection);   //间接高光
		//    float2  ViewUV = float2(viewdir.xy*0.5+0.5);
		//	float4 Ispecular = tex2D(_Refection,ViewUV);

			float4 Fspecular =float4( Dspecular + Ispecular.rgb*0.8f,1);
			Fspecular.rgb = clamp(Fspecular.rgb,float3(0,0,0),float3(1,1,1));

			float4 MASK = tex2D(_Mask,i.uv);
			float pbrMask = MASK.r;
			float2 EmissUv = i.uv ;
			 EmissUv.y -=  _Time.x*_EmissSpeed;
			float4 EmissMask = tex2D(_Mask,EmissUv);
			//emission 
			float4 emiss = lerp( _Emission,_EmissionTwo,(sin(_Time.y)*0.5+0.5)*clamp(i.uv.y*2,0,1));
			col = lerp(col,emiss,MASK.g*EmissMask.b);

		  float4 FinalSpe = lerp(SpeColor,Fspecular,pbrMask);
		  float4 Finaldiffuse = lerp(col*diffcolor,(1-_Metallic+0.3)*col,pbrMask);

                col = Finaldiffuse + FinalSpe;

                return col;
            }
           ENDHLSL
        }

		 Pass
        {
		   
		   Name "OutLine"
		//  Tags{"LightMode" = "LightweightForward"}
		   Cull Front  ZWrite On 
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"


            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;

            };

            struct v2f
            {

                float4 vertex : SV_POSITION;
            };

			float _OutHeight;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex =TransformObjectToHClip(v.vertex.xyz);
				float3 worldnormal = normalize(TransformObjectToWorldNormal(v.normal));
				float3 viewnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);
				float3 ndcNormal = normalize(TransformWViewToHClip(viewnormal.xyz)) * o.vertex.w;
				 o.vertex.xy += ndcNormal.xy *_OutHeight;
                return o;
            }
		    float4 _OutLineColor ;
            half4 frag (v2f i) : SV_Target
            {
                return half4(0, 1, 0, 1);
            }
           ENDHLSL
		 }
    }
}
