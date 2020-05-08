using UnityEngine;
using UnityEditor;
using System.Collections;
using Boo.Lang;
using System;
using AmplifyShaderEditor;
using MileCode;
using NUnit.Framework.Internal;

namespace MileCode {
    public class LBP_Generator : EditorWindow {
        static string LBPFolder = "Assets/MilesWorkshop/Prefabs/LBP/";
        static string LBPMaterialsFolder = "Assets/MilesWorkshop/Materials/LBP_Materials/";
        static Shader LBPShader;
        [MenuItem("Lightmap/LBP/Generate")]
        public static void GenerateLBP() {
            LBPShader = Shader.Find("MileShader/LightBakedPrefab");
            if(LBPShader == null) {
                Debug.LogError("LBP Shader can't be found!");
                return;
            }
            FetchGameObjects();
        }

        static void Test(string matName) {
            Material mat = new Material(LBPShader);
            mat.name = matName;
            AssetDatabase.CreateAsset(mat, LBPMaterialsFolder + mat.name + ".mat");
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
                    if(CanUseLBPSettings(meshRenderers)) {
                        MakeLBP(go);
                    }
                }
            }
            //Lightmapping.Clear();
        }

        static bool CanUseLBPSettings(MeshRenderer[] meshRenders) {
            bool enableLBP = true;
            foreach(MeshRenderer mr in meshRenders) {
                if(mr.lightmapIndex >= 0) {
                    LBP_Settings lbpSettings = mr.GetComponent<LBP_Settings>();
                    if(lbpSettings == null) {
                        lbpSettings = mr.gameObject.AddComponent<LBP_Settings>();
                    }
                    lbpSettings.lightmapScaleAndOffset = mr.lightmapScaleOffset;
                    lbpSettings.lightmapIndex = mr.lightmapIndex;
                    Texture2D sceneLightmap = FetchCurrentLightmap(mr.lightmapIndex);
                    lbpSettings.lightmap = sceneLightmap;
                    PrepareLBPMaterial(mr);
                } else {
                    enableLBP = false;
                    Debug.LogError(mr.gameObject.name + "(static) doesn't have lightmap enabled, LBP can't be generated.");
                    break;
                }
            }
            return enableLBP;
        }

        static void PrepareLBPMaterial(MeshRenderer mr) {
            foreach(Material mat in mr.sharedMaterials) {
                //Texture mainTexture = mat.GetTexture("_MainTex");
                string LBPMatName = mat.name + "_LBP";
                Material lbpMat = AssetDatabase.LoadAssetAtPath<Material>(LBPMaterialsFolder + LBPMatName + ".mat");
                if(lbpMat == null) {
                    lbpMat = new Material(LBPShader);
                    AssetDatabase.CreateAsset(lbpMat, LBPMaterialsFolder + LBPMatName + ".mat");
                }
                mr.sharedMaterial = lbpMat;
            }
        }

        static Texture2D FetchCurrentLightmap(int lightmapIndex) {
            LightmapData lightmapData = LightmapSettings.lightmaps[lightmapIndex];
            return lightmapData.lightmapColor;
        }

        static void MakeLBP(GameObject go) {
            string lbpPath = LBPFolder + go.name + ".prefab";
            //PrefabUtility.CreatePrefab(lbpPath, go);
            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, lbpPath, UnityEditor.InteractionMode.AutomatedAction);
        }
    }
}
