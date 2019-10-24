using UnityEngine;      // marked with these directives will not be included when building
#if UNITY_EDITOR
using UnityEditor;      //Editor class must be in Editor folder so, because namespace UnityEditor is not avialable when app is being built.
#endif
public class HelloEditor1 {
#if UNITY_EDITOR
    [MenuItem("Help/HelloEditor1")]
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
#endif
}