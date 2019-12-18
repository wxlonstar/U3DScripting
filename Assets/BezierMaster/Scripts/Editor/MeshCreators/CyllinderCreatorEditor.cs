using BezierMasterNS.MeshesCreating;
using BezierMasterNS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CylinderMeshCreator))]
public class CyllinderCreatorEditor : MeshCreatorEditor
{

    protected SerializedProperty capStartProperty;
    protected SerializedProperty capEndProperty;


    string capStartPropertyName = "capStart";
    string capEndPropertyName = "capEnd";

    public override void Setup(BezierMaster bezierMaster)
    {
        base.Setup(bezierMaster);

        capStartProperty = serializedObject.FindProperty(capStartPropertyName);

        capEndProperty = serializedObject.FindProperty(capEndPropertyName);
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUILayout.PropertyField(lenghtSegmentsProperty);
        lenghtSegmentsProperty.intValue = Mathf.Clamp(lenghtSegmentsProperty.intValue, 3, 1000);

        EditorGUILayout.PropertyField(widhtSegmentsProperty);
        widhtSegmentsProperty.intValue = Mathf.Clamp(widhtSegmentsProperty.intValue, 2, 100);

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(widthProperty, new GUIContent("Radius"));
        GUILayout.Space(5);


        if (!master.spline.Loop)
        {
            EditorGUILayout.PropertyField(capStartProperty);
            EditorGUILayout.PropertyField(capEndProperty);
        }

        GUILayout.Space(5);


        EditorGUILayout.PropertyField(textureOrientationProperty);
        EditorGUILayout.PropertyField(fixTextureStretchProperty);


        GUILayout.Space(5);

        EditorGUILayout.PropertyField(TextureScaleProperty);

        GUILayout.Space(5);

        DrawCopyPasteMenu();

        GUILayout.Space(5);

        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }


}
