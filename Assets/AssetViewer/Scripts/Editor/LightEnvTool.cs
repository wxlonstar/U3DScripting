using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Rendering;

namespace MileCode {
    //[EditorTool("Light Env", typeof(MeshInfo))]
    public class LightEnvTool : EditorTool {
        GUIContent m_ToolbarIcon;

        Light[] sceneLights;
        List<GameObject> lightsObj;
        GameObject lightEnv;
        List<Light> viewerLights;
        int viewerLightCount = 3;
        float viewrLightAngle = 0;
        bool bakedGI = false;
        Material skyMaterial;
        float SHIntensiy = 1;
        float defaultIntensity;
        float defaultReflectionIntensity;


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
                Debug.Log("Not my lightTool.");
                this.RemoveLightEnv();
                this.ResetLightingSettings();
                this.TurnOnOffSceneLight(true);
                return;
            }
            //Debug.Log("LightEnv DidChange");
            this.BuildLightEnv();
            
        }

        private void ResetLightingSettings() {
            RenderSettings.ambientIntensity = this.defaultIntensity;
            RenderSettings.reflectionIntensity = this.defaultReflectionIntensity;
        }

        private void InitializeEnvironmentLight() {
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.bakeResolution = 10;
            if(!this.bakedGI || RenderSettings.skybox != this.skyMaterial) {
                this.defaultIntensity = RenderSettings.ambientIntensity;
                //this.bakedGI = Lightmapping.BakeAsync();
                Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
                this.skyMaterial = RenderSettings.skybox;
                //Lightmapping.lightingDataUpdated += this.SetBakedGI;
                Lightmapping.bakeCompleted += this.SetBakedGI;
            }

        }

        IEnumerator JustWaitAndRun() {
            yield return new EditorWaitForSeconds(1.0f);
        }

        private void SetBakedGI() {
            JustWaitAndRun();
            this.bakedGI = true;
            Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
        }

        private void SaveCurrentReflectionProbeIntensity() {
            this.defaultIntensity = RenderSettings.ambientIntensity;
            this.defaultReflectionIntensity = RenderSettings.reflectionIntensity;
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
                if(this.viewerLights.Count >= 1) {
                    foreach(Light light in this.viewerLights) {
                        if(light.gameObject.name == "light_0") {
                            light.intensity = lightIntensity;
                            light.shadows = LightShadows.Soft;
                        } else {
                            light.intensity = lightIntensity * 0.5f;
                            light.shadows = LightShadows.None;
                        }
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

        private void InitializeViewerLight() {
            this.viewerLights = new List<Light>();
        }

        private void SaveCurrentSceneLights() {
            this.SaveCurrentReflectionProbeIntensity();
            this.sceneLights = Light.GetLights(LightType.Directional, 0);
            if(this.sceneLights == null) {
                Debug.Log("sceneLights is not initialized.");
            }
        }

/*        private void SaveCurrentSceneLightsObj() {
            this.lightsObj = new List<GameObject>();
            Light[] lights = Light.GetLights(LightType.Directional, 0);
            Debug.Log(lights.Length);
        }*/

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
                        foreach(Light light in this.sceneLights) {
                            light.enabled = false;
                        }
                    }
                   
                }
            } else if(onOff == true) {
                if(this.sceneLights != null) {
                    //Debug.Log(this.sceneLights.Length);
                   
                    if(this.sceneLights.Length >= 1) {
                        
                        foreach(Light light in this.sceneLights) {
                            light.enabled = true;
                        }
                        
                    }
                    
                }

            }

        }

        // called when toolIcon shows
        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;
            this.SaveCurrentSceneLights();
            this.InitializeViewerLight();
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
                GUILayout.BeginArea(new Rect(Screen.width - 229, Screen.height - 450, 217, 340), boxStyle);
                {
                   
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Light Condition: ");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
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
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Spherical Harmonics: ");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    if(this.bakedGI) {
                        if(this.skyMaterial != null) {
                            if(RenderSettings.skybox != this.skyMaterial) {
                                this.InitializeEnvironmentLight();
                            }
                            GUILayout.Label("SH Source: " + RenderSettings.ambientMode.ToString());
                            GUILayout.Label("Skybox Intensity: " + this.SHIntensiy);
                            this.SHIntensiy = GUILayout.HorizontalSlider(this.SHIntensiy, 0, 10);
                            this.AdjustSHIntensity(this.SHIntensiy);
                        }
                    } else {
                        
                        if(GUILayout.Button("Bake SH")) {
                            this.InitializeEnvironmentLight();
                        }
                        
                    }

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Reflection Probe: ");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if(this.bakedGI) {
                        GUILayout.Label("Reflection Mode: " + RenderSettings.defaultReflectionMode.ToString());
                        if(RenderSettings.defaultReflectionMode == DefaultReflectionMode.Skybox) {
                            string skyboxName = null;
                            if(RenderSettings.skybox != null) {
                                skyboxName = RenderSettings.skybox.name;
                            }
                            GUILayout.Label("Skybox Name: " + skyboxName);
                        }
                        if(RenderSettings.defaultReflectionMode == DefaultReflectionMode.Custom) {
                            string cubemapName = "null";
                            if(RenderSettings.customReflection != null) {
                                cubemapName = RenderSettings.customReflection.name;
                            }
                            GUILayout.Label("Cubemap Name: " + cubemapName);
                        }
                        GUILayout.Label("Reflection Intensity: " + RenderSettings.reflectionIntensity);

                        RenderSettings.reflectionIntensity = GUILayout.HorizontalSlider(RenderSettings.reflectionIntensity, 0, 1);
                        GUILayout.Space(15);
                        if(RenderSettings.defaultReflectionMode == DefaultReflectionMode.Skybox) {
                            if(GUILayout.Button("Use Custom")) {
                                RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
                            }
                        }
                        if(RenderSettings.defaultReflectionMode == DefaultReflectionMode.Custom) {
                            //GUILayout.HorizontalScrollbar(0, 2, 0, 10);
                            if(GUILayout.Button("Use Skybox")) {
                                RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
                            }
                        }
                        
                    }

                    //GUILayout.HorizontalScrollbar(20, 100, 0, 300);
 

                    GUILayout.EndVertical();   
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
