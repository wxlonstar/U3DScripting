using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MileCode {
    public class SceneViewTest : Editor {
        [MenuItem("MileTool/SceneView01")]
        public static void fun1() {
            DrawCameraMode drawMode = DrawCameraMode.Wireframe;
            //SceneView.lastActiveSceneView.cameraMode = SceneView.GetBuiltinCameraMode(DrawCameraMode.Normal);
            SceneView sv = SceneView.lastActiveSceneView;
            if(sv != null) {
                Debug.Log(sv.cameraMode.drawMode.ToString());
            }
        }

    }
}
