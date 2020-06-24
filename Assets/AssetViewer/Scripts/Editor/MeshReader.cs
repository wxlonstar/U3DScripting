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
        }
    }
}
