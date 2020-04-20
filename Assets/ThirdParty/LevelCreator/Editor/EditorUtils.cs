using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

// i create C here
namespace RunRun.LevelCreator {
    public static class EditorUtils {
        // check if current scene needs to be saved.
        public static void CreateNewScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        // remove every gameobject in the scene
        public static void CleanScene() {
            GameObject[] allObjs = Object.FindObjectsOfType<GameObject>();
            foreach(GameObject obj in allObjs) {
                GameObject.DestroyImmediate(obj);
            }
        }

        // actual method needs to be called.
        public static void NewLevel() {
            CreateNewScene();
            CleanScene();
            GameObject levelInScene = new GameObject("Level");
            levelInScene.transform.position = Vector3.zero;
            levelInScene.AddComponent<MileCode.MileTest.MyLevel>();
        }
    }
}