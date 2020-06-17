using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.EditorTools;

namespace MileCode {
    [EditorTool("Light Group", typeof(MeshInfo))]
    public class LightGroup : EditorTool {
        GUIContent m_ToolbarIcon;
        GUIContent Slider;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(
                            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/Light-Bulb-icon.png"),
                            "Light Group"
                        );
                }
                return m_ToolbarIcon;

            }
        }

        GameObject lightGroup;
    
        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                return;
            }
            /*
            if(lightGroup == null) {
                lightGroup = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetViewer/Prefabs/LightGroup.prefab");
                Instantiate(lightGroup);
            }
            Debug.Log("Changed");
            */
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;

        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= ActiveToolDidChange;
        }

        public override void OnToolGUI(EditorWindow window) {
            EditorGUI.BeginChangeCheck();

            Vector3 position = Tools.handlePosition;

            using(new Handles.DrawingScope(Color.green)) {
                position = Handles.Slider(position, Vector3.right);
                //Handles.DrawSolidArc(position + new Vector3(10, 10, 10), Vector3.up, Vector3.down, 90, 10);
                Handles.Label(position, "make.................");
            }

            if(EditorGUI.EndChangeCheck()) {
                Debug.Log("End Check");
                Vector3 delta = position - Tools.handlePosition;

                Undo.RecordObjects(Selection.transforms, "Move Platform");

                foreach(var transform in Selection.transforms) {
                    transform.position += delta;
                }
                    
            }
        }

    }
}
