using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MileCode {
    public class GetLightMapInfo {
        static LightmapData[] currentLightmaps = new LightmapData[] { };

        [MenuItem("Lightmap/Info")]
        private static void LightmapInfo() {
            currentLightmaps = LightmapSettings.lightmaps;
            Debug.Log(currentLightmaps.Length + " 张灯光贴图");
        }
    }
}
