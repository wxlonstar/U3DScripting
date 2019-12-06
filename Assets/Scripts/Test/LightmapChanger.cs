using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.IO;
using System.Collections.Generic;

public class LightmapChanger : MonoBehaviour {
    private string _JsonFileName = "lightmapConfiguration.txt";
    [SerializeField]
    private string m_resourceFolder = "LightMapData_1";
    public string resourceFolder {
        get {
            return m_resourceFolder;
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
        if(!IsLightmpaDirectoryExists(m_resourceFolder)) {
            Directory.CreateDirectory(GetLightmapsDirectory(dir));
        }
    }

    public void Load(string folderName) {
        m_resourceFolder = folderName;
        Load();
    }

    // load everything
    public void Load() {
        lightingScenarioData = LoadJsonData();
        var newLightmaps = new LightmapData[lightingScenarioData.lightmaps.Length];
        for(int i = 0; i < newLightmaps.Length; i++) {
            newLightmaps[i] = new LightmapData();
            //newLightmaps[i].lightmapLight = Resources.Load<Texture2D>(m_resourceFolder + "/" + lightingScenarioData.lightmaps[i].name);
            newLightmaps[i].lightmapColor = Resources.Load<Texture2D>(m_resourceFolder + "/" + lightingScenarioData.lightmaps[i].name);
            if(lightingScenarioData.lightmapsMode != LightmapsMode.NonDirectional) {
                newLightmaps[i].lightmapDir = Resources.Load<Texture2D>(m_resourceFolder + "/" + lightingScenarioData.lightmaps_Dir[i].name);
                if(lightingScenarioData.lightmaps_Shadow[i] != null) {      // if the texture exists 
                    newLightmaps[i].shadowMask = Resources.Load<Texture2D>(m_resourceFolder + "/" + lightingScenarioData.lightmaps_Shadow[i].name);
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
        absoluteName = GetLightmapsDirectory(m_resourceFolder) + _JsonFileName;
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
                    info.lightmapIndex = newLightmapsTexturesDir.IndexOf(lightmapDir);
                    if(info.lightmapIndex == -1) {
                        info.lightmapIndex = newLightmapsTexturesDir.Count;
                        newLightmapsTexturesDir.Add(lightmapDir);
                    }
                    Texture2D lightmapShadow = LightmapSettings.lightmaps[mr.lightmapIndex].shadowMask;
                    info.lightmapIndex = newLightmapsTexturesShadow.IndexOf(lightmapShadow);
                    if(info.lightmapIndex == -1) {
                        info.lightmapIndex = newLightmapsTexturesShadow.Count;
                        newLightmapsTexturesShadow.Add(lightmapShadow);
                    }
                }
                newRendererInfos.Add(info);
            }
        }
        lightingScenarioData.lightmapsMode = newLightmapsMode;
        lightingScenarioData.lightmaps = newLightmapsTextures.ToArray();
        if(newLightmapsMode != LightmapsMode.NonDirectional) {
            lightingScenarioData.lightmaps_Dir = newLightmapsTexturesDir.ToArray();
            lightingScenarioData.lightmaps_Shadow = newLightmapsTexturesShadow.ToArray();
        }
        lightingScenarioData.rendererInfos = newRendererInfos.ToArray();
        var scene_LightProbes = new SphericalHarmonicsL2[LightmapSettings.lightProbes.bakedProbes.Length];
        scene_LightProbes = LightmapSettings.lightProbes.bakedProbes;
        for(int i = 0; i < scene_LightProbes.Length; i++) {
            var SHCoeff = new SphericalHarmonics();
            for(int j = 0; j < 3; j++) {
                for(int k = 0; k <9; k++) {
                    SHCoeff.probeCoefficient[j * 9 + k] = scene_LightProbes[i][j, k];
                }
            }
            newSphericalHarmonicsList.Add(SHCoeff);
        }
        lightingScenarioData.lightProbes = newSphericalHarmonicsList.ToArray();
        CreateLightmapDirectory(m_resourceFolder);
        string resourcesDir = GetLightmapsDirectory(m_resourceFolder);
        CopyTextureToResource(resourcesDir, lightingScenarioData.lightmaps);
        CopyTextureToResource(resourcesDir, lightingScenarioData.lightmaps_Dir);
        CopyTextureToResource(resourcesDir, lightingScenarioData.lightmaps_Shadow);
        string jsonContent = JsonUtility.ToJson(lightingScenarioData);
        WriteJsonFile(resourcesDir, jsonContent);
    }
    private void CopyTextureToResource(string toPath, Texture2D[] textures) {
        for(int i = 0; i < textures.Length; i++) {
            Texture2D texture = textures[i];
            if(texture != null) {
                FileUtil.ReplaceFile(AssetDatabase.GetAssetPath(texture), toPath + Path.GetFileName(AssetDatabase.GetAssetPath(texture)));
                AssetDatabase.Refresh();
                Texture2D newTexture = Resources.Load<Texture2D>(m_resourceFolder + "/" + texture.name);
                CopyTextureImporterProperties(textures[i], newTexture);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newTexture));
                EditorUtility.CompressTexture(newTexture, textures[i].format, UnityEditor.TextureCompressionQuality.Best);
                textures[i] = newTexture;
            }
        }
    }

    private void CopyTextureImporterProperties(Texture2D fromTexture, Texture2D toTexture) {
        TextureImporter fromTextureImporter = GetTextureImporter(fromTexture);
        TextureImporter toTextureImporter = GetTextureImporter(toTexture);
        toTextureImporter.wrapMode = fromTextureImporter.wrapMode;
        toTextureImporter.anisoLevel = fromTextureImporter.anisoLevel;

    }

    private TextureImporter GetTextureImporter(Texture2D texture) {
        string newTexturePath = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(newTexturePath) as TextureImporter;
        return importer;
    }
}
