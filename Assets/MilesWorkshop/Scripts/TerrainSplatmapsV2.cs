using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class TerrainSplatmapsV2 : MonoBehaviour {
    public Texture2D[] splatMaps;
    public TerrainData terrainData;

    private void Update() {
        this.SetMaterialWithTerrainData();
    }


    private void SetMaterialWithTerrainData() {
        if(this.terrainData == null) {
            Debug.Log("Can't find any terrain data");
            return;
        }

        if(!IsShaderCorrect("SoFunny/Chicken-Terrain_v2")) {
            Debug.Log("Shader is not Correct");
            return;
        }
        Texture2D[] splatmaps = new Texture2D[this.terrainData.alphamapTextureCount];


        //Debug.Log(splatmaps.Length);
        
    }



    private bool IsShaderCorrect(string shaderFullName) {
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        if(mr.sharedMaterial != null) {
            if(mr.sharedMaterial.shader.name == shaderFullName) {
                return true;
            } else {
                return false;
            }
        } else {
            Debug.Log("Can't find the material.");
            return false;
        }
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
