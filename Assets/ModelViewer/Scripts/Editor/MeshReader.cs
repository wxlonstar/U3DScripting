using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MileCode {
    public class MeshReader {
        MeshFilter meshfilter;
        MeshRenderer meshRenderer;
        public float turnTableSpeed = 0.0f;
        Vector3 originalPosition = Vector3.zero;
        Vector3 originalEulerAngles = Vector3.zero;
        Vector3 originalScale = Vector3.one;
        Material defaultMaterial;
        Material UVMaterial;
        Material mapCheckerMaterial;
        public float uvTilling = 1;
        public Dictionary<string, string> texturesInUse;

        public MeshReader(MeshRenderer meshRenderer) {
            if(meshRenderer != null) {
                this.meshRenderer = meshRenderer;
                MeshFilter mf = meshRenderer.GetComponent<MeshFilter>();
                if(mf != null) {
                    this.meshfilter = mf;
                }
                this.originalEulerAngles = meshRenderer.transform.localEulerAngles;
                this.originalPosition = meshRenderer.transform.localPosition;
                this.originalScale = meshRenderer.transform.localScale;
                this.defaultMaterial = meshRenderer.sharedMaterial;
                this.UVMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/ModelViewer/Materials/UVChecker.mat");
                this.mapCheckerMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                this.SetTexturesInUse();
            }
        }

        public string GetMeshName() {
            if(this.meshfilter != null) {
                return this.meshfilter.sharedMesh.name;
            } else {
                return "Mesh not found";
            }
        }

        public float GetMeshVerticesCount() {
            return this.meshfilter.sharedMesh.vertexCount;
        }

        public float GetMeshTrianglesCount() {
            return this.meshfilter.sharedMesh.triangles.Length / 3f;
        }

        public int GetSubMeshCount() {
            return this.meshfilter.sharedMesh.subMeshCount;
        }

        public bool HasNormal() {
            if(this.meshfilter.sharedMesh.HasVertexAttribute(VertexAttribute.Normal)) {
                return true;
            } else {
                return false;
            }
        }

        public bool HasVertexColor() {
            if(this.meshfilter.sharedMesh.HasVertexAttribute(VertexAttribute.Color)) {
                return true;
            } else {
                return false;
            }
        }

        public bool HasUV2() {
            if(this.meshfilter.sharedMesh.HasVertexAttribute(VertexAttribute.TexCoord1)) {
                return true;
            } else {
                return false;
            }
        }

        public void RotateMesh(float speed) {
            if(this.meshRenderer != null) {
                this.meshRenderer.transform.Rotate(new Vector3(0, Time.deltaTime * speed, 0));
            }      
        }

        public void ResetMeshTransform() {
            this.meshRenderer.transform.localPosition = this.originalPosition;
            this.meshRenderer.transform.localEulerAngles = this.originalEulerAngles;
            this.meshRenderer.transform.localScale = this.originalScale;
            // by the way, set default material
            this.SetDefaultMaterial();
        }

        public void SetDefaultMaterial() {
            this.meshRenderer.sharedMaterial = this.defaultMaterial;
        }

        public void CheckUV() {
            this.meshRenderer.sharedMaterial = this.UVMaterial;
        }

        public void TileUV(float scale) {
            if(this.meshRenderer.sharedMaterial.name == "UVChecker") {
                this.meshRenderer.sharedMaterial.SetTextureScale("_BaseMap", new Vector2(scale, scale));
            }
        }

        public string GetDefaultMaterialName() {
            return this.defaultMaterial.name;
        }

        public string GetDefaultShaderName() {
            return this.defaultMaterial.shader.name;
        }


        private void SetTexturesInUse() {
            ///List<Texture> texturesInUse = new List<Texture>();
            Dictionary<string, string> texturesInUse = new Dictionary<string, string>();
            string[] textureVariables = this.defaultMaterial.GetTexturePropertyNames();
            if(textureVariables.Length >= 1) {
                foreach(string textureVariable in textureVariables) {
                    Texture textureFileName = this.defaultMaterial.GetTexture(textureVariable);
                    if(textureFileName != null) {
                        texturesInUse.Add(textureVariable, textureFileName.name);
                    }
                }
            }
            this.texturesInUse = texturesInUse;
        }

        public void CheckMap(string mapName) {
            Texture targetTexture = this.defaultMaterial.GetTexture(mapName);
            mapCheckerMaterial.SetTexture("_BaseMap", targetTexture);
            this.meshRenderer.sharedMaterial = mapCheckerMaterial;
        }

    }
}
