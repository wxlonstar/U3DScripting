using UnityEngine;
using UnityEditor;

// Create an editor window which can display a chosen GameObject.
// Use OnInteractivePreviewGUI to display the GameObject and
// allow it to be interactive.

public class TestPreview : EditorWindow {
    GameObject gameObject;
    Editor gameObjectEditor;

    [MenuItem("Tools/Test Preview")]
    static void ShowWindow() {
        GetWindowWithRect<TestPreview>(new Rect(0, 0, 1024, 1024));
    }
    
    void OnGUI() {
        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);
        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;
        if(gameObject != null) {
            if(gameObjectEditor == null) {
                gameObjectEditor = Editor.CreateEditor(gameObject);
                gameObjectEditor.DrawHeader();
                gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(1024, 1024), bgColor);
            }
            
            //Debug.Log(gameObjectEditor.target.name);
        }
        /*
        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;

        if(gameObject != null) {
            
            if(gameObjectEditor == null) {

                gameObjectEditor = Editor.CreateEditor(gameObject);
            }

            gameObjectEditor.DrawHeader();

            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(1024 , 1024), bgColor);


        }
        */
        
        
    }
    
}