using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using BezierMasterNS.MeshesCreating;

namespace BezierMasterNS
{
    

    [CustomEditor(typeof(BezierMaster))]
    public class BezierMasterInspectorExt : Editor
    {
        private BezierMaster master;
        private BezierSpline spline;

        BezierSplineEditor splineEditor;
        MeshCreatorEditor meshCreatorEditor;
       


        private Transform handleTransform;
        private Quaternion handleRotation;

        private const int lineSteps = 3;
        private const float directionScale = 0.05f;
        private const float handleSize = 0.1f;

        int count;
        private Color[] modeColors = {
             Color.white,
             Color.yellow,
             Color.cyan
        };


        [MenuItem("Tools/Create Bezier Master")]
        public static void CreateBezierMaster()
        {
            var master = new GameObject("Bezier Master");
            master.AddComponent<BezierMaster>().Reset();

            Selection.activeGameObject = master;
        }

        GUIStyle headerStyle;

        private void OnEnable()
        {
            master = target as BezierMaster;

            if (master.spline == null)
            {
                master.spline = BezierSpline.CreateSpline();
                master.instID = master.GetInstanceID();
            }            
            else if( master.instID != master.GetInstanceID())
            {
                // object was duplicated
                // create new instances of child scriptable objects to broke connection to reference
                OnDuplicate();

            }

           

            count = master.ObjectsCount;

            //Debug.Log("CreateMeshBase count " + FindObjectsOfType<CreateMeshBase>().Length);
            //Debug.Log("BezierSpline count " + FindObjectsOfType<BezierSpline>().Length);


           

            CreateSplineEditor();


            CreateMechCreatorEditor();          
        }

        public void OnDuplicate()
        {
            master.instID = master.GetInstanceID();

            if (master.spline)
                master.spline = BezierSpline.CreateSpline(master.spline);

            if (master.meshCreator)
                master.meshCreator = CreateMeshBase.InstatiateCreator(master.meshCreator);
        }

        void CreateSplineEditor()
        {

            spline = master.spline;

            if (splineEditor)
                DestroyImmediate(splineEditor);

            splineEditor = CreateEditor(spline) as BezierSplineEditor;
            splineEditor.EditorSetup(master, CreateSplineEditor);

        }
        void CreateMechCreatorEditor()
        {
            if (master.usingOfSpline != Using.Mesh)
                return;

            if (master.meshCreator && (master.meshCreator.GetMeshCreatorType() != master.meshType))// || )
                DestroyImmediate(master.meshCreator);

            if (!master.meshCreator)
            {
                Debug.Log("Create editor");

                switch (master.meshType)
                {
                    case MeshCreatorType.Line:

                        master.meshCreator = LineMeshCreator.CreateLineMesh(spline);

                        meshCreatorEditor = (LineCreatorEditor)CreateEditor(master.meshCreator, typeof(LineCreatorEditor));
                        break;

                    case MeshCreatorType.Cylinder:

                        master.meshCreator = CylinderMeshCreator.CreateCylinderMesh(spline);

                        meshCreatorEditor = (CyllinderCreatorEditor)CreateEditor(master.meshCreator, typeof(CyllinderCreatorEditor));

                        break;

                    case MeshCreatorType.Tube:
                        master.meshCreator = TubeMeshCreator.CreateTubeMesh(spline);

                        meshCreatorEditor = (TubeCreatorEditor)CreateEditor(master.meshCreator, typeof(TubeCreatorEditor));
                        break;
                }


                meshCreatorEditor.Setup(master);
            }
            else
            {
                master.meshCreator.spline = spline;

                meshCreatorEditor = (MeshCreatorEditor)CreateEditor(master.meshCreator);

                meshCreatorEditor.Setup(master);
            }

        }

        void CreateAsset(Mesh Meshasset)
        {
            var path = "Assets/" + Meshasset.name + ".asset";

            AssetDatabase.CreateAsset(Meshasset, path);

            AssetDatabase.SaveAssets();

            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        }

        bool showUpdateTab = true;

        public override void OnInspectorGUI()
        {
            if (this.target.GetInstanceID() == 0)
                return;

            GUILayout.Space(10);
            GUILayout.Label("Welcome to Besier Master! Just create line and then \nchoose options below");
            GUILayout.Space(5);

            GUILayout.BeginVertical("box");
            EditorGUI.indentLevel += 1;

            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.foldoutHeader);
                headerStyle.fontStyle = FontStyle.Bold;
            }

            master.showCurveEditor = EditorGUILayout.Foldout(master.showCurveEditor, "Curve Editor", headerStyle);

            if (master.showCurveEditor)
                splineEditor.OnInspectorGUI();


            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");

            master.showObjectsOptions = EditorGUILayout.Foldout(master.showObjectsOptions, "Create Objects", headerStyle);

