using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MileCode {
    [ExecuteInEditMode]
    public class LBP_Settings : MonoBehaviour {
        [SerializeField]
        int lightmapIndex;
        [SerializeField]
        Vector4 lightmapScaleAndOffset = new Vector4(1, 1, 0, 0);
        [SerializeField]
        Texture2D lightmap;
        private Renderer renderer;
        private MaterialPropertyBlock propBlock;

        void SetLightmapValues(int index, Vector4 scaleAndOffset) {
            this.lightmapIndex = index;
            this.lightmapScaleAndOffset = scaleAndOffset;
        }

        private void Awake() {
            propBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
        }

        private void Update() {
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetVector("_Lightmap_ST", lightmapScaleAndOffset);
            propBlock.SetTexture("_Lightmap", lightmap);
            renderer.SetPropertyBlock(propBlock);
        }
    }
}