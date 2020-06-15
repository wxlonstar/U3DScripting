using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.EditorTools;

//[EditorTool("Light Group", typeof(MeshRenderer))]
public class LightGroup : EditorTool {
    GUIContent m_ToolbarIcon;
    GUIContent Slider;
    public override GUIContent toolbarIcon {
        get {
            if(m_ToolbarIcon == null) {
                m_ToolbarIcon = new GUIContent(
                        AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AssetViewer/Textures/Icons/Light-Bulb-icon.png"),
                        "Light Group"
                    );
            }
            return m_ToolbarIcon;

        }
    }

    GameObject lightGroup;
    void ActiveToolDidChange() {
        if(!EditorTools.IsActiveTool(this)) {
            return;
        }
        if(lightGroup == null) {
            lightGroup = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetViewer/Prefabs/LightGroup.prefab");
            Instantiate(lightGroup);
        }
        Debug.Log("Changed");
    }

    private void OnEnable() {
        EditorTools.activeToolChanged += ActiveToolDidChange;
        Debug.Log("Enable");
    }

    private void OnDisable() {
        EditorTools.activeToolChanged -= ActiveToolDidChange;
        //Debug.Log("Disable");
    }

}
