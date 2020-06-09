using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

//[EditorTool("Platform Tool")]
public class SceneViewTool : EditorTool {
    [SerializeField]
    Texture2D m_ToolIcon;
    
    GUIContent m_IconContent;

    /*
    private void OnEnable() {
        m_IconContent = new GUIContent() { 
            image = m_ToolIcon,
            text = "Platform Tool",
            tooltip = "Platform Tooltip"
        };
    }

    public override GUIContent toolbarIcon {
        get {
            return m_IconContent;
        }
    }
    */
    /*
    public override void OnToolGUI(EditorWindow window) {
        EditorGUI.BeginChangeCheck();
        Vector3 position = Tools.handlePosition;
        using(new Handles.DrawingScope(Color.green)) {
            position = Handles.Slider(position, Vector3.right);
        }

        if(EditorGUI.EndChangeCheck()) {
            Vector3 delta = position - Tools.handlePosition;
            Undo.RecordObjects(Selection.transforms, "Move Platform");
            foreach(var transform in Selection.transforms) {
                transform.position += delta;
            }
        }
    }
    */
}
