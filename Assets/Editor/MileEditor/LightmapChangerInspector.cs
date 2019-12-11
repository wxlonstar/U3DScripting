using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightmapChanger))]
public class LightmapChangerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        LightmapChanger lightData = (LightmapChanger)target;
        if(GUILayout.Button("Load")) {
            lightData.Load();
        }
        if(GUILayout.Button("Save")) {
            if(lightData.IsLightmpaDirectoryExists(lightData.resourceFolder)) {
                if (lightData.IsLightmpaDirectoryExists(lightData.resourceFolder)) {
                    if (!EditorUtility.DisplayDialog("Overwrite Lightmap Resources?", "Lighmap Resources folder with name: \"" + lightData.resourceFolder + "\" already exists.\n\nPress OK to overwrite existing lightmap data.", "OK", "Cancel")) {
                        return;
                    }
                } else {
                    if (!EditorUtility.DisplayDialog("Create Lightmap Resources?", "Create new lighmap Resources folder: \"" + lightData.resourceFolder + "?", "OK", "Cancel")) {
                        return;
                    }
                }
                lightData.GenerateStoredLightmapInfo();

            }
        }
    }
}
