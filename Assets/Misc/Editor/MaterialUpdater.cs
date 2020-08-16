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
            gUIContent.text = "Material Updater";
            gUIContent.tooltip = "This tool is used to update project materials";
            updaterWindow.titleContent = gUIContent;
            Vector2 windowSize = new Vector2(600, 500);
            updaterWindow.maxSize = windowSize;
            updaterWindow.minSize = windowSize;
            updaterWindow.Show();
        }


        Rect rect_ImportMaterial = new Rect(new Vector2(0, 0), new Vector2(300, 500));
        Rect rect_MaterialBox = new Rect(new Vector2(3, 0), new Vector2(287, 18));
        Vector2 pos = new Vector2(10, 10);
        private void OnGUI() {
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");

                GUILayout.BeginArea(rect_ImportMaterial, boxStyle);
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label("---------------------------------------------------------------------------------------------------");
                    if(GUILayout.Button("Import Materials")) {
                        
                    }
                    using(var scrollView = new GUILayout.ScrollViewScope(this.pos)) {
                        this.pos = scrollView.scrollPosition;
                        //EditorGUI.ObjectField(rect_MaterialBox, null, typeof(Material), false);
                        EditorGUI.ObjectField(rect_MaterialBox, null, typeof(Material), false);
                        EditorGUI.ObjectField(rect_MaterialBox, null, typeof(Material), false);
                        
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
