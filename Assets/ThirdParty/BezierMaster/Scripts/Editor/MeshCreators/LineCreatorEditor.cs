using BezierMasterNS.MeshesCreating;
using BezierMasterNS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineMeshCreator))]
public class LineCreatorEditor : MeshCreatorEditor
{  

    public override void OnInspectorGUI()
    {  

        serializedObject.Update();

        EditorGUILayout.PropertyField(lenghtSegmentsProperty);
        lenghtSegmentsProperty.intValue = Mathf.Clamp(lenghtSegmentsProperty.intValue, 3, 1000);

        EditorGUILayout.PropertyField(widhtSegmentsProperty );
        widhtSegmentsProperty.intValue = Mathf.Clamp(widhtSegmentsProperty.intValue, 2, 100);

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(widthProperty, new GUIContent("Width"));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(twoSidedProperty);
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


