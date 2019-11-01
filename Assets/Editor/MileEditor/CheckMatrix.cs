using UnityEditor;
using UnityEngine;

namespace MileCode.MileTest {
    [CustomEditor(typeof(UsingMatrixTransformation))]
    public class CheckMatrix: Editor {
        
        

        private void OnEnable() {
            Info();
        }

        void Info() {
            Transform selected = Selection.activeTransform;
            // Debug.Log("ObjectName: " + selected.name + ". Position: " + selected.position);
        }

        [MenuItem("Help/TransformTo")]
        static void TransformationMatrix() {
            Transform selected = Selection.activeTransform;
            Transform selectedParent = Selection.activeTransform.root;
            Vector3 posWorld = selectedParent.localToWorldMatrix.MultiplyPoint(selected.localPosition);
            Debug.Log(posWorld);
          

        }

        


        

    }
}
