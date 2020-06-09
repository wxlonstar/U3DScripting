using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshRenderer))]
public class ShowMeshInfo : Editor {
    string meshName;
    float trianglesCount;
    float verticesCount;
    float subMeshCount;
    bool hasVertexNormal = false;
    bool hasVertexColor = false;
    bool hasUV2 = false;

    void OnSceneGUI() {
        MeshRenderer meshRenderer = (MeshRenderer)target;
        //MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
        Mesh mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
        if(mesh != null) {
            trianglesCount = mesh.triangles.Length / 3;
            verticesCount = mesh.vertexCount;
            subMeshCount = mesh.subMeshCount;
            meshName = mesh.name;
            if(mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Normal)) {
                hasVertexNormal = true;
            }
            if(mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color)) {
                hasVertexColor = true;
            }
            if(mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.TexCoord1)) {
                hasUV2 = true;
            }
        }

        //Debug.Log(SceneView.lastActiveSceneView.sceneLighting.ToString());
        

        Vector3 cameraToTarget = Camera.current.transform.position - meshRenderer.transform.position;
        Quaternion billboardOrientation = Quaternion.LookRotation(cameraToTarget, Camera.current.transform.up);
        Matrix4x4 matrix = Matrix4x4.TRS(meshRenderer.transform.position, billboardOrientation, Vector3.one);
        float distance = Vector3.Magnitude(cameraToTarget);
        if(distance <= 70) {
            using(new Handles.DrawingScope(Color.magenta, matrix)) {
                Vector3 size = meshRenderer.bounds.size;
                float radius = Mathf.Max(size.x, size.y, size.z);

                Handles.Label(Vector3.up * radius * 0.7f, "Mesh Name: " + meshName);
                Handles.Label(Vector3.up * radius * 0.66f, "Vertices: " + verticesCount + " | Triangles: " + trianglesCount + " | SubMesh: " + subMeshCount);
                Handles.Label(Vector3.up * radius * 0.62f, "Normals: " + hasVertexNormal + " | Color: " + hasVertexColor + " | UV2: " + hasUV2);
            }
        }

    }
}
