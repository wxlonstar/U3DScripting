using UnityEngine;
using UnityEditor;      //Editor class must be in Editor folder so, because namespace UnityEditor is not avialable when app is being built.

public class HelloEditor1 {
    [MenuItem("Help/HelloEditor")]
    private static void FirstClick() {
        if (EditorUtility.DisplayDialog(
                "MyTitle",
                "MyMsg",
                "Yeah",
                "Nah"
            )
            ) {
            new GameObject("YouGotMe");

        }
    }
}