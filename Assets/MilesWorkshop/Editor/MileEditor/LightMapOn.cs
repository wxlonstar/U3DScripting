using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MileCode.LightingSettings {
    
    public class LightMapOn {
        static LightmapData[] currentLightmaps = new LightmapData[] { };
        private Material currentMat;

        /*
        [MenuItem("Help/LightMap Off")]
        private static void TurnOff() {
            currentLightmaps = LightmapSettings.lightmaps;
            LightmapSettings.lightmaps = new LightmapData[] { };
            foreach(LightmapData ld in currentLightmaps) {
                
            }

        }

        [MenuItem("Help/LightMap On")]
        private static void TurnOn() {
            if (currentLightmaps.Length <=0) {
                Debug.Log("Can't find lightmaps");
                return;
            }
            LightmapSettings.lightmaps = currentLightmaps;

        }
        */
        [MenuItem("Lightmap/不显示")]
        private static void LightmapHidden() {
            currentLightmaps = LightmapSettings.lightmaps;
            Debug.Log(currentLightmaps.Length + " 张灯光贴图.");
            Lightmapping.Clear();
            //LightmapSettings.lightmaps = currentLightmaps;
           
            //EditorSceneManager.LoadScene()
            /*
            Lightmapping.Clear();
            foreach (LightmapData ld in currentLightmaps) {
                
            }
            */
            //Lightmapping.ClearLightingDataAsset();
        }

        [MenuItem("Lightmap/显示")]
        private static void LightmapShow() {
            if(currentLightmaps.Length  > 0) {
                LightmapSettings.lightmaps = currentLightmaps;
            }

            //EditorSceneManager.GetActiveScene();
            //Lightmapping.lightingDataAsset = Resources.Load("Map 1 - Sun/LightingData", typeof(LightingDataAsset)) as LightingDataAsset;

            //Lightmapping.ClearLightingDataAsset();
        }



        [MenuItem("Lightmap/清除掉")]
        private static void LightmapClear() {
      
            Lightmapping.Clear();
            Lightmapping.ClearDiskCache();
            Lightmapping.ClearLightingDataAsset();
        }

        [MenuItem("Lightmap/烘焙测试")]
        private static void LightmapTest() {
            LightmapClear();
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.prioritizeView = true;
            LightmapEditorSettings.directSampleCount = 8;
            LightmapEditorSettings.indirectSampleCount = 16;
            LightmapEditorSettings.bounces = 2;
            LightmapEditorSettings.filteringMode = LightmapEditorSettings.FilterMode.None;
            LightmapEditorSettings.bakeResolution = 20;
            LightmapEditorSettings.maxAtlasSize = 512;
            LightmapEditorSettings.textureCompression = true;
            LightmapEditorSettings.enableAmbientOcclusion = false;
            LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;
            
            Debug.Log("烘焙测试中....");
            //Lightmapping.BakeAsync();
        }

        [MenuItem("Lightmap/烘焙全部")]
        private static void LightmapAll() {
            LightmapClear();
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.prioritizeView = true;
            LightmapEditorSettings.directSampleCount = 16;
            LightmapEditorSettings.indirectSampleCount = 64;
            LightmapEditorSettings.bounces = 2;
            LightmapEditorSettings.filteringMode = LightmapEditorSettings.FilterMode.Auto;
            LightmapEditorSettings.bakeResolution = 30;
            LightmapEditorSettings.textureCompression = true;
            LightmapEditorSettings.enableAmbientOcclusion = true;
            LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;

            Debug.Log("烘焙全部中....");
            //Lightmapping.BakeAsync();
        }

        [MenuItem("Lightmap/烘焙质量")]
        private static void LightmapQuality() {
            LightmapClear();
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            LightmapEditorSettings.prioritizeView = true;
            LightmapEditorSettings.directSampleCount = 64;
            LightmapEditorSettings.indirectSampleCount = 512;
            LightmapEditorSettings.bounces = 2;
            LightmapEditorSettings.filteringMode = LightmapEditorSettings.FilterMode.Advanced;
            LightmapEditorSettings.bakeResolution = 40;
            LightmapEditorSettings.textureCompression = true;
            LightmapEditorSettings.enableAmbientOcclusion = true;
            LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;

            Debug.Log("烘焙质量中....");
            //Lightmapping.BakeAsync();
        }
    }
}