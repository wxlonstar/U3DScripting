using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.VersionControl;
using NUnit.Framework.Constraints;

namespace MileCode {
    [EditorTool("Light Env", typeof(MeshInfo))]
    public class LightEnvTool : EditorTool {
        Light[] sceneLights;
        GameObject lightEnv;
        GUIContent m_ToolbarIcon;
        List<Light> viewerLights;
        int viewerLightCount = 3;
        float viewrLightAngle = 0;
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
                return;
            }
            //Debug.Log("LightEnv DidChange");
            this.IntializeSceneLights();
            this.InitializeViewerLight();
            this.BuildLightEnv();
            
        }

        private void RemoveLightEnv() {
            if(lightEnv != null) {
                GameObject.DestroyImmediate(lightEnv);
            }
            
        }
        private void BuildLightEnv() {
            this.TurnOnOffSceneLight(false);
            lightEnv = new GameObject("Temp Light");
            this.addLights(lightEnv, this.viewerLightCount);
        }

        private float lightIntensity = 1;
        private void addLights(GameObject lightEnv, int num) {
            for(int i = 0; i < num; i++) {
                GameObject lightObj = new GameObject("light_" + i);
                Light light = lightObj.AddComponent<Light>();
                light.intensity = this.lightIntensity;
                light.type = LightType.Directional;
                light.shadows = LightShadows.Soft;
                light.transform.SetParent(lightEnv.transform);
                lightObj.transform.localRotation = Quaternion.Euler(new Vector3(50, 60, 0));
                light.transform.Rotate(new Vector3(i * 120, i * 120, i * 120));
                this.viewerLights.Add(light);
            }
        }


        private void AdjustLightIntensity(float lightIntensity) {
            foreach(Light light in this.viewerLights) {
                light.intensity = lightIntensity;
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

        private void IntializeSceneLights() {
            this.sceneLights = Light.GetLights(LightType.Directional, 0);
            if(this.sceneLights == null) {
                Debug.Log("sceneLights is not initialized.");
            }
        }

        private void AdjustLightAngle(float lightAngle) {
            if(this.lightEnv != null) {
                this.lightEnv.transform.localEulerAngles = new Vector3(lightAngle, lightAngle, 0);
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
                   
                    GUILayout.Label("Light Condition: ");
                    GUILayout.BeginVertical();
                    if(GUILayout.Button("On / Off")) {
                        if(lightEnv.activeSelf) {
                            lightEnv.SetActive(false);
                        } else {
                            lightEnv.SetActive(true);
                        }
                    }

                    GUILayout.Label("Light Intensity: " + this.lightIntensity);
                    this.lightIntensity = GUILayout.HorizontalSlider(Mathf.CeilToInt(this.lightIntensity), 0, 10);
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

                    GUILayout.EndVertical();
                   
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
