using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MileCode {
    public class MySceneState : Editor {
        [MenuItem("MileEditor/MySceneState")]
        public static void SetSceneState() {
            SetDirectionalLight();
            SetEnvironment();
        }

        static void SetDirectionalLight() {
            var directionalLight = GameObject.Find("Directional Light");
            if(directionalLight != null) {
                Light light = directionalLight.GetComponent<Light>();
                light.lightmapBakeType = LightmapBakeType.Mixed;
                //Debug.Log("Directonal Light now is " + light.lightmapBakeType.ToString());
            }
        }

        static void SetEnvironment() {
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;
            LightmapEditorSettings.bakeResolution = 20;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            Debug.Log(LightmapEditorSettings.lightmapper.ToString());
        }
    }
}
