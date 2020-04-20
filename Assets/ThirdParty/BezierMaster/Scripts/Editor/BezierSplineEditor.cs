using BezierMasterNS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor
{
    BezierMaster master;

    BezierSpline spline;
    public int selectedIndex = -1;

    event Action onPasted;

    private void OnEnable()
    {
        spline = target as BezierSpline;
    }
    private void OnDisable()
    {
        
    }


    public void EditorSetup(BezierMaster master, Action onPasted)
    {
        this.master = master;
        this.onPasted = onPasted;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CurveEditor();

        serializedObject.ApplyModifiedProperties();
    }

    private void CurveEditor()
    {
        

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button( new GUIContent("Add point", "Adds point in the end")))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }

        if ( GUILayout.Button(new GUIContent("Insert point", "Inserts point after selected")))
        {
            Undo.RecordObject(spline, "Add Point");
            spline.InsertPoint(selectedIndex);
            EditorUtility.SetDirty(spline);
        }

        if (GUILayout.Button(new GUIContent("Reset spline", "Reset all changes")))
        {
            Undo.RecordObject(spline, "Reset");

            spline.Reset();
           // master.Reset();
            selectedIndex = -1;
            EditorUtility.SetDirty(spline);
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Copy spline")))
        {
            BezierMaster.CopySpline(spline);
        }
        if (BezierMaster.copySpline != null && GUILayout.Button("Paste spline") )
        {
            Undo.RecordObject(master, "Paste spline");

            master.PasteSpline();
            spline = master.spline;

            onPasted?.Invoke();

            EditorUtility.SetDirty(master);
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        bool loop = EditorGUILayout.Toggle(new GUIContent("Loop", "Is spline closed?"), spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }

        EditorGUILayout.LabelField("Curve Lenght:", spline.GetCurveLenght().ToString());

        //GUILayout.Label();
        GUILayout.Space(5);
        GUILayout.EndVertical();

        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
            DrawSelectedPointInspector();
        else
        {
            GUILayout.BeginVertical("box");
            GUILayout.Space(5);
            GUILayout.Label("   Select point to edit!");
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        {
            

            GUILayout.Label("Selected Point: ");

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("<", GUILayout.Width(20)))
            {
                selectedIndex--;

                if (selectedIndex < 0)
                    selectedIndex = spline.ControlPointCount - 1;
            }

            GUILayout.Label(selectedIndex.ToString());

            if (GUILayout.Button(">", GUILayout.Width(20)))
            {
                selectedIndex++;

                if (selectedIndex >= spline.ControlPointCount)
                    selectedIndex = 0;
            }

            GUILayout.FlexibleSpace();



            if (GUILayout.Button("Remove" , GUILayout.Width(60)))
            {
                Undo.RecordObject(spline, "Remove Point");

                spline.RemoveCurve(selectedIndex);

                selectedIndex--;

                if (selectedIndex < 0)
                    selectedIndex = 0;

                EditorUtility.SetDirty(spline);
            }


        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPointPosition(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPointPosition(selectedIndex, point);

        }
        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
             EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        float zRotation = EditorGUILayout.FloatField(new GUIContent("Z Rotation", "Rotates child objects/mesh at this segment by degree"), spline.zRotationAtPoint[(selectedIndex + 1) / 3]);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Variable change");
            int index = (selectedIndex + 1) / 3;
            EditorUtility.SetDirty(spline);
            spline.zRotationAtPoint[index] = zRotation;

            if (spline.Loop && (index == spline.zRotationAtPoint.Length - 1 || index == 0))
                spline.zRotationAtPoint[index] = spline.zRotationAtPoint[0] = zRotation;

        }
        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        Vector3 scale = EditorGUILayout.Vector3Field(new GUIContent("Scale Factor", "Scale of child objects/mesh at this segment"), spline.scaleFactor3d[(selectedIndex + 1) / 3]);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Variable change");
            int index = (selectedIndex + 1) / 3;
            EditorUtility.SetDirty(spline);
            spline.scaleFactor3d[index] = scale;

            if (spline.Loop && (index == spline.zRotationAtPoint.Length - 1 || index == 0))
                spline.scaleFactor3d[index] = spline.scaleFactor3d[0] = scale;

        }

        if (GUILayout.Button(new GUIContent("Reset values", "Reset scale and rotation to default")))
        {
            Undo.RecordObject(spline, "Reset Point");

            int index = (selectedIndex + 1) / 3;

            spline.zRotationAtPoint[index] = 0;
            spline.scaleFactor3d[index] = Vector3.one;

            EditorUtility.SetDirty(spline);
        }

        GUILayout.Space(5);
        GUILayout.EndVertical();

    }
}
