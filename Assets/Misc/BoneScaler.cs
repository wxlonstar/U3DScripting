using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScaler {
    static BoneSize boneSizeData;
    static SkinnedMeshRenderer skinnedMeshRendererData;
    static bool isIntialized = false;

    public static void Initialization( ScriptableObject boneSize, SkinnedMeshRenderer skinnedMeshRenderer) {
        if(boneSize == null || skinnedMeshRenderer == null) {
            return;
        }
        boneSizeData = boneSize as BoneSize;
        skinnedMeshRendererData = skinnedMeshRenderer;
        isIntialized = true;
    }

    public static void DeformToFat() {
        if(isIntialized) {
            return;
        }
        Debug.Log("BoneSizeData: " + boneSizeData.HeadSize.ToString());
    }


}
