using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace MileCode {
    public class MeshInfo : MonoBehaviour {
        [HideInInspector]
        public string meshName = "No Found Mesh";
        [HideInInspector]
        public float verticesNumber = 0;
        [HideInInspector]
        public float trianglesNumber = 0;
        [HideInInspector]
        public int subMeshCount = 0;
        [HideInInspector]
        public bool containsNormal = false;
        [HideInInspector]
        public bool containsColor = false;
        [HideInInspector]
        public bool containsUV2 = false;
        [HideInInspector]
        public float turnTableSpeed = 0;

        private MeshRenderer meshRenderer;
        //private Material[] defaultMaterials;
        private bool isDefaultMaterialSet = false;
        Material defaultMaterial;
        Material uvCheckerMaterial;
        Material mapCheckerMaterial;

        private void OnValidate() {
            MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
            if(meshRenderer != null) {
                this.meshRenderer = meshRenderer;
                if(this.meshRenderer.sharedMaterial != null) {
                    defaultMaterial = this.meshRenderer.sharedMaterial;
                    if(defaultMaterial == null) {
                        Debug.Log("Default material initialized failed.");
                    }
                    uvCheckerMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/AssetViewer/Materials/UVChecker.mat");
                    if(uvCheckerMaterial == null) {
                        Debug.Log("uvCheckerMaterial initialized failed.");
                    }
                    mapCheckerMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                    if(mapCheckerMaterial == null) {
                        Debug.Log("mapCheckerMaterial initialized failed.");
                    }

                }
            }

        }

        public bool ReadMesh() {
            bool readState = true;
            MeshFilter meshFilter = this.GetComponent<MeshFilter>();
            if(meshFilter != null) {
                //Debug.Log(meshFilter.sharedMesh.name);
                meshName = meshFilter.sharedMesh.name;
                trianglesNumber = meshFilter.sharedMesh.triangles.Length / 3;
                verticesNumber = meshFilter.sharedMesh.vertexCount;
                subMeshCount = meshFilter.sharedMesh.subMeshCount;
                if(meshFilter.sharedMesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Normal)) {
                    containsNormal = true;
                }
                if(meshFilter.sharedMesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color)) {
                    containsColor = true;
                }

                containsUV2 = false;
                if(meshFilter.sharedMesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.TexCoord1)) {
                    containsUV2 = true;
                }          
            } else {
                readState = false;
            }
            return readState;
        }

        public void turnTable(float speed) {            
            this.transform.Rotate(new Vector3(0, Time.deltaTime * speed, 0));
        }

        private void ProtectCurrentMaterials() {
            if(isDefaultMaterialSet == false) {
                this.meshRenderer = this.GetComponent<MeshRenderer>();
                if(this.meshRenderer == null) {
                    return;
                }
                defaultMaterial = this.meshRenderer.sharedMaterial;
            }
            isDefaultMaterialSet = true;
        }

        public void SetDefaultMaterial() {
            meshRenderer.sharedMaterial = defaultMaterial;
        }

        public void CheckUV() {
            meshRenderer.sharedMaterial = uvCheckerMaterial;
        }

        [HideInInspector]
        public float textureTilling = 1;
        public void TillingTexture(float tilling) {
            if(meshRenderer != null) {
                if(meshRenderer.sharedMaterial.name == "UVChecker") {
                    //Debug.Log(meshRenderer.sharedMaterial.GetTextureScale("_BaseMap"));
                    meshRenderer.sharedMaterial.SetTextureScale("_BaseMap", new Vector2(tilling, tilling));
                }
            }
            
        }

        public Dictionary<string, string> GetTexturesNamesInUse() {
            Dictionary<string, string> textureNames = new Dictionary<string, string>();
            if(defaultMaterial == null) {
                return textureNames;
            }
            string[] textureVarNames = defaultMaterial.GetTexturePropertyNames();
            foreach(string textureVarName in textureVarNames) {
                Texture textureFileName = defaultMaterial.GetTexture(textureVarName);
                if(textureFileName != null) {
                    textureNames.Add(textureVarName, textureFileName.name);
                }
            }
            return textureNames;
        }

        public List<string> FetchMaterialTextureNames() {

            List<string> textureNamesInUse = new List<string>();
            if(defaultMaterial == null) {
                return textureNamesInUse;
            }
            string[] textureNames = defaultMaterial.GetTexturePropertyNames();
            foreach(string textureName in textureNames) {
                Texture texture = defaultMaterial.GetTexture(textureName);
                if(texture != null) {
                    textureNamesInUse.Add(texture.name);
                }
            }
            return textureNamesInUse;
        }

        public string GetDefaultMaterialName() {
            if(defaultMaterial != null) {
                return defaultMaterial.name;
            } else {
                return "Can't find material in use.";
            }
        }

        public void CheckMap(string mapName) {
            //Debug.Log("Check map: " + mapName);
            //Material material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            if(mapCheckerMaterial != null) {
                if(defaultMaterial != null) {
                    Texture texture = defaultMaterial.GetTexture(mapName);
                    //Debug.Log(texture.name);
                    mapCheckerMaterial.SetTexture("_BaseMap", texture);
                    meshRenderer.sharedMaterial = mapCheckerMaterial;
                }
            }
        }

        public string GetShaderName() {
            if(defaultMaterial != null) {
                return defaultMaterial.shader.name;
            }
            return "Shader not found";
        }

        public void ResetMesh() {
            if(this.meshRenderer != null) {
                if(this.defaultMaterial != null) {
                    this.meshRenderer.sharedMaterial = this.defaultMaterial;
                }
            }
            if(this != null) {
                this.isDefaultMaterialSet = false;
                this.transform.localPosition = Vector3.zero;
                this.transform.localRotation = Quaternion.identity;
                this.transform.localScale = Vector3.one;
            }
            
        }

    }
}
