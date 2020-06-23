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
                //Debug.Log("Not my meshTool.");
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
            if(mr  != null) {
                MeshInfo mi = mr.GetComponent<MeshInfo>();
                if(mi != null) {
                    mi.ResetMesh();
                    DestroyImmediate(mi);
                }
            }
            
        }
        
        private void OnEnable() {
            EditorTools.activeToolChanged += ActiveToolDidChange;
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= ActiveToolDidChange;
            //Debug.Log("MeshInfoTool Disable");
            this.RemoveMeshInfo();
        }
        
        public override void OnToolGUI(EditorWindow window) {

        }

    }
}
