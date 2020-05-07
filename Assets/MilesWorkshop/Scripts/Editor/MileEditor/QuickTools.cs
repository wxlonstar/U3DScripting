using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class QuickTools : EditorWindow {
    [MenuItem("MileTool/Open _%#T")]
    public static void ShowWindow() {
        //Debug.Log("Hello");
        var window = GetWindow<QuickTools>();
        window.titleContent = new GUIContent("MileTool");
        // this is the minimum size to the window
        window.minSize = new Vector2(250, 50);
    }
}
