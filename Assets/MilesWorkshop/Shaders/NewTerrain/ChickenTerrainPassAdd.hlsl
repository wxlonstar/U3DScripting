#ifndef CHICKEN_TERRAIN_PASS_ADD_INCLUDED
#define CHICKEN_TERRAIN_PASS_ADD_INCLUDED

#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"

struct a2v {
	float4 positionOS : POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f {
	float4 positionCS : SV_POSITION;
	float2 uv : TEXCOORD0;
	float4 uvL5L6 : TEXCOORD3;
	float4 uvL7L8 : TEXCOORD4;
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
	o.uvL5L6.xy = TRANSFORM_TEX(v.uv, _TexLayer05);
	o.uvL5L6.zw = TRANSFORM_TEX(v.uv, _TexLayer06);
	o.uvL7L8.xy = TRANSFORM_TEX(v.uv, _TexLayer07);
	o.uvL7L8.zw = TRANSFORM_TEX(v.uv, _TexLayer08);
	o.positionCS = vpi.positionCS;
	return o;
}

half4 frag(v2f i) : SV_TARGET {
	half4 controlColor02 = SAMPLE_TEXTURE2D(_Control02, sampler_Control02, i.uv);
	half4 controlValue02 = GetControlValue(_Weight, controlColor02);

	half4 albedo01 = SAMPLE_TEXTURE2D(_TexLayer05, sampler_TexLayer05, i.uvL5L6.xy);
	half4 albedo02 = SAMPLE_TEXTURE2D(_TexLayer06, sampler_TexLayer05, i.uvL5L6.zw);
	half4 albedo03 = SAMPLE_TEXTURE2D(_TexLayer07, sampler_TexLayer07, i.uvL7L8.xy);
	half4 albedo04 = SAMPLE_TEXTURE2D(_TexLayer08, sampler_TexLayer08, i.uvL7L8.zw);

	half4 finalAlbedo = GetMixedAlbedoAndSpecular(controlValue02, albedo01, albedo02, albedo03, albedo04);
	
	return finalAlbedo;
}


#endif