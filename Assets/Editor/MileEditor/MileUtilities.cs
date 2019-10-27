using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace MileCode.MileTest {
    public static class MileUtilities {
        private static void CreateNewScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }

        private static void CleanScene() {
            GameObject[] allObjs = Object.FindObjectsOfType<GameObject>();
            foreach(GameObject obj in allObjs) {
                GameObject.DestroyImmediate(obj);
            }
        }

        public static void NewScene() {
            CreateNewScene();
            //CleanScene();
            GameObject tempObj = new GameObject("Temp");
            tempObj.transform.position = Vector3.zero;
     
        }
    }

}