using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetAllUsedShaders {
    static List<Shader> shadersUsedList = new List<Shader>();
    static List<Shader> shadersUnusedList = new List<Shader>();
    [MenuItem("Assets/GetAllShaders")]
    public static void GetShadersAmongSelectedMaterials() {
        shadersUsedList.Clear();
        Object[] objs = Selection.objects;
        foreach(var obj in objs) {
            if((obj as Material) is Material) {
                FillShaderUsedListWithoutRepeat(shadersUsedList, (obj as Material).shader);
            }
        }
        SaveShaderList(shadersUsedList, "Assets/MileStudio/ShaderUsedList.asset");
        Debug.Log("Shader used: " + shadersUsedList.Count);
    }

    [MenuItem("Assets/UnusedShader")]
    public static void GetUnusedShaders() {
        shadersUnusedList.Clear();
        ShaderListScritableObj shaderUsedList = AssetDatabase.LoadAssetAtPath<ShaderListScritableObj>("Assets/MileStudio/ShaderUsedList.asset");
        if(shaderUsedList == null) {
            Debug.Log("Can't find ShaderList.");
            return;
        }
        Object[] objs = Selection.objects;
        foreach(var obj in objs) {
            if((obj as Shader) is Shader) {
                FillShaderUnusedListWithoutRepeat(shaderUsedList.shaders, shadersUnusedList, obj as Shader);
            }
        }
        SaveShaderList(shadersUnusedList, "Assets/MileStudio/shadersUnusedList.asset");
        Debug.Log("Shader unused: " + shadersUnusedList.Count);
    }

    static void SaveShaderList(List<Shader> input, string path) {
        //AssetDatabase.CreateAsset(new ShaderList(), Application.dataPath + "/MileStudio/shaderList.asset");
        //ShaderList shaderList = AssetDatabase.LoadAssetAtPath<ShaderList>("Assets/MileStudio/ShaderList.asset");
        ShaderListScritableObj shaderList_ScritableObj = new ShaderListScritableObj();
        shaderList_ScritableObj.AddShaders(input);
        //AssetDatabase.CreateAsset(shaderList_ScritableObj, "Assets/MileStudio/ShaderList_ " + System.DateTime.Now.ToString("MMddHHmmss") + ".asset");
        AssetDatabase.CreateAsset(shaderList_ScritableObj, path);
    }

    static void FillShaderUnusedListWithoutRepeat(List<Shader> shaderUsedList, List<Shader> shaderUnusedList, Shader shd) {
        if(!shaderUsedList.Contains(shd)) {
            shaderUnusedList.Add(shd);
        }
    }

    static void FillShaderUsedListWithoutRepeat(List<Shader> shaderUsedList, Shader shd) {
        if(shaderUsedList.Contains(shd)) {
            return;
        } else {
            shaderUsedList.Add(shd);
        }
    }
}
