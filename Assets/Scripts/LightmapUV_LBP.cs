using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapUV_LBP : MonoBehaviour {
    [SerializeField]
    int lightmapIndex;
    [SerializeField]
    Vector4 lightmapScaleAndOffset;
    void SetLightmapValues(int index, Vector4 scaleAndOffset) {
        this.lightmapIndex = index;
        this.lightmapScaleAndOffset = scaleAndOffset;
    }
   
}
