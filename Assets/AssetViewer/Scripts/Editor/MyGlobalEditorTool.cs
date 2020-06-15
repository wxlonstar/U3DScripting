using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;


// this tool button will be accesible through the top left toolbar in the editor
//[EditorTool("MyGlobalTool")]
public class MyGlobalEditorTool : EditorTool {
    // this image must must be assgined in OnEnable function, icon will be seen when this tool is activated.
    [SerializeField]
    Texture2D toolbarIconImage;

    GUIContent testToolBarGUI;

    // this method will be called after editor window is ready, only once
    private void OnEnable() {
        toolbarIconImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ThirdParty/Other/Art/Platformer/Platformer_InteractiveCoin_Normal6.png");
        //Debug.Log("On Enable is called.");
        testToolBarGUI = new GUIContent() {
            image = toolbarIconImage,
            text = "A new name",
            tooltip = "Miles added this button"
        };
    }

    // this property make sure the specified icon will be shown on editor tool bar
    public override GUIContent toolbarIcon {
        get {
            return testToolBarGUI;
        }
    }

    public override void OnToolGUI(EditorWindow window) {
        EditorGUI.BeginChangeCheck();
        Vector3 position = Tools.handlePosition;
        using(new Handles.DrawingScope(Color.green)) {
            //position = Handles.Slider(position, Vector3.right);
        }
        /*
        if(EditorGUI.EndChangeCheck()) {
            Vector3 delta = position - Tools.handlePosition;
            Undo.RecordObjects(Selection.transforms, "My Oh My");
            foreach(var transform in Selection.transforms) {
                transform.position += delta;
            }
        }
        */
    }

}
