using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace MileCode {
    [EditorTool("Scene Env", typeof(MeshRenderer))]
    public class SceneEnvTool : EditorTool {

        GUIContent m_ToolbarIcon;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/tree_Icon.png"),
                            "Scene Env"
                            );
                }
                m_ToolbarIcon.text = "SceneEnv";
                return m_ToolbarIcon;
            }
        }

        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                LightEnv.RestoreSavedLightEnv(this);
                LightEnvManager.TurnOffAllTempLights();
                return;
            }
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += this.ActiveToolDidChange;
            LightEnvManager.LoadLightEnv();
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= this.ActiveToolDidChange;
        }

        Vector2 pos = new Vector2(10, 10);
        public override void OnToolGUI(EditorWindow window) {
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(Screen.width - 229, Screen.height - 450, 217, 340), boxStyle);
                {
                    GUILayout.Label("------------ LightEnv -------------");
                    if(LightEnvManager.lightEnvsFound == null || LightEnvManager.lightEnvsFound.Count <= 0) {
                        GUILayout.Label("LightEnv not found");
                    } else {
                        GUILayout.Label("LightEnv found: " + LightEnvManager.lightEnvsFound.Count);
                        using(var scrollView = new GUILayout.ScrollViewScope(this.pos)) {
                            this.pos = scrollView.scrollPosition;
                            foreach(LightEnv lightEnv in LightEnvManager.lightEnvsFound) {
                                GUILayout.BeginHorizontal();
                                if(GUILayout.Button(lightEnv.GetName())) {
                                    LightEnvManager.Apply(lightEnv);
                                    
                                }
                                if(lightEnv.HasAdditionalProbes()) {
                                    if(GUILayout.Button("Ref", GUILayout.Width(40))) {
                                        lightEnv.UseAdditionalReflectionProbe();
                                    }
                                }
                                
                                GUILayout.EndHorizontal();

                            }
                        }
                    }
                    
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
