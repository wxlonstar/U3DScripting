using BezierMasterNS.MeshesCreating;
using BezierMasterNS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TubeMeshCreator))]
public class TubeCreatorEditor : MeshCreatorEditor
{
    protected SerializedProperty radius2Property;


    string radius2PropertyName = "radius2";


    public override void Setup(BezierMaster bezierMaster)
    {

        base.Setup(bezierMaster);

        radius2Property = serializedObject.FindProperty(radius2PropertyName);
    }

    public override void OnInspectorGUI()
    {
        //Debug.Log(target.name);     

        serializedObject.Update();

        EditorGUILayout.PropertyField(lenghtSegmentsProperty);
        lenghtSegmentsProperty.intValue = Mathf.Clamp(lenghtSegmentsProperty.intValue, 3, 1000);

        EditorGUILayout.PropertyField(widhtSegmentsProperty);
        widhtSegmentsProperty.intValue = Mathf.Clamp(widhtSegmentsProperty.intValue, 2, 100);

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(widthProperty);

        EditorGUILayout.PropertyField(radius2Property);

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
