using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.EditorTools;
using System.Linq;

//[EditorTool("Show Vertices", typeof(MeshFilter))]
public class VertexTool : EditorTool {
    /*
    struct TransformAndPositions {
        public Transform transform;
        public Vector3[] positions;
    }

    IEnumerable<TransformAndPositions> m_Vertices;
    GUIContent m_ToolbarIcon;

    public override GUIContent toolbarIcon {
        get {
            if(m_ToolbarIcon == null) {
                m_ToolbarIcon = new GUIContent(
                        AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ThirdParty/Other/Art/Platformer/Platformer_Player_Dead1.png"),
                        "Vertex Visualization Tool"
                    );
            }
            return m_ToolbarIcon;
            
        }
    }

    void ActiveToolDidChange() {
        if(!EditorTools.IsActiveTool(this)) {
            return;
        }
        m_Vertices = targets.Select(
                x => {
                    return new TransformAndPositions() {
                        transform = ((MeshFilter)x).transform,
                        positions = ((MeshFilter)x).sharedMesh.vertices
                    };
                }
            ).ToArray();
    }

    private void OnEnable() {
        EditorTools.activeToolChanged += ActiveToolDidChange;
    }

    private void OnDisable() {
        EditorTools.activeToolChanged -= ActiveToolDidChange;
    }

    public override void OnToolGUI(EditorWindow window) {
        var evt = Event.current;
        if(evt.type == EventType.Repaint) { 
            var zTest = Handles.zTest;
            Handles.zTest = CompareFunction.LessEqual;
            foreach(var entry in m_Vertices) {
                foreach(var vertex in entry.positions) {
                    var world = entry.transform.TransformPoint(vertex);
                    Handles.DotHandleCap(0, world, Quaternion.identity, HandleUtility.GetHandleSize(world) * 0.05f, evt.type);
                }
            }
            Handles.zTest = zTest;
        }
    }
    */
}
