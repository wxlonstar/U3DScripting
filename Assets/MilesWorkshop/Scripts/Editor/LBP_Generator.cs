using UnityEngine;
using UnityEditor;
using System.Collections;
using Boo.Lang;
using System;
using AmplifyShaderEditor;
using MileCode;

public class LBP_Generator : EditorWindow {
    static string LBPFolder = "Assets/MilesWorkshop/Prefabs/LBP/";

    [MenuItem("LBP/Generate")]
    public static void GenerateLBP() {
        FetchGameObjects();
    }

    static void FetchGameObjects() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("LBP");
        if(gameObjects.Length <= 0) {
            Debug.Log("Can't find gameObjects with the tag(LBP)");
        } else {
            Debug.LogWarning(gameObjects.Length + " LBP Tags.");
        }

        FindPotentialLBP(gameObjects);
    }

    static void FindPotentialLBP(GameObject[] gameObjects) {
        foreach(GameObject go in gameObjects) {
            if(go.isStatic) {
                Debug.Log(go.name + " is static. ");
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                UseLBPSettings(meshRenderers);
                CheckGo(go);
            }
        }
    }

    static void UseLBPSettings(MeshRenderer[] meshRenders) {
        foreach(MeshRenderer mr in meshRenders) {
            if(mr.lightmapIndex >= 0) {
                LBP_Settings lbpSettings = mr.gameObject.AddComponent<LBP_Settings>();
                lbpSettings.lightmapScaleAndOffset = mr.lightmapScaleOffset;
                lbpSettings.lightmapIndex = mr.lightmapIndex;
                //LightmapSettings.lightmaps.
            }
        }
    }

    static void CheckGo(GameObject go) {
        string lbpPath = LBPFolder + go.name + ".prefab";
        //PrefabUtility.CreatePrefab(lbpPath, go);
        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, lbpPath, UnityEditor.InteractionMode.AutomatedAction);
    }
}
