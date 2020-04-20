using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BezierMasterNS;
using BezierMasterNS.MeshesCreating;


[CustomEditor(typeof(CreateMeshBase), true)]
public class MeshCreatorEditor : Editor
{

    protected CreateMeshBase meshCreator;
    protected BezierMaster master;

    protected SerializedProperty lenghtSegmentsProperty;
    protected SerializedProperty widhtSegmentsProperty;
    protected SerializedProperty widthProperty;
    protected SerializedProperty twoSidedProperty;
    protected SerializedProperty textureOrientationProperty;
    protected SerializedProperty fixTextureStretchProperty;
    protected SerializedProperty TextureScaleProperty;


    string lenghtSegmentsPropertyName = "lenghtSegmentsCount";
    string widhtSegmentsPropertyName = "widhtSegmentsCount";
    string widthPropertyName = "Radius1";
    string twoSidedPropertyName = "twoSided";
    string textureOrientationPropertyName = "textureOrientation";
    string fixTextureStretchPropertyName = "fixTextureStretching";
    string textureScalePropertyName = "textureScale";

    private void OnEnable()
    {
        meshCreator = target as CreateMeshBase;

        try
        { 

            lenghtSegmentsProperty = serializedObject.FindProperty(lenghtSegmentsPropertyName);

            widhtSegmentsProperty = serializedObject.FindProperty(widhtSegmentsPropertyName);

            widthProperty = serializedObject.FindProperty(widthPropertyName);

            twoSidedProperty = serializedObject.FindProperty(twoSidedPropertyName);

            textureOrientationProperty = serializedObject.FindProperty(textureOrientationPropertyName);

            fixTextureStretchProperty = serializedObject.FindProperty(fixTextureStretchPropertyName);

            TextureScaleProperty = serializedObject.FindProperty(textureScalePropertyName);
        }
        catch
        {
            DestroyImmediate(this);
        }
    }

    public virtual void Setup(BezierMaster bezierMaster)
    {
        master = bezierMaster;
    }


    public void DrawCopyPasteMenu()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Copy mesh"))
        {
            BezierMaster.CopyMesh(meshCreator);
        }
        if (GUILayout.Button("Paste mesh"))
        {
            Undo.RecordObject(master, "Paste spline");

            master.PasteMesh();           

            EditorUtility.SetDirty(master);
        }
        GUILayout.EndHorizontal();
    }
   

}

