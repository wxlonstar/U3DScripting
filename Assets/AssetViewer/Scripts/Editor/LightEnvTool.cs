using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.VersionControl;
using NUnit.Framework.Constraints;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MileCode {
    [EditorTool("Light Env", typeof(MeshInfo))]
    public class LightEnvTool : EditorTool {
        Light[] sceneLights;
        GameObject lightEnv;
        GUIContent m_ToolbarIcon;
        List<Light> viewerLights;
        int viewerLightCount = 3;
        float viewrLightAngle = 0;
        bool bakedGI = false;
        Material skyMaterial;
        float SHIntensiy = 1;
        float defaultIntensity;
        Color lightColor;

        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/LightBulb_icon.png"),
                            "Light Env"
                            );
                }
                return m_ToolbarIcon;
            }
        }

        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                //Debug.Log("Not my lightTool.");
                this.RemoveLightEnv();
                this.TurnOnOffSceneLight(true);
                this.ResetLightingSettings();
                return;
            }
            //Debug.Log("LightEnv DidChange");
            this.SaveCurrentSceneLights();
            this.InitializeViewerLight();
            this.InitializeEnvironmentLight();
            this.BuildLightEnv();
            
        }

        private void ResetLightingSettings() {
            RenderSettings.ambientIntensity = this.defaultIntensity;
        }

        private void InitializeEnvironmentLight() {
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.bakeResolution = 10;
            if(!this.bakedGI || RenderSettings.skybox != this.skyMaterial) {
                this.defaultIntensity = RenderSettings.ambientIntensity;
                this.bakedGI = Lightmapping.BakeAsync();
                this.skyMaterial = RenderSettings.skybox;
            } 
            
        }

        private void AdjustSHIntensity(float intensity) {
            RenderSettings.ambientIntensity = intensity;
        }

        private void RemoveLightEnv() {
            if(this.lightEnv != null) {
                GameObject.DestroyImmediate(this.lightEnv);
            }
            
        }
        private void BuildLightEnv() {
            this.TurnOnOffSceneLight(false);
            this.lightEnv = new GameObject("Temp Light");
            this.addLights(this.lightEnv, 3);
        }

        private float lightIntensity = 1.0f;
        private void addLights(GameObject lightEnv, int num) {
            Vector3[] lightsAngle = new Vector3[] {
                new Vector3(40, 140, 0),
                new Vector3(50, -40, 0),
                new Vector3(-50, 0, 150)
            };
            for(int i = 0; i < num; i++) {
                GameObject lightObj = new GameObject("light_" + i);
                Light light = lightObj.AddComponent<Light>();
                if(i == 0) {
                    light.intensity = this.lightIntensity;
                    light.shadows = LightShadows.Soft;
                } else {
                    light.intensity = this.lightIntensity * 0.5f;
                    light.shadows = LightShadows.None;
                }
                light.type = LightType.Directional;      
                light.transform.SetParent(lightEnv.transform);
                lightObj.transform.eulerAngles = lightsAngle[i];
                this.viewerLights.Add(light);
            }
        }


        private void AdjustLightIntensity(float lightIntensity) {
            if(this.viewerLights != null) {
                foreach(Light light in this.viewerLights) {
                    if(light.gameObject.name == "light_0") {
                        light.intensity = lightIntensity;
                    } else {
                        light.intensity = lightIntensity * 0.5f;
                    }
                }
            }      
        }

        private void AdjustLightCount(int lightCount) {
            switch(lightCount) {
                case 0:
                    this.viewerLights[0].enabled = true;
                    this.viewerLights[1].enabled = false;
                    this.viewerLights[2].enabled = false;
                    break;
                case 1:
                    this.viewerLights[0].enabled = true;
                    this.viewerLights[1].enabled = true;
                    this.viewerLights[2].enabled = false;
                    break;
                case 2:
                    this.viewerLights[0].enabled = true;
                    this.viewerLights[1].enabled = true;
                    this.viewerLights[2].enabled = true;
                    break;
                default:
                    break;
            }

        }

        private void addProbe() { 
        }

        private void InitializeViewerLight() {
            this.viewerLights = new List<Light>();
        }

        private void SaveCurrentSceneLights() {
            this.sceneLights = Light.GetLights(LightType.Directional, 0);
            if(this.sceneLights == null) {
                Debug.Log("sceneLights is not initialized.");
            }
        }

        private void AdjustLightAngle(float lightAngle) {
            if(this.lightEnv != null) {
                this.lightEnv.transform.localEulerAngles = new Vector3(0, lightAngle, 0);
            }
        }

        private void TurnOnOffSceneLight(bool onOff) {
            // if false, all scene light off
            if(onOff == false) {
                if(this.sceneLights != null) {
                    if(this.sceneLights.Length >= 1) {
                        foreach(Light light in sceneLights) {
                            light.enabled = false;
                        }
                    }
                }
            } else if(onOff == true) {
                if(this.sceneLights != null) {
                    if(this.sceneLights.Length >= 1) {
                        foreach(Light light in sceneLights) {
                            light.enabled = true;
                        }
                    }
                }

            }

        }


        // called when toolIcon shows
        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;
            //Debug.Log("LightEnv Enable");
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= ActiveToolDidChange;
            //Debug.Log("LightEnv Disable");

        }

        

        public override void OnToolGUI(EditorWindow window) {
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(Screen.width - 229, Screen.height - 400, 217, 290), boxStyle);
                {
                   
                    GUILayout.BeginVertical();
                    /*
                    if(GUILayout.Button("On / Off")) {
                        if(lightEnv.activeSelf) {
                            lightEnv.SetActive(false);
                        } else {
                            lightEnv.SetActive(true);
                        }
                    }
                    */
                    
                    GUILayout.Label("Light Intensity: " + this.lightIntensity);
                    this.lightIntensity = GUILayout.HorizontalSlider(this.lightIntensity, 0, 10);
                    this.AdjustLightIntensity(this.lightIntensity);
                    GUILayout.Space(15);
                    GUILayout.Label("Light Number: " + this.viewerLightCount);
                    this.viewerLightCount = (int)GUILayout.HorizontalSlider(this.viewerLightCount, 1, 3);
                    this.AdjustLightCount(this.viewerLightCount - 1);
                    GUILayout.Space(15);
                    GUILayout.Label("Light Angle: " + this.viewrLightAngle);
                    this.viewrLightAngle = GUILayout.HorizontalSlider(this.viewrLightAngle, 0, 360);
                    this.AdjustLightAngle(this.viewrLightAngle);
                    GUILayout.Space(15);
                    if(this.skyMaterial != null) {
                        GUILayout.Label("SH Source: " + this.skyMaterial.name);
                        this.SHIntensiy =  GUILayout.HorizontalSlider(this.SHIntensiy, 0, 10);
                        this.AdjustSHIntensity(this.SHIntensiy);
                    }

                    GUILayout.EndVertical();
                   
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
