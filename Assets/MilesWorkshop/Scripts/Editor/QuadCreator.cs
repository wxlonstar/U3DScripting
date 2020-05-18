using UnityEngine;
using UnityEditor;
using System.Security.Policy;
using System.IO;

public class QuadCreator : Editor {
    public static float width = 1;
    public static float height = 1;
    public static GameObject gameObject;
    public static string savePath = "Assets/Exports/";

    [MenuItem("GoQuad/MakeAndSaveQuad")]
    public static void Go() {
        gameObject = new GameObject();
        Debug.Log("asdfa");
        Start1();
    }
    public static void Start1() {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;

        AssetDatabase.CreateAsset(mesh, savePath + "mesh.asset");
        AssetDatabase.SaveAssets();
    }
}
