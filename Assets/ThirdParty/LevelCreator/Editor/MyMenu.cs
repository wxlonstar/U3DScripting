using UnityEngine;
using UnityEditor;

//I create all the V here
namespace RunRun.LevelCreator {
    public static class MyMenu {
        [MenuItem("Tools/New Scene")]
        private static void CreateNewLevel() {
            EditorUtils.NewLevel();
        }
        
    }

   
}