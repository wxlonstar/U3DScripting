using UnityEngine;
using UnityEditor;

namespace MileCode.MileTest {
    class MileEditor {
        [MenuItem("MileEditor/TestController")]
        private static void OpenScene() {
            MileUtilities.NewScene();
        }
    }
}
