using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LightmapAddEmission {
    [MenuItem("Lightmap/LightmapAddEmission")]
    public static void AddEmission() {
        LightmapData[] currentLightmapdatas = LightmapSettings.lightmaps;
        if(currentLightmapdatas.Length >= 1) {
            for(int i = 0; i < currentLightmapdatas.Length; i++) {
                //Debug.Log(i);
                ReDefineLightmap(currentLightmapdatas[i].lightmapColor, i);
            }
        }
    }

    static void ReadAndSaveLightmap(Texture2D lightmap, int index) {
        Texture2D newLightmap = new Texture2D(lightmap.width, lightmap.height, TextureFormat.RGBAFloat, false);
        //Texture2D newLightmap = new Texture2D(lightmap.width, lightmap.height, TextureFormat.RGBA32, false);

        Color[] lightmapColors = lightmap.GetPixels();

        // linearColor
        Color[] lightmapColorLinear = new Color[lightmapColors.Length];
        for(int i = 0; i < lightmapColors.Length; i++) {
            lightmapColorLinear[i] = lightmapColors[i].linear;
        }
        //newLightmap.SetPixels(lightmapColorLinear);


        newLightmap.SetPixels(lightmapColors);       
        newLightmap.Apply();

        byte[] bytes = newLightmap.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        //byte[] bytes = newLightmap.EncodeToTGA();
        string path = GeneratePathForNewLightmap(lightmap.name, index);
        File.WriteAllBytes(path, bytes);
        Object.DestroyImmediate(newLightmap);
    }

    static string GeneratePathForNewLightmap(string lightmapName, int index) {
        // exr
        string path1 = Application.dataPath + "/" + lightmapName + "_" + index + ".exr";
        // tga
        string path2 = Application.dataPath + "/" + lightmapName + "_" + index + ".tga";
        //Debug.Log(path);
        string path3 = Application.dataPath + "/" + lightmapName + "_" + index + ".png";
        //Debug.Log(path);
        return path1;
    }


    // lightmap compress must be disable
    static void ReDefineLightmap(Texture2D lightmap, int index) {
        lightmap.SetPixel(0, 0, Color.green);
        lightmap.Apply();
        byte[] bytes = lightmap.EncodeToPNG();
       
        string path = GeneratePathForNewLightmap(lightmap.name, index);
        File.WriteAllBytes(path, bytes);
        //Object.DestroyImmediate(lightmap);
    }


}
