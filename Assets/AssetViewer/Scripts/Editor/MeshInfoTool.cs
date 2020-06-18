using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.EditorTools;

namespace MileCode {
    [EditorTool("Mesh Info", typeof(MeshRenderer))]
    public class MeshInfoTool : EditorTool {
        GUIContent m_ToolbarIcon;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(
                            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/m_icon.png"),
                            "Mesh Info"
                        );
                }
                return m_ToolbarIcon;

            }
        }

        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                return;
            }
            this.AddMeshInfo();
            //Debug.Log("MeshInfo DidChange");
        }

        private void AddMeshInfo() {
            MeshRenderer mr = (MeshRenderer)target;
            if(mr.GetComponent<MeshInfo>() == null) {
                mr.gameObject.AddComponent<MeshInfo>();
            }
        }

        private void RemoveMeshInfo() {
            MeshRenderer mr = (MeshRenderer)target;
            MeshInfo mi = mr.GetComponent<MeshInfo>();
            if(mi != null) {
                mi.ResetMesh();
                DestroyImmediate(mi);
            }
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= ActiveToolDidChange;
            Debug.Log("MeshInfoTool Disable");
            this.RemoveMeshInfo();
        }

        public override void OnToolGUI(EditorWindow window) {
            //MeshInfo mi = (MeshInfo)target;
            /*
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(10, 10, 300, 400), boxStyle);
                {
                    GUILayout.Label("Mesh Information");
              

                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
            */
            /*
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
            */
        }

    }
}
