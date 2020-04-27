using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LightmapUV_LBP : MonoBehaviour {
    [SerializeField]
    int lightmapIndex;
    [SerializeField]
    Vector4 lightmapScaleAndOffset;
    void SetLightmapValues(int index, Vector4 scaleAndOffset) {
        this.lightmapIndex = index;
        this.lightmapScaleAndOffset = scaleAndOffset;
    }

    private void Start() {
        Vector4 defaultValue = new Vector4(1, 1, 0, 0);
        SetLightmapValues(0, defaultValue);
    }

    private void Update() {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial.SetVector("_Lightmap_ST", lightmapScaleAndOffset);
    }

}
