using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine.Rendering;

namespace MileCode {
    [EditorTool("Light EnvV2", typeof(MeshInfo))]
    public class LightEnvToolV2 : EditorTool {

        LightEnv lightEnv;

        GUIContent m_ToolbarIcon;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/LightBulb_icon.png"),
                            "Light EnvV2"
                            );
                }
                return m_ToolbarIcon;
            }
        }

        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                LightEnv.RestoreSavedLightEnv();
                this.lightEnv.RemoveEnvironment();
                return;
            }
            LightEnv.TurnOffSavedLightEnv();
            this.lightEnv.CreateEnvironment();
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += this.ActiveToolDidChange;
            //Debug.Log("Save Current LightEnv");
            //Debug.Log("Get Default LightEnv");
            this.lightEnv = ScriptableObject.CreateInstance<LightEnv>();
            LightEnv.SaveCurrentLightEnv();
            
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= this.ActiveToolDidChange;
            //Debug.Log("Disabled.");
        }

        public override void OnToolGUI(EditorWindow window) {
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(Screen.width - 229, Screen.height - 450, 217, 340), boxStyle);
                {
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();        //
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("------------ Light Env ------------");        // this is firstline
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();          //

                    GUILayout.Label("Light Intensity: " + this.lightEnv.tempLightIntensity) ;
                    this.lightEnv.tempLightIntensity = GUILayout.HorizontalSlider(this.lightEnv.tempLightIntensity, 0, 10);
                    this.lightEnv.SetLightEnvIntensity(this.lightEnv.tempLightIntensity);
                    GUILayout.Space(15);
                    GUILayout.Label("Light Number: " + this.lightEnv.tempLightOnCount);
                    this.lightEnv.tempLightOnCount = (int)GUILayout.HorizontalSlider(this.lightEnv.tempLightOnCount, 1, 3);
                    this.lightEnv.SetLightEnvOnNumber(this.lightEnv.tempLightOnCount - 1);
                    GUILayout.Space(15);
                    GUILayout.Label("Light Angle: " + this.lightEnv.tempLightAngle);
                    this.lightEnv.tempLightAngle = GUILayout.HorizontalSlider(this.lightEnv.tempLightAngle, 0, 360);
                    this.lightEnv.SetLightEnvAngle(this.lightEnv.tempLightAngle);
                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Spherical Harmonics");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if(this.lightEnv.environmentLightingDone) {
                        if(RenderSettings.ambientMode == AmbientMode.Skybox) {
                            GUILayout.Label("Ambient Mode: " + RenderSettings.ambientMode.ToString());
                            GUILayout.Label("Skybox Name: " + RenderSettings.skybox.name);
                            GUILayout.Label("Ambient Intensity: " + RenderSettings.ambientIntensity);
                            RenderSettings.ambientIntensity = GUILayout.HorizontalSlider(RenderSettings.ambientIntensity, 0, 10);
                        }
                        if(RenderSettings.ambientMode == AmbientMode.Flat) {
                            GUILayout.Label("Ambient Mode: " + RenderSettings.ambientMode.ToString());
                            EditorGUILayout.ColorField("Baked Color: ", RenderSettings.ambientLight);
                        }
                        if(RenderSettings.ambientMode == AmbientMode.Trilight) {
                            GUILayout.Label("Ambient Mode: " + RenderSettings.ambientMode.ToString());
                            EditorGUILayout.ColorField("Sky Color: ", RenderSettings.ambientSkyColor);
                            EditorGUILayout.ColorField("Equator Color: ", RenderSettings.ambientEquatorColor);
                            EditorGUILayout.ColorField("Ground Color: ", RenderSettings.ambientGroundColor);
                        }
                    } else {
                        if(GUILayout.Button("Bake SH")) {
                            this.lightEnv.GetEnvironmentLightingDone();
                        }
                    }

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Reflection Probe: ");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}