            if (master.showObjectsOptions)
            {
                master.usingOfSpline = (Using)EditorGUILayout.EnumPopup(master.usingOfSpline);

                switch (master.usingOfSpline)
                {
                    case Using.Objects:
                        DestroyImmediate(master.meshGO);
                        ObjectsInstantiatingUI();
                        break;
                    case Using.Mesh:
                        master.ClearInstatiatedObjects(true);
                        MeshCreateUI();
                        break;

                    case Using.None:
                        DestroyImmediate(master.meshGO);
                        master.ClearInstatiatedObjects(true);
                        break;
                }

                
            }

          

            GUILayout.EndVertical();



                GUILayout.BeginVertical("box");
                showUpdateTab = EditorGUILayout.Foldout(showUpdateTab, "Update Mode", headerStyle);
                if (showUpdateTab)
                {
                    GUILayout.Space(5);

                    master.autoUpdate = EditorGUILayout.Toggle("Auto Update", master.autoUpdate);
                    if (!master.autoUpdate)
                    {
                        if (GUILayout.Button("Update"))
                            master.UpdateMaster(true);
                    }
                    else
                        master.UpdateMaster(false);

                    GUILayout.Space(5);
                }


                GUILayout.EndVertical();
           


            GUILayout.BeginVertical("box");
            master.showAnimationOptions = EditorGUILayout.Foldout(master.showAnimationOptions, "Animation", headerStyle);
            if (master.showAnimationOptions)
            {
                Animation();
            }
            GUILayout.EndVertical();


            

        }

        private void ObjectsInstantiatingUI()
        {
            GUILayout.BeginVertical("box");
            serializedObject.Update();
            ShowElements(serializedObject.FindProperty("objectsPrefabs"));
            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(3);

            EditorGUI.BeginChangeCheck();
            count = EditorGUILayout.IntField("Objects Count", master.ObjectsCount);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(master, "Variable change");
                EditorUtility.SetDirty(master);
                master.ObjectsCount = count;
            }

            GUILayout.Space(5);
            master.showRotationsOptions = EditorGUILayout.Foldout(master.showRotationsOptions, "Adjust Rotation");
            if (master.showRotationsOptions)
            {
                // EditorGUI.indentLevel += 1;
                GUILayout.Space(5);

                EditorGUI.BeginChangeCheck();
                Vector3 rotation = EditorGUILayout.Vector3Field("Rotation", master.addRotation.eulerAngles);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(master, "Variable change");
                    EditorUtility.SetDirty(master);
                    master.addRotation = Quaternion.Euler(rotation);
                }
                GUILayout.Space(5);
                EditorGUI.BeginChangeCheck();
                bool applyRotationX = EditorGUILayout.Toggle(new GUIContent("Apply Rotation X", "Apply X component of Rotation. Affects randomization"), master.ApplyRotationX);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(master, "Variable change");
                    EditorUtility.SetDirty(master);
                    master.ApplyRotationX = applyRotationX;
                }

                EditorGUI.BeginChangeCheck();
                bool applyRotationY = EditorGUILayout.Toggle(new GUIContent("Apply Rotation Y", "Apply Y component of Rotation. Affects randomization"), master.ApplyRotationY);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(master, "Variable change");
                    EditorUtility.SetDirty(master);
                    master.ApplyRotationY = applyRotationY;
                }

                EditorGUI.BeginChangeCheck();
                bool applyRotationZ = EditorGUILayout.Toggle(new GUIContent("Apply Rotation Z", "Apply Z component of Rotation. Affects randomization"), master.ApplyRotationZ);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(master, "Variable change");
                    EditorUtility.SetDirty(master);
                    master.ApplyRotationZ = applyRotationZ;
                }

               
               // EditorGUI.indentLevel -= 1;
            }
            GUILayout.Space(5);
            master.showObjRandomisationOptions = EditorGUILayout.Foldout(master.showObjRandomisationOptions, "Randomise");
            if (master.showObjRandomisationOptions)
            {
                EditorGUI.indentLevel += 1;

                EditorGUI.BeginChangeCheck();
                bool randomise = EditorGUILayout.Toggle(new GUIContent("Randomise", "Add some random offsets/scales/rotations"), master.randomise);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(master, "Variable change");
                    EditorUtility.SetDirty(master);
                    master.randomise = randomise;
                    master.UpdateMaster(true);
                }

                if (randomise)
                {


                    EditorGUI.BeginChangeCheck();
                    Vector3 MaxOffset = EditorGUILayout.Vector3Field("Max Possition Offset", master.offsetRandomMaximum);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(master, "Variable change");
                        EditorUtility.SetDirty(master);
                        master.offsetRandomMaximum = MaxOffset;

                        if (master.autoUpdate)
                            master.UpdateMaster(true);

                    }

                    EditorGUI.BeginChangeCheck();
                    Vector3 MaxRotation = EditorGUILayout.Vector3Field("Max Rotation Offset", master.rotationRandomMaximum);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(master, "Variable change");
                        EditorUtility.SetDirty(master);
                        master.rotationRandomMaximum = MaxRotation;

                        if (master.autoUpdate)
                            master.UpdateMaster(true);
                    }
                    EditorGUI.BeginChangeCheck();
                    Vector3 MaxScale = EditorGUILayout.Vector3Field("Max Scale Offset", master.scaleRandomMaximum);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(master, "Variable change");
                        EditorUtility.SetDirty(master);
                        master.scaleRandomMaximum = MaxScale;
                        if (master.autoUpdate)
                            master.UpdateMaster(true);
                    }
                }

                EditorGUI.indentLevel -= 1;
            }
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Detach Objects", "Detach objects and break their script link")))
            {
                Undo.RecordObject(master, "Detach Objects");
                EditorUtility.SetDirty(master);
                master.DetachObjects();
                master.usingOfSpline = Using.None;
            }

            GUILayout.EndVertical();
        }

        private void MeshCreateUI()
        {
            GUILayout.BeginVertical("box");

            GUILayout.Space(5);

            master.meshType = (MeshCreatorType)EditorGUILayout.EnumPopup("Mesh Type", master.meshType);

            GUILayout.Space(5);


            CreateMechCreatorEditor();


            if (meshCreatorEditor != null)
            {
                meshCreatorEditor.OnInspectorGUI();
            }
           

            GUILayout.Space(5);

            if (GUILayout.Button(new GUIContent("Create Mesh Prefab", "Saves mesh asset in Assets folder")))
                CreateAsset(master.meshCreator.CreateMesh());

           //GUILayout.Space(5);

            if (GUILayout.Button(new GUIContent("Detach Mesh", "Detach ready mesh as new object and break script link")))
            {
                Undo.RecordObject(master, "Detach Objects");
                EditorUtility.SetDirty(master);
                master.DetachObjects();
                master.usingOfSpline = Using.None;
            }
            GUILayout.Space(5);

            //  GUILayout.EndVertical();

            //GUILayout.Label("");

            //  GUILayout.BeginVertical("box");
            GUILayout.Label("Vertex Count: " + master.verticesCount + "\n" + "Triangles Count: " + master.trianglesCount, EditorStyles.helpBox, GUILayout.ExpandWidth(true));
           // GUILayout.Label("Vertex Count: " + master.verticesCount);
           // GUILayout.Label("Triangles Count: " + master.trianglesCount);
            GUILayout.EndVertical();
        }
    
        private void Animation()
        {
            GUILayout.Label("Now you can animate spline points and mesh creator variables through script! See how in Example folder", EditorStyles.helpBox, GUILayout.ExpandWidth(true));
            if (!master.autoUpdate)
            {
                
                GUILayout.Label("Don't forget enable Auto Update toggle", EditorStyles.helpBox, GUILayout.ExpandWidth(true));
            }
          
        }

        private static void ShowElements(SerializedProperty list)
        {
            EditorGUILayout.PropertyField(list);

            if (list.arraySize == 0)
                list.arraySize = 1;

            if (list.GetArrayElementAtIndex(0) == null)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Add prefab!");

                GUILayout.Space(20);
                GUILayout.EndVertical();
            }

            if (list.isExpanded)
            {
                EditorGUI.indentLevel += 1;


                for (int i = 0; i < list.arraySize; i++)
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent("Object " + (i + 1)));




                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(0), GUIContent.none);
            }
        }

        #region SCENE VIEW MANIPULATORS

        private void OnSceneGUI()
        {
            if (!master.showCurveEditor)
                return;

            if (spline == null)
                spline = master.spline;

            if (spline == null)
                return;

                handleTransform = master.transform;

            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for (int i = 1; i < spline.ControlPointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector3 p3 = ShowPoint(i + 2);

                Handles.color = modeColors[(int)spline.GetControlPointMode(i)];
                Handles.DrawLine(p0, p1);
                Handles.color = modeColors[(int)spline.GetControlPointMode(i + 2)];
                Handles.DrawLine(p2, p3);
                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }

            
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(spline.GetControlPointPosition(index));
            float size = HandleUtility.GetHandleSize(point);

            if (index % 3 == 0)
                size *= (handleSize * 1.5f);
            else
                size *= handleSize;

            EditorGUI.BeginChangeCheck();

            Handles.color = modeColors[(int)spline.GetControlPointMode(index)];



            if (Handles.Button(point, handleRotation, size, size, Handles.SphereHandleCap))
            {
                splineEditor.selectedIndex = index;

            }

            if (index == splineEditor.selectedIndex)
            {
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {


                    Undo.RecordObject(spline, "Move Point");
                    EditorUtility.SetDirty(spline);
                    spline.SetControlPointPosition(index, handleTransform.InverseTransformPoint(point));


                }
                Repaint();
            }
            return point;
        }

        private void ShowVelocity()
        {
            Handles.color = Color.green;
            Vector3 point = master.transform.TransformPoint(spline.GetPointOnCurve(0f));
            Handles.DrawLine(point, point + spline.GetVelocity(0f) * directionScale);

            int ls = lineSteps * spline.ControlPointCount;
            for (int i = 1; i <= ls; i++)
            {
                point = master.transform.TransformPoint(spline.GetPointOnCurve(i / (float)ls));
                Handles.DrawLine(point, point + spline.GetVelocity(i / (float)ls) * directionScale);
            }
        }

        #endregion

       
    }
}
    
