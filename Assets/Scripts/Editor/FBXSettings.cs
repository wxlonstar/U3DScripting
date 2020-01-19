using UnityEditor;
using UnityEngine;


public class FBXSettings :  Editor {
    [MenuItem("Assets/MyFBX/HuiYuanStyle")]
    public static void HuiYuanStyleMulti() {
        Object[] objs = Selection.objects;
        if(objs[0].name != "Assets") {
            foreach(GameObject go in objs) {
                HuiYuanStyleSolo(go);
            }
        }else {
            Debug.Log("Nothing's selected.");
        }
        
    }
    public static void HuiYuanStyleSolo(GameObject go) {
        //GameObject obj = Selection.activeObject as GameObject;
        string path = AssetDatabase.GetAssetPath(go);
        ModelImporter mi = AssetImporter.GetAtPath(path) as ModelImporter;
        mi.materialImportMode = ModelImporterMaterialImportMode.None;
        mi.importCameras = false;
        mi.importLights = false;
        mi.importBlendShapes = false;
        mi.importVisibility = false;
        mi.sortHierarchyByName = false;
        mi.animationType = ModelImporterAnimationType.None;
        mi.importAnimation = false;
    }

    [MenuItem("Assets/MyFBX/Default", priority = 0)]
    public static void DefaultStyleMulti() {
        Object[] objs = Selection.objects;
        if (objs[0].name != "Assets") {
            foreach (GameObject go in objs) {
                DefaultStyleSolo(go);
            }
        } else {
            Debug.Log("Nothing's selected.");
        }
    }
    public static void DefaultStyleSolo(GameObject go) {
        //GameObject obj = Selection.activeObject as GameObject;
        string path = AssetDatabase.GetAssetPath(go);
        ModelImporter mi = AssetImporter.GetAtPath(path) as ModelImporter;
        mi.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
        mi.importCameras = true;
        mi.importLights = true;
        mi.importBlendShapes = true;
        mi.importVisibility = true;
        mi.sortHierarchyByName =true;
        mi.animationType = ModelImporterAnimationType.Generic;
        //mi.importAnimation =
        mi.importAnimation = true;
    }


}
