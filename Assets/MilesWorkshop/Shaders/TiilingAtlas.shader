// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SoFunny/WorldShader"
{
   Properties
   {
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _AtlasCount ("Number of sub textures in the atlas", float) = 1
   }
 
   SubShader
   {
     Lighting Off
   
     pass
     {
       CGPROGRAM
       #pragma vertex vShader
       #pragma fragment pShader
       #include "UnityCG.cginc"
 
       sampler2D _MainTex;
       float _AtlasCount;
     
 
       struct VertIn
       {
         float4 vertex : POSITION;
         float4 color : COLOR;
         float2 texcoord : TEXCOORD0;
       };
 
       struct VertOut
       {
         float4 position : POSITION;
         float4 color : COLOR;
         float2 texcoord : TEXCOORD0;
       };
 
       VertOut vShader(VertIn input)
       {
         VertOut output;
         output.position = UnityObjectToClipPos(input.vertex);
         output.color = input.color;
         output.texcoord = input.texcoord;
         return output;
       }
 
       float4 pShader(VertOut input) : COLOR0
       {
         half _tileSize = 1 / _AtlasCount;
         float2 newUVs = float2((fmod(input.texcoord.x/input.color.b,_tileSize)+_tileSize*input.color.r),fmod(input.texcoord.y/input.color.b,_tileSize)+_tileSize*input.color.g);
         return input.color;
         return tex2D(_MainTex, newUVs);
       }
       ENDCG
     }
   }
}