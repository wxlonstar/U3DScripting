#ifndef CHICKEN_TERRAIN_INPUTS_INCLUDED
#define CHICKEN_TERRAIN_INPUTS_INCLUDED

#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"

TEXTURE2D(_Control01);          SAMPLER(sampler_Control01);
TEXTURE2D(_Control02);          SAMPLER(sampler_Control02);
/*
TEXTURE2D(_TexLayer01);          SAMPLER(sampler_TexLayer01);
TEXTURE2D(_TexLayer02);          SAMPLER(sampler_TexLayer02);
TEXTURE2D(_TexLayer03);          SAMPLER(sampler_TexLayer03);
TEXTURE2D(_TexLayer04);          SAMPLER(sampler_TexLayer04);
TEXTURE2D(_TexLayer05);          SAMPLER(sampler_TexLayer05);
TEXTURE2D(_TexLayer06);          SAMPLER(sampler_TexLayer06);
TEXTURE2D(_TexLayer07);         SAMPLER(sampler_TexLayer07);
TEXTURE2D(_TexLayer08);          SAMPLER(sampler_TexLayer08);
*/

CBUFFER_START(UnityPerMaterial)
half _Weight;
half _Test;
half4 _Color01;
half4 _Color02;
half4 _Color03;
half4 _Color04;
half4 _Color05;
half4 _Color06;
half4 _Color07;
half4 _Color08;
/*
float4 _TexLayer01_ST;
float4 _TexLayer02_ST;
float4 _TexLayer03_ST;
float4 _TexLayer04_ST;
float4 _TexLayer05_ST;
float4 _TexLayer06_ST;
float4 _TexLayer07_ST;
float4 _TexLayer08_ST;
*/
CBUFFER_END

#endif