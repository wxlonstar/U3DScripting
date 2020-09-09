using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[DisallowMultipleComponent]
public class MakeATexture : MonoBehaviour {
    //public RenderTexture m_InputTexture;
    void SaveRenderTexture() {

        int width = 2;
        int height = 2;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);

        texture.SetPixel(0, 0, Color.red);
        texture.SetPixel(0, 1, Color.green);
        texture.SetPixel(1, 0, Color.blue);
        texture.SetPixel(1, 0, Color.white);
        
        texture.Apply();

        byte[] bytes = texture.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        //byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Test.exr", bytes);
        Debug.Log(Application.dataPath);
        Object.DestroyImmediate(texture);

    }

    private void Start() {
        Debug.Log("....");
        SaveRenderTexture();
    }
}
