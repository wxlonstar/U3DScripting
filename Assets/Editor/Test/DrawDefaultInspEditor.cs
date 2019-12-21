using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UseDrawDefaultInsp))]
public class DrawDefaultInspEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("You wazzup", MessageType.Info);
    }
}
