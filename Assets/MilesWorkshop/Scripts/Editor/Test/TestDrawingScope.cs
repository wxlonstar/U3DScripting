using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshRenderer))]
public class TestDrawingScope : Editor {
    void OnSceneGUI() {
        Debug.Log("...");
        MeshRenderer meshRenderer = (MeshRenderer)target;

        Vector3 cameraToTarget = Camera.current.transform.position - meshRenderer.transform.position;
        Quaternion billboardOrientation = Quaternion.LookRotation(cameraToTarget, Camera.current.transform.up);
        Matrix4x4 matrix = Matrix4x4.TRS(meshRenderer.transform.position, billboardOrientation, Vector3.one);

        using(new Handles.DrawingScope(Color.magenta, matrix)) {
            Vector3 size = meshRenderer.bounds.size;
            float radius = Mathf.Max(size.x, size.y, size.z);
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.right, 360f, radius);
            Handles.Label(Vector3.up * radius, meshRenderer.name);
        }
    }
}
