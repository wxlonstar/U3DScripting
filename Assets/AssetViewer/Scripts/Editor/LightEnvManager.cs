using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.Experimental.TerrainAPI;

namespace MileCode {
    public class LightEnvManager : Editor {
        public static List<LightEnv> lightEnvsFound = new List<LightEnv>();

        [MenuItem("Scene/Export LightEnv")]
        public static void ExportLightEnv() {
            string sceneName = SceneManager.GetActiveScene().name;
            if(sceneName == "") {
                Debug.Log("The scene is not saved.");
                return;
            }
            LightEnv lightEnv = CreateLightEnv(sceneName);
            AssetDatabase.CreateAsset(lightEnv, "Assets/AssetViewer/LightEnv/" + lightEnv.sceneName + "_" + System.DateTime.Now.ToString("MMddHHmmss") + "_lightEnv" + ".asset");
        }


        private static LightEnv CreateLightEnv(string sceneName) {
            LightEnv lightEnv = ScriptableObject.CreateInstance<LightEnv>();
            lightEnv.sceneName = sceneName;
            lightEnv.SaveLightParams();
            lightEnv.SaveEnvironmentLighting();
            lightEnv.SaveReflectionProbe();
            return lightEnv;
        }

        public static void Apply(LightEnv lightEnv) {
            LightEnv.TurnOffSavedLightEnv();
            LightEnvManager.TurnOffAllTempLightsExcept(lightEnv);
            lightEnv.UseLightFromScriptObject();
            lightEnv.UseEnvironmentLightFromScriptObject();
            lightEnv.UseReflectionProbeFromScriptObject();
            lightEnv.GetEnvironmentLightingDone();
        }

        public static void TurnOffAllTempLightsExcept(LightEnv lightEnvInUse) {
            foreach(LightEnv lightEnv in lightEnvsFound) {
                if(lightEnv.name != lightEnvInUse.name) {
                    //Debug.Log(lightEnv.name + "_off");
                    lightEnv.RemoveTempLight();
                } else {
                    //Debug.Log(lightEnv.name + "_on");
                }
            }
        }

        public static void TurnOffAllTempLights() {
            if(lightEnvsFound != null && lightEnvsFound.Count >= 1) {
                foreach(LightEnv lightEnv in lightEnvsFound) {
                    if(lightEnv != null) {
                        lightEnv.RemoveTempLight();
                    }
                }
            }
            
        }

        public static void LoadLightEnv() {
            lightEnvsFound.Clear();
            string lightEnvsFolder = "Assets/AssetViewer/LightEnv";
            bool lightEnvsFolderExists = Directory.Exists(lightEnvsFolder);
            if(lightEnvsFolderExists) {
                string[] lightEnvPaths = Directory.GetFiles(lightEnvsFolder);
                if(lightEnvPaths.Length >= 1) {
                    foreach(string lightEnvPath in lightEnvPaths) {
                        if(lightEnvPath.EndsWith("_lightEnv.asset")) {
                            LightEnv lightEnv = AssetDatabase.LoadAssetAtPath<LightEnv>(lightEnvPath);
                            lightEnvsFound.Add(lightEnv);
                            //Debug.Log(lightEnvPath);
                        }
                    }
                } else {
                    Debug.Log("Can't find any lightEnv");
                }
            } else {
                Debug.Log("Can't find lightEnv folder");
            }
        }

        
    }
}
