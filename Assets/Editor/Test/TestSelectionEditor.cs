using UnityEditor;
using UnityEngine;


namespace MileCode.MileTest {
    [CustomEditor(typeof(TestSelection))]
    //[CanEditMultipleObjects]
    public class SeeSelection : Editor {
        public override void OnInspectorGUI() {
            Transform selected = Selection.activeTransform;
            Debug.Log(selected.name);
        }
    }
    
}