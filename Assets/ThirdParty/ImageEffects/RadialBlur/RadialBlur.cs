using UnityEngine;
using SLua;

[CustomLuaClassAttribute]
[ExecuteInEditMode]
public class RadialBlur : ImageEffectBase
{
    //[HideInInspector]
    public int iteration = 1;
    //[HideInInspector]
    public float blurStrength = 2.2f;
    //[HideInInspector]
    public float blurWidth = 1.0f;
    //[HideInInspector]
    public Vector2 center = new Vector2(0.5f, 0.5f);

    void Start()
    {
        if (shader == null)
        {
            Debug.LogError("shader missing!", this);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {       
        material.SetFloat("_BlurStrength", blurStrength);
        material.SetFloat("_BlurWidth", blurWidth);
        material.SetVector("_Center", center);
        //render target 和render texture 为同一个的时候，在一些手机上会出现屏幕撕裂
        // for(int i = 0; i < iteration - 1; i++)
        // {
        //     Graphics.Blit(source, source, material);
        // }
        Graphics.Blit(source, dest, material);
    }
}