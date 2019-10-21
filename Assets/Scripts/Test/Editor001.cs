using UnityEngine;
using UnityEditor;
public class Editor001 {
    [MenuItem("Help/Create Editor001")]
    private static void CreateEditor001() {
        if (EditorUtility.DisplayDialog(
                "This is Title",
                "This is message",
                "ComfirmButton",
                "CancelButton"
            )) {
            GameObject wazzup =  new GameObject("Mankind.");        // A new Gameobject named mankind will be generated.
            Debug.Log(wazzup);
            wazzup.tag = "Player";      // Can't add non - exsiting tag
         

        }
    }
}