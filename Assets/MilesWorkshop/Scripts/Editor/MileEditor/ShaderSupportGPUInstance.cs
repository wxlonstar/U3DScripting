using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderSupportGPUInstance : ShaderGUI {
    Material target;
    MaterialEditor materialEditor;
    MaterialProperty[] materialProperties;

    void DoAdvanced() {
        //GUILayout.Label("GPU INSTANCE", EditorStyles.boldLabel);
        //this.materialEditor.EnableInstancingField();
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        base.OnGUI(materialEditor, properties);
        this.target = materialEditor.target as Material;
        this.materialEditor = materialEditor;
        this.materialProperties = properties;
        DoAdvanced();
    }
}
