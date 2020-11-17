using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class TerrainSplatmaps : MonoBehaviour {
    public Texture2D[] splatMaps;

    private void Update() {
        this.InitializeSplatmapsAndSetMaterial();

    }

    private void InitializeSplatmapsAndSetMaterial() {
        if(this.splatMaps.Length == 2) {
            if(this.splatMaps[0] != null && this.splatMaps[1] != null) {
                MeshRenderer mr = this.GetComponent<MeshRenderer>();
                if(mr.sharedMaterial.shader.name == "SoFunny/Chicken-Terrain_v2") {
                    this.SetSplatmapsForMaterials(mr.sharedMaterial, this.splatMaps);
                }
            } else {
                Debug.Log("One of the splatmap is not set correctly.");
                return;
            }

        } else {
            Debug.Log("Two splatmps are needed.");
                return;
        }
    }

    private void SetSplatmapsForMaterials(Material material, Texture2D[] textures) {
        material.SetTexture("_Control01", textures[0]);
        material.SetTexture("_Control02", textures[1]);
    }
}
