using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine.Rendering;

namespace MileCode {
    [EditorTool("Mesh InfoV2", typeof(MeshRenderer))]
    public class MeshInfoToolV2 : EditorTool {
        MeshReader meshReader;

        GUIContent m_ToolbarIcon;
        public override GUIContent toolbarIcon {
            get {
                if(m_ToolbarIcon == null) {
                    m_ToolbarIcon = new GUIContent(
                            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/m_icon.png"),
                            "Mesh InfoV2"
                        );
                }
                return m_ToolbarIcon;

            }
        }


        void ActiveToolDidChange() {
            if(!EditorTools.IsActiveTool(this)) {
                Debug.Log("Not Using MeshToolV2");
                this.meshReader.ResetMeshTransform();
                return;
            }
            Debug.Log("Using MeshToolV2");
            this.meshReader = new MeshReader((MeshRenderer)target);
        }

        private float GetRectHPosition(float height) {
            return 240 + 20 * height + 10;
        }

        private void OnEnable() {
            EditorTools.activeToolChanged += this.ActiveToolDidChange;  
            //Debug.Log("MeshToolV2 enabled");
        }

        private void OnDisable() {
            EditorTools.activeToolChanged -= this.ActiveToolDidChange;
            //Debug.Log("MeshToolV2 disabled");
        }
        // temp
        int texturesCount = 3;
        public override void OnToolGUI(EditorWindow window) {

            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(10, Screen.height - 50 - GetRectHPosition(this.texturesCount), 300, GetRectHPosition(this.texturesCount)), boxStyle);
                {
                    GUILayout.Label("--------------- Mesh Information ----------------");
                    GUILayout.Label("Mesh Name: " + this.meshReader.GetMeshName());
                    GUILayout.Label("Vertices: " + this.meshReader.GetMeshVerticesCount() + " | Triangles: " + this.meshReader.GetMeshTrianglesCount() + " | SubMesh: " + this.meshReader.GetSubMeshCount());
                    GUILayout.Label("Normals: " + this.meshReader.HasNormal() + " | Color: " + this.meshReader.HasVertexColor() + " | UV2: " + this.meshReader.HasUV2());
                    GUILayout.Label("Turntable Speed: " + this.meshReader.turnTableSpeed);
                    this.meshReader.turnTableSpeed = GUILayout.HorizontalSlider(this.meshReader.turnTableSpeed, 0, 10);
                    this.meshReader.RotateMesh(this.meshReader.turnTableSpeed);
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }

    }
}