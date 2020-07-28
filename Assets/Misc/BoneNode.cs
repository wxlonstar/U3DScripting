using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneNode : MonoBehaviour {
    public ScriptableObject boneSizeData;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private BoneScaler boneScaler;
    private void Start() {
        skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer>();
        if(skinnedMeshRenderer == null) {
            return;
        } else {
            Debug.Log(skinnedMeshRenderer.sharedMesh.name + " has been loaded.");
            BoneScaler.Initialization(boneSizeData, skinnedMeshRenderer);
            //BoneScaler.DeformToFat();
        }
    }

    
}
