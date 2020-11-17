#ifndef CHICKEN_TERRAIN_PASS_INCLUDED
#define CHICKEN_TERRAIN_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"

struct a2v {
	float4 positionOS : POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f {
	float4 positionCS : SV_POSITION;
	float2 uv : TEXCOORD0;
	/*
	float4 uvL1L2 : TEXCOORD1;
	float4 uvL3L4 : TEXCOORD2;
	*/
};

// get control value which indicates how r g b a color mixes
half4 GetControlValue(half weight, half4 controlColor) {
	half4 controlValue;
	controlValue = controlColor;
	half4 maxControl = controlValue - (max(controlValue.r, max(controlValue.g, max(controlValue.b, controlValue.a))));
	half4 withWeight = max(maxControl + weight, half4(0, 0, 0, 0)) * controlColor;
	half4 finalValue = withWeight / (withWeight.r + withWeight.g + withWeight.b + withWeight.a);
	return finalValue;
}


half4 GetMixedAlbedoAndSpecular(half4 controlValue, half4 albedo01, half4 albedo02, half4 albedo03, half4 albedo04) {
	albedo01 = albedo01 * controlValue.r;
	albedo02 = albedo02 * controlValue.g;
	albedo03 = albedo03 * controlValue.b;
	albedo04 = albedo04 * controlValue.a;
	return albedo01 + albedo02 + albedo03 + albedo04;
}

v2f vert(a2v v) {
	v2f o = (v2f)0;
	VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
	o.uv = v.uv;
	/*
	o.uvL1L2.xy = TRANSFORM_TEX(v.uv, _TexLayer01);
	o.uvL1L2.zw = TRANSFORM_TEX(v.uv, _TexLayer02);
	o.uvL3L4.xy = TRANSFORM_TEX(v.uv, _TexLayer03);
	o.uvL3L4.zw = TRANSFORM_TEX(v.uv, _TexLayer04);
	*/
	o.positionCS = vpi.positionCS;
	return o;
}

half4 frag(v2f i) : SV_TARGET {
	half4 controlColor01 = SAMPLE_TEXTURE2D(_Control01, sampler_Control01, i.uv);
	half4 controlValue01 = GetControlValue(_Weight, controlColor01);

	half4 controlColor02 = SAMPLE_TEXTURE2D(_Control02, sampler_Control02, i.uv);
	half4 controlValue02 = GetControlValue(_Weight, controlColor02);
	/*
	half4 albedo01 = SAMPLE_TEXTURE2D(_TexLayer01, sampler_TexLayer01, i.uvL1L2.xy);
	half4 albedo02 = SAMPLE_TEXTURE2D(_TexLayer02, sampler_TexLayer02, i.uvL1L2.zw);
	half4 albedo03 = SAMPLE_TEXTURE2D(_TexLayer03, sampler_TexLayer03, i.uvL3L4.xy);
	half4 albedo04 = SAMPLE_TEXTURE2D(_TexLayer04, sampler_TexLayer04, i.uvL3L4.zw);
	*/
	half4 finalAlbedo01 = GetMixedAlbedoAndSpecular(controlColor01, _Color01, _Color02, _Color03, _Color04);
	half4 finalAlbedo02 = GetMixedAlbedoAndSpecular(controlColor02, _Color05, _Color06, _Color07, _Color08);
	//half4 controlColor02 = SAMPLE_TEXTURE2D(_Control02, sampler_Control02, i.uv);
	return finalAlbedo01 + finalAlbedo02;
	//return half4(1, 1, 1, 1) - 0.5;
	//return finalAlbedo01;
}


#endif