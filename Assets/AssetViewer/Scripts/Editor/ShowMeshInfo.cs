using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditor.VersionControl;

[CustomEditor(typeof(MeshRenderer))]
public class ShowMeshInfo : Editor {
    string meshName;
    float trianglesCount;
    float verticesCount;
    float subMeshCount;
    bool hasVertexNormal = false;
    bool hasVertexColor = false;
    bool hasUV2 = false;
    LightTools lt;
    ReflectionProbeTools rpt;

    private void OnDisable() {
        /*
        GameObject lightEnv = GameObject.Find("LightEnv(Clone)");
        if(lightEnv != null) {
            GameObject.DestroyImmediate(lightEnv);
        }
        */
    }

    

    void OnSceneGUI() {
        MeshRenderer meshRenderer = (MeshRenderer)target;
        /*
        AssetViewerParams avp = meshRenderer.GetComponent<AssetViewerParams>();
        if(avp == null) {
            meshRenderer.gameObject.AddComponent<AssetViewerParams>();
        }
        */
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


        if(GameObject.Find("LightEnv(Clone)") == null) {
            GameObject lightEnv = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetViewer/Prefabs/LightEnv.prefab");
            Instantiate(lightEnv);
            //Debug.Log(lightEnv.name);
            lt = lightEnv.GetComponentInChildren<LightTools>();
            rpt = lightEnv.GetComponentInChildren<ReflectionProbeTools>();
            //Debug.Log(lightEnv.GetComponentsInChildren<Transform>()[2].name);
        } 

        //Debug.Log(SceneView.lastActiveSceneView.sceneLighting.ToString());
        Vector3 cameraToTarget = Camera.current.transform.position - meshRenderer.transform.position;
        Quaternion billboardOrientation = Quaternion.LookRotation(cameraToTarget, Camera.current.transform.up);
        Matrix4x4 matrix = Matrix4x4.TRS(meshRenderer.transform.position, billboardOrientation, Vector3.one);
        using(new Handles.DrawingScope(Color.magenta, matrix)) {
            Vector3 size = meshRenderer.bounds.size;
            float radius = Mathf.Max(size.x, size.y, size.z);

            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(10, 10, 300, 100), boxStyle);
                {
                    GUILayout.Label("Mesh Infomation");
                    GUILayout.Label("Mesh Name: " + meshName);
                    GUILayout.Label("Vertices: " + verticesCount + " | Triangles: " + trianglesCount + " | SubMesh: " + subMeshCount);
                    GUILayout.Label("Normals: " + hasVertexNormal + " | Color: " + hasVertexColor + " | UV2: " + hasUV2);
                    if(rpt != null) {
                        rpt.intensity = GUILayout.HorizontalSlider(rpt.intensity, 0, 1);
                    }
                    
                }
                GUILayout.EndArea();

            }
            Handles.EndGUI();
        }

    }
}
