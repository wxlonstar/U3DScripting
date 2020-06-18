using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MileCode {
    [CustomEditor(typeof(MeshInfo))]
    public class MeshInfoEditor : Editor {
        private void OnEnable() {
            //Debug.Log("Checking..");
        }

        private void OnDisable() {
            if(Selection.activeGameObject == null) {
                MeshInfo mi = (MeshInfo)target;
                mi.ResetMesh();
            }
        }

        private void OnSceneGUI() {
            MeshInfo mi = (MeshInfo)target;
            Handles.BeginGUI();
            {
                GUIStyle boxStyle = new GUIStyle("box");
                GUILayout.BeginArea(new Rect(10, 10, 300, 400), boxStyle);
                {
                    GUILayout.Label("Mesh Information");
                    if(mi == null) {
                        return;
                    }
                    if(mi.ReadMesh()) {
                        GUILayout.Label("Mesh Name: " + mi.meshName);
                        GUILayout.Label("Vertices: " + mi.verticesNumber + " | Triangles: " + mi.trianglesNumber + " | SubMesh: " + mi.subMeshCount);
                        GUILayout.Label("Normals: " + mi.containsNormal + " | Color: " + mi.containsColor + " | UV2: " + mi.containsUV2);
                        GUILayout.Label("Turntable Speed: ");
                        mi.turnTableSpeed = GUILayout.HorizontalSlider(mi.turnTableSpeed, 0, 10);
                        mi.turnTable(mi.turnTableSpeed);
                        GUILayout.Space(18);
                        GUILayout.BeginHorizontal();
                        if(GUILayout.Button("Material", GUILayout.Width(144))) {
                            mi.SetDefaultMaterial();
                        }
                        if(GUILayout.Button("UV", GUILayout.Width(144))) {
                            mi.CheckUV();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(2);
                        GUILayout.Label("UV Tilling: (works only for UV mode)");
                        mi.textureTilling = GUILayout.HorizontalSlider(mi.textureTilling, 1, 20);
                        mi.TillingTexture(mi.textureTilling);
                        GUILayout.Space(15);
                        GUILayout.Label("Material Name: " + mi.GetDefaultMaterialName());
                        GUILayout.Label("Shader Name: " + mi.GetShaderName());
                        GUILayout.Label("Textures In Use: ");
                        Dictionary<string, string> texturesInfo = mi.GetTexturesNamesInUse();
                        if(texturesInfo.Count >= 1) {
                            GUILayout.BeginVertical();
                            foreach(string textureVarName in texturesInfo.Keys) {
                                string textureFileName = texturesInfo[textureVarName];
                                if(GUILayout.Button(textureFileName, GUILayout.Width(290))) {
                                    mi.CheckMap(textureVarName);
                                }

                            }
                            GUILayout.EndVertical();
                        }

                    }
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
    }
}
