using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TerrainAnalyzer {
    Terrain terrain;
    TerrainData terrainData;
    TerrainLayer[] terrainLayers;
    public TerrainAnalyzer(Terrain terrain) {
        this.terrain = terrain;
        this.terrainData = terrain.terrainData;
        this.terrainLayers = this.terrainData.terrainLayers;
    }

    public int GetTerrainLayersCounts() {
        Debug.Log(this.terrainData.terrainLayers.Length);
        return this.terrainData.terrainLayers.Length;
    }

    public void GetTerrainSplatMaps() {
        Texture2D[] splatMaps = this.terrainData.alphamapTextures;
        if(splatMaps.Length <= 0) {
            Debug.Log("Can't find any splat map");
            return;
        } else {
            foreach(Texture2D splatMap in splatMaps) {
                string path = Application.dataPath + "/MileMisc/" + this.terrain.name + "_" + splatMap.name + ".png";
                SaveTextureOnDisk(splatMap, path);
            }
            
        }
    }

    private Texture2D GetTextureFromLayers(TerrainLayer[] terrainLayers, int index) {
        return terrainLayers[index].diffuseTexture;
    }

    private void SaveTextureOnDisk(Texture2D texture, string path) {
        Byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    public void GetTerrainLayers() {
        Texture2D[] splatMaps = this.terrainData.alphamapTextures;
        if(splatMaps.Length <= 0) {
            Debug.Log("Can't find any splat map.");
            return;
        } else {
            for(int i = 0; i < this.terrainLayers.Length; i++) {
                Texture2D diffuse = this.terrainLayers[i].diffuseTexture;
                int desiredChannel = GetDesiredChannelViaLayerOrder(i);
                int desiredSplat = GetDesiredSplatViaLayerOrder(i);
                //Debug.Log(desiredChannel);
                Texture2D diffuseWithSplat = CombineDiffuseSplat(diffuse, splatMaps[desiredSplat], desiredChannel);
            }
        }
    }

    Texture2D CombineDiffuseSplat(Texture2D diffuse, Texture2D splat, int channel) {
        return diffuse;

    }

    int GetDesiredChannelViaLayerOrder(int index) {
        return (index % 4) + 1;
        
    }
    int GetDesiredSplatViaLayerOrder(int index) {
        if(Mathf.FloorToInt((index / 4)) == 0) {
            return 0;
        } else if(Mathf.FloorToInt(index / 4) == 1) {
            return 1;
        } else {
            return -1;
        }

    }
}
