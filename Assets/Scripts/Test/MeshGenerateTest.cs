using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerateTest {
    GameObject go;
    public void Gen() {
        go = new GameObject("GenMesh");
    }

    public void PrepareComponent() {
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        Mesh emptyMesh = new Mesh();
        meshFilter.mesh = AssembleMesh(emptyMesh);
    }

    public Mesh AssembleMesh(Mesh mesh) {
        Vector3 originPosition = go.transform.position;

        Vector3[] vertices = new Vector3[4] {
            originPosition + new Vector3(1, 1, 0),      // 1st vertex
            originPosition + new Vector3(-1, 1, 0),     // 2nd vertex
            originPosition + new Vector3(-1, -1, 0),    // 3rd vertex
            originPosition + new Vector3(1, -1, 0)      // 4th vertex
        };

        mesh.vertices = vertices;
        int[] trisOrder = new int[6] {
            1, 0, 2, 2, 0, 3
        };

        mesh.triangles = trisOrder;
        return mesh;
    }
}
