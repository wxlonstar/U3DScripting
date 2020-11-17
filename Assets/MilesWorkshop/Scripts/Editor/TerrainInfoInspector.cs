using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainInfo))]
public class TerrainInfoInspector : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        Terrain mainTerrain = Terrain.activeTerrain;
        TerrainAnalyzer ta;
        if(mainTerrain == null) {
            Debug.Log("can't find main terrain.");
            return;
        } else {
            ta = new TerrainAnalyzer(mainTerrain);
        }
        if(GUILayout.Button("Check")) {
            ta.GetTerrainSplatMaps();
            
        }
    }
}
