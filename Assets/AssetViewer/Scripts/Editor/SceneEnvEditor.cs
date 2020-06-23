using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneEnvEditor : Editor {
    [MenuItem("Scene/Export SceneEnv")]
    public static void ExportSceneEnv() {
        SceneEnv se = ScriptableObject.CreateInstance<SceneEnv>();
        AssetDatabase.CreateAsset(se, "Assets/AssetViewer/SceneEnv/maha.asset");
    }
}
