using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace MileCode {
    [EditorTool("Scene Env", typeof(MeshInfo))]
    public class SceneEnvTool : EditorTool {
        List<SceneEnv> loadedSceneEnvs;

        GUIContent m_ToolbarIcon;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/tree_Icon.png"),
                            "Scene Env"
                            );
                }
                return m_ToolbarIcon;
            }
        }

        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                return;
            }
            
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += this.ActiveToolDidChange;
            //Debug.Log("Load SceneEnv");
            this.LoadSceneEnvs();
        }

        private void LoadSceneEnvs() {
            this.loadedSceneEnvs = new List<SceneEnv>();
            this.loadedSceneEnvs.Add(ScriptableObject.CreateInstance<SceneEnv>());
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
                    GUILayout.Label("-----------------------------------");
                    if(this.loadedSceneEnvs == null || this.loadedSceneEnvs.Count <= 0) {
                        GUILayout.Label("SceneEnv not found");
                    } else {
                        GUILayout.Label("SceneEnv found: " + this.loadedSceneEnvs.Count);
                        using(var scrollView = new GUILayout.ScrollViewScope(this.pos)) {
                            this.pos = scrollView.scrollPosition;
                            foreach(SceneEnv sceneEnv in this.loadedSceneEnvs) {
                                if(GUILayout.Button(sceneEnv.GetName())) {
                                    sceneEnv.ApplySceneEnv();
                                }
                                
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
