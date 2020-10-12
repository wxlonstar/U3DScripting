using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetAllUsedShaders {
    static List<Shader> shadersList = new List<Shader>();
    [MenuItem("Assets/GetAllShaders")]
    public static void GetShadersAmongSelectedMaterials() {
        shadersList.Clear();
        Object[] objs = Selection.objects;
        foreach(var obj in objs) {
            if((obj as Material) is Material) {
                FillShaderListWithoutRepeat(obj as Material);
            }
        }
        SaveShaderList(shadersList);
        Debug.Log(shadersList.Count);
    }

    [MenuItem("Assets/UnusedShader")]
    public static void GetUnusedShaders() {
        ShaderListScritableObj shaderList = AssetDatabase.LoadAssetAtPath<ShaderListScritableObj>("Assets/MileStudio/ShaderList.asset");
        Object[] objs = Selection.objects;

    }

    static void SaveShaderList(List<Shader> input) {
        //AssetDatabase.CreateAsset(new ShaderList(), Application.dataPath + "/MileStudio/shaderList.asset");
        //ShaderList shaderList = AssetDatabase.LoadAssetAtPath<ShaderList>("Assets/MileStudio/ShaderList.asset");
        ShaderListScritableObj shaderList_ScritableObj = new ShaderListScritableObj();
        shaderList_ScritableObj.AddShaders(input);

        //AssetDatabase.CreateAsset(shaderList_ScritableObj, "Assets/MileStudio/ShaderList_ " + System.DateTime.Now.ToString("MMddHHmmss") + ".asset");
        AssetDatabase.CreateAsset(shaderList_ScritableObj, "Assets/MileStudio/ShaderList.asset");
        //Debug.Log(shaderList.);

    }

    static void FillShaderListWithoutRepeat(Material mat) {
        if(shadersList.Contains(mat.shader)) {
            return;
        } else {
            shadersList.Add(mat.shader);
        }
    }
}
