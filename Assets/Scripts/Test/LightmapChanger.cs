using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.IO;
using System.Collections.Generic;

public class LightmapChanger : MonoBehaviour {
    private string _JsonFileName = "lightmapConfiguration.txt";
    [SerializeField]
    private string map_resourceFolder = "LightMapData_1";
    public string resourceFolder {
        get {
            return map_resourceFolder;
        }
    }

    private string absoluteName;
    
    [System.Serializable]
    private class SphericalHarmonics {
        public float[] probeCoefficient = new float[27];        // why 27?
    }

    [System.Serializable]
    private class RendererInfo {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;     //UV scale & offset used for lightmap
    }

    [System.Serializable]
    private class LightingScenarioData {
        public RendererInfo[] rendererInfos;
        public Texture2D[] lightmaps;
        public Texture2D[] lightmaps_Dir;
        public Texture2D[] lightmaps_Shadow;
        public LightmapsMode lightmapsMode;
        public SphericalHarmonics[] lightProbes;
        
    }

    [SerializeField]
    private LightingScenarioData lightingScenarioData;

    [SerializeField]
    private bool loadOnAwake = false;       //load selected lightmaps when this script wakes up

    [SerializeField]
    private bool verbose = false;       // enable logs, confusing

    public static LightmapChanger instance;

    public string GetLightmapsDirectory(string dir) {
        return Application.dataPath + "Resources/" + dir + "/";     //lightmap directory
    }

    public bool IsLightmpaDirectoryExists(string dir) {
        return Directory.Exists(GetLightmapsDirectory(dir));
    }

    private void CreateLightmapDirectory(string dir) {
        if(!IsLightmpaDirectoryExists(map_resourceFolder)) {
            Directory.CreateDirectory(GetLightmapsDirectory(dir));
        }
    }

    public void Load(string folderName) {
        map_resourceFolder = folderName;
        Load();
    }

    // load everything
    public void Load() {
        lightingScenarioData = LoadJsonData();
        var newLightmaps = new LightmapData[lightingScenarioData.lightmaps.Length];
        for(int i = 0; i < newLightmaps.Length; i++) {
            newLightmaps[i] = new LightmapData();
            //newLightmaps[i].lightmapLight = Resources.Load<Texture2D>(map_resourceFolder + "/" + lightingScenarioData.lightmaps[i].name);
            newLightmaps[i].lightmapColor = Resources.Load<Texture2D>(map_resourceFolder + "/" + lightingScenarioData.lightmaps[i].name);
            if(lightingScenarioData.lightmapsMode != LightmapsMode.NonDirectional) {
                newLightmaps[i].lightmapDir = Resources.Load<Texture2D>(map_resourceFolder + "/" + lightingScenarioData.lightmaps_Dir[i].name);
                if(lightingScenarioData.lightmaps_Shadow[i] != null) {      // if the texture exists 
                    newLightmaps[i].shadowMask = Resources.Load<Texture2D>(map_resourceFolder + "/" + lightingScenarioData.lightmaps_Shadow[i].name);
                }
            }
        }

        LoadLightProbes();
        ApplyRendererInfo(lightingScenarioData.rendererInfos);
        LightmapSettings.lightmaps = newLightmaps;
    }

    private void LoadLightProbes() {
        var sphericalHarmonicsArray = new SphericalHarmonicsL2[lightingScenarioData.lightProbes.Length];
        for(int i = 0; i < lightingScenarioData.lightProbes.Length; i++) {
            var sphericalHarmonics = new SphericalHarmonicsL2();
            //
            for(int j = 0; j < 3; j++) {
                //
                for(int k = 0; k < 9; k++) {
                    sphericalHarmonics[j, k] = lightingScenarioData.lightProbes[i].probeCoefficient[j * 9 + k];
                }
            }

            sphericalHarmonicsArray[i] = sphericalHarmonics;
        }

        try {
            LightmapSettings.lightProbes.bakedProbes = sphericalHarmonicsArray;
        }catch {
            Debug.LogWarning("Warning, error when trying to load lightprobes.");
        }
    }

    private void ApplyRendererInfo(RendererInfo[] infos) {
        try {
            for(int i = 0; i < infos.Length; i++) {
                var info = infos[i];
                info.renderer.lightmapIndex = infos[i].lightmapIndex;
                if(!info.renderer.isPartOfStaticBatch) {
                    info.renderer.lightmapScaleOffset = infos[i].lightmapOffsetScale;
                }
                if(info.renderer.isPartOfStaticBatch && verbose == true) {
                    Debug.Log("Object " + info.renderer.gameObject.name + " is part of static batch, lightmap ST skipped.");
                }
            }
        }catch(Exception e) {
            Debug.LogError("Errors when apply renderInfo: " + e.GetType().ToString());
        }
    }

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        }else {
            instance = this;
        }
        if(loadOnAwake) {
            Load();
        }
    }

    private void WriteJsonFile(string path, string content) {
        absoluteName = path + _JsonFileName;
        File.WriteAllText(absoluteName, content);
    }

    private string ReadJsonFile(string file) {
        if(!File.Exists(file)) {
            return "";
        }
        return File.ReadAllText(file);
    }

    private LightingScenarioData LoadJsonData() {
        absoluteName = GetLightmapsDirectory(map_resourceFolder) + _JsonFileName;
        string jsonContent = ReadJsonFile(absoluteName);
        return lightingScenarioData = JsonUtility.FromJson<LightingScenarioData>(jsonContent);
    }

    public void GenerateStoredLightmapInfo() {
        lightingScenarioData = new LightingScenarioData();
        var newRendererInfos = new List<RendererInfo>();
        var newLightmapsTextures = new List<Texture2D>();
        var newLightmapsTexturesDir = new List<Texture2D>();
        var newLightmapsTexturesShadow = new List<Texture2D>();
        var newLightmapsMode = new LightmapsMode();
        var newSphericalHarmonicsList = new List<SphericalHarmonics>();

        newLightmapsMode = LightmapSettings.lightmapsMode;
        var renderers = FindObjectsOfType(typeof(MeshRenderer));
        Debug.Log("stored info for " + renderers.Length + " MeshRenders");
        foreach(MeshRenderer mr in renderers) {
            if(mr.lightmapIndex != -1) {
                RendererInfo info = new RendererInfo();
                info.renderer = mr;
                info.lightmapOffsetScale = mr.lightmapScaleOffset;

                Texture2D lightmapColor = LightmapSettings.lightmaps[mr.lightmapIndex].lightmapColor;
                info.lightmapIndex = newLightmapsTextures.IndexOf(lightmapColor);
                if(info.lightmapIndex == -1) {
                    info.lightmapIndex = newLightmapsTextures.Count;
                    newLightmapsTextures.Add(lightmapColor);
                }
                if(newLightmapsMode != LightmapsMode.NonDirectional) {
                    Texture2D lightmapDir = LightmapSettings.lightmaps[mr.lightmapIndex].lightmapDir;
                }
            }
        }
    }
}
