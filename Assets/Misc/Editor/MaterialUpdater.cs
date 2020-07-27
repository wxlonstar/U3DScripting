using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MileCode {
    public class MaterialUpdater : EditorWindow {
        
        [MenuItem("Window/Update Materials")]
        private static void UpdateSelection() {
            MaterialUpdater updaterWindow = EditorWindow.GetWindow(typeof(MaterialUpdater)) as MaterialUpdater;
            GUIContent gUIContent = new GUIContent();
            gUIContent.text = "Material Upater";
            gUIContent.tooltip = "This tool is used to update project materials";
            updaterWindow.titleContent = gUIContent;
            Vector2 windowSize = new Vector2(500, 500);
            updaterWindow.maxSize = windowSize;
            updaterWindow.minSize = windowSize;
            updaterWindow.Show();
        }



        private void OnGUI() {
            GUILayout.Label("----------------------------------------------------------------------------------", EditorStyles.boldLabel);
            if(GUILayout.Button("Import Materials")) {
                var selection = Selection.objects;
                if(selection == null || selection.Length == 0) {
                    EditorUtility.DisplayDialog("Error", "You the man", "Yeah");
                    return;
                } else {
                    
                }
            }
            EditorGUI.ObjectField(new Rect(new Vector2(5, 15), new Vector2(150, 20)), null, typeof(Material), true);
        }
    }
}
