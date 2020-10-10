Shader "Custom/JustDepth"
{
    SubShader
    {
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
           ColorMask 0
        }
    }
}
