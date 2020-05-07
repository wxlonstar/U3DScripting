using UnityEngine;
using UnityEditor;
namespace MileCode.MileTest {
    public class UseGizmoEditor {
        [MenuItem("MileEditor/GizmoInEditor")]
        private static void Hey() {
            DrawOneGizmo();
            Debug.Log("Hey");
        }

        private static void DrawOneGizmo() {
            GameObject temp = new GameObject("Temp");
            temp.AddComponent<MyGizmoStyle>();      // If MyGizmoStyle extends MonoBehaviar and in Editor Folder, appliction will not run correctly.
            
        }
    }
    /* A class here runs correctly
    class MyGizmoStyle : MonoBehaviour {        // cllass must be in here or outside the editro folder
        private void OnDrawGizmos() {
            Debug.Log("wazzup!");
        }
    }
    */


}
