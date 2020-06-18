using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.VersionControl;

namespace MileCode {
    [EditorTool("Light Env", typeof(MeshInfo))]
    public class LightEnvTool : EditorTool {
        GUIContent m_ToolbarIcon;
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
                this.ResetLight();
                return;
            }
            //Debug.Log("LightEnv DidChange");
            this.BuildLight();
        }

        Light[] lights;
        private void BuildLight() {
            Light[] currentLights = Light.GetLights(LightType.Directional, 0);
            if(currentLights.Length >= 1) {
                this.lights = currentLights;
                //Debug.Log(this.lights.Length);
                foreach(Light light in currentLights) {
                    light.enabled = false;
                }
            }
            
            GameObject LightEnv = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetViewer/Prefabs/LightEnv.prefab");
            
            LightEnv.name = "Temp Light";
            PrefabUtility.InstantiatePrefab(LightEnv);
            //Instantiate(LightEnv);
            //Debug.Log("Light is built.");
        }

        private void ResetLight() {
            GameObject LightEnv = GameObject.Find("Temp Light");
            if(LightEnv != null) {
                GameObject.DestroyImmediate(LightEnv);
            }
            if(lights != null) {
                if(lights.Length >= 1) {
                    //Debug.Log("CurrentLight" + lights.Length);
                    foreach(Light light in this.lights) {
                        light.enabled = true;
                    }
                }
            }
            
        }

        // called when toolIcon shows
        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;
            //Debug.Log("Enable");
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= ActiveToolDidChange;
            //Debug.Log("LightEnv Disable");
            //this.ResetLight();
        }

        

        public override void OnToolGUI(EditorWindow window) {
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(Screen.width - 229, Screen.height - 400, 215, 290), boxStyle);
                {
                   
                    GUILayout.Label("Light Condition: " + Screen.height);
         

                    GUILayout.HorizontalSlider(20, 10, 100);
                    
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
