using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UsingCustomInspector))]
public class CustomInspectorUI : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        //GUILayout.Label("Let me see");
        EditorGUILayout.LabelField("Current exp");
        //UsingCustomInspector uci = (UsingCustomInspector)target;
        UsingCustomInspector uci = target as UsingCustomInspector;
        uci.Exp = EditorGUILayout.IntField("Exp: ", uci.Exp);
        EditorGUILayout.LabelField("Level: " + uci.Level);
    }
}
