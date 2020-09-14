using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class T : MonoBehaviour {
    LightmapData[] m_savedSceneLightmapData;

    [System.Serializable]
    public struct CustomLightmaps {
        public Texture2D lightmapColor;
        public Texture2D lightmapDir;
        //public Texture2D lightmapLight;
        public Texture2D shadowMask;
    }
    [System.Serializable]
    public struct MeshRendererLightmapData {
        public string friendlyName;
        public MeshRenderer actualMeshRenderer;
        public int objectHash;
        public int RendererLightmapIndex;
        public Vector4 RendererLightmapScaleOffset;

        public MeshRendererLightmapData(MeshRenderer actualMeshRenderer, string friendlyName, int objectHash, int RendererLightmapIndex, Vector4 RendererLightmapScaleOffset) {
            this.actualMeshRenderer = actualMeshRenderer;
            this.friendlyName = friendlyName;
            this.objectHash = objectHash;
            this.RendererLightmapIndex = RendererLightmapIndex;
            this.RendererLightmapScaleOffset = RendererLightmapScaleOffset;
        }
    }
    [Space(15)]
    [SerializeField]
    List<CustomLightmapsArray> m_savedSceneLightmaps;
    [Space(30)]
    [TextArea(3, 3)]
    public string info0 = "Press this bool after you finished a full bake \nto record all generated lightmap references.";
    [Space(5)]
    [SerializeField]
    bool m_SaveNewLightmapData;

    [Space(15)]
    [TextArea(3, 3)]
    public string info1 = "Press this bool after you Render Selected Groups \nand you see the other non-re-rendered groups \nsuddenly got broken lightmaps/UVs. Or if \nyou just want to swap between different lightmap variants (e.g. day/night).";
    [Space(15)]
    [SerializeField]
    int m_LightmapSelectionToRestoreFrom = 0;
    [Space(5)]
    [SerializeField]
    bool m_LoadLightmapFromSelection;

    [Space(20)]
    [Header("________________________________________________________________________________ Nice custom GUI skills amirite?")]
    [Space(45)]
    [SerializeField]
    bool m_AlsoSaveLightmapDataFromAllMeshRenderers = false;

    [TextArea(3, 3)]
    public string info2 = "Be patient when you click Save or Load. It may take a while (e.g. up to 10 seconds) if you have many thousands of mesh renderers in your scene.";

    [System.Serializable]
    public class CustomLightmapsArray {//we need a struct/class to hold arrays because unity inspector does not support arrays of arrays, but supports arrays of structs/classes that hold arrays........!
        public string friendlyName;
        public CustomLightmaps[] customLightmaps;
        public Dictionary<int, MeshRendererLightmapData> meshRendererLightmapDataFromScene;
        [TextArea(4, 4)]
        public string info;// "The following array is just for show in the inspector. Internally I use a dictionary of instance ids, so it can be ported between similar scenes without needing mesh renderer references.";
        public List<MeshRendererLightmapData> meshRendererLightmapDataFromScene_inspector;

        public CustomLightmapsArray(int length) {
            friendlyName = "";
            customLightmaps = new CustomLightmaps[length];
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();

            info = "The following array is for show in the inspector. BUT also to save the non-serializable internal dictionary of instance ids";
            meshRendererLightmapDataFromScene_inspector = new List<MeshRendererLightmapData>();
        }
        public CustomLightmapsArray(int length, string friendlyName) {
            this.friendlyName = friendlyName;
            customLightmaps = new CustomLightmaps[length];
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();

            info = "The following array is for show in the inspector. BUT also to save the non-serializable internal dictionary of instance ids";
            meshRendererLightmapDataFromScene_inspector = new List<MeshRendererLightmapData>();
        }

        public void CloneToInspectorArray() {
            meshRendererLightmapDataFromScene_inspector.Clear();
            meshRendererLightmapDataFromScene_inspector.AddRange(meshRendererLightmapDataFromScene.Values);
        }

        public void LoadDictionaryFromInspectorArray() {
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();
            foreach(MeshRendererLightmapData lmd in meshRendererLightmapDataFromScene_inspector) {
                meshRendererLightmapDataFromScene.Add(lmd.objectHash, lmd);
            }
        }
    }


    public void SaveLightmapData() {
        m_savedSceneLightmapData = new LightmapData[LightmapSettings.lightmaps.Length];

        for(int i = 0; i < LightmapSettings.lightmaps.Length; i++) {
            m_savedSceneLightmapData[i] = LightmapSettings.lightmaps[i];
        }

        int index;
        if(m_AlsoSaveLightmapDataFromAllMeshRenderers) {
            MeshRenderer[] allMeshRenderers = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
            m_savedSceneLightmaps.Add(new CustomLightmapsArray(m_savedSceneLightmapData.Length, m_savedSceneLightmaps.Count + " "));
            index = m_savedSceneLightmaps.Count - 1;
            m_LightmapSelectionToRestoreFrom = index;

            for(int i = 0; i < allMeshRenderers.Length; i++) {
                if(allMeshRenderers[i].lightmapIndex == -1) {
                    continue;
                }

                MeshRendererLightmapData newLMD = new MeshRendererLightmapData(allMeshRenderers[i], allMeshRenderers[i].name, allMeshRenderers[i].GetHashCode(), allMeshRenderers[i].lightmapIndex, allMeshRenderers[i].lightmapScaleOffset);
                m_savedSceneLightmaps[index].meshRendererLightmapDataFromScene.Add(newLMD.objectHash, newLMD);
            }
            m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].CloneToInspectorArray();
        } else {
            index = m_savedSceneLightmaps.Count - 1;
            m_LightmapSelectionToRestoreFrom = index;
        }

        for(int i = 0; i < LightmapSettings.lightmaps.Length; i++) {
            m_savedSceneLightmaps[index].customLightmaps[i].lightmapColor = m_savedSceneLightmapData[i].lightmapColor;
            m_savedSceneLightmaps[index].customLightmaps[i].lightmapDir = m_savedSceneLightmapData[i].lightmapDir;
            //m_savedSceneLightmaps[i].lightmapLight = m_savedSceneLightmapData[i].lightmapLight;
            m_savedSceneLightmaps[index].customLightmaps[i].shadowMask = m_savedSceneLightmapData[i].shadowMask;
        }

        Debug.Log("[BakeryLightmapManager][SAVED] Current LightmapSettings.lightmaps (lightmap texture references) saved to m_savedSceneLightmaps!");
    }

    public void LoadLightmapData(int lightmapSelectionToRestoreFrom) {
        m_LightmapSelectionToRestoreFrom = lightmapSelectionToRestoreFrom;

        //LightmapSettings.lightmaps = new LightmapData[m_savedSceneLightmapData.Length];
        LightmapData[] customLightmapDataArr = new LightmapData[m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length];
        /*
        if(m_savedSceneLightmapData!=null)
            for(int i = 0; i< m_savedSceneLightmaps.Length; i++){
                if(i<m_savedSceneLightmapData.Length)
                    customLightmapDataArr[i] = m_savedSceneLightmapData[i];
                else{
                    customLightmapDataArr[i] = new LightmapData();
                    Debug.Log("[BakeryLightmapManager] The internal m_savedSceneLightmapData number of textures does not match the m_savedSceneLightmaps number of textures that you have in the inspector. Did you forget to Save New Lightmap Data after the previous bake completed?");
                }
            }
        else*/
        for(int i = 0; i < m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length; i++) {
            customLightmapDataArr[i] = new LightmapData();
        }

        if(m_AlsoSaveLightmapDataFromAllMeshRenderers) {
            //Unity can't serialize dictionaries, and this editor script will have its internal dictionary flushed/reset for instance every time you recompile any scripts in your project.
            if(m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataFromScene == null) {
                m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].LoadDictionaryFromInspectorArray();
            }

            MeshRenderer[] allMeshRenderers = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
            //foreach(KeyValuePair<int,MeshRendererLightmapData> mrld in m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataForLoadedScenes){
            //foreach(MeshRendererLightmapData mrld in m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataForLoadedScenes){
            //mrld.actualMeshRenderer.lightmapIndex = mrld.RendererLightmapIndex;
            //mrld.actualMeshRenderer.lightmapScaleOffset = mrld.RendererLightmapScaleOffset;
            //}
            for(int i = 0; i < allMeshRenderers.Length; i++) {
                MeshRendererLightmapData mrld;

                bool success = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataFromScene.TryGetValue(allMeshRenderers[i].GetInstanceID(), out mrld);
                if(success) {
                    allMeshRenderers[i].lightmapIndex = mrld.RendererLightmapIndex;
                    allMeshRenderers[i].lightmapScaleOffset = mrld.RendererLightmapScaleOffset;

                    //this is so you can see it in the inspector. I wish Dictionaries worked witht he inspector without having to install 3rd party assets...
                    mrld.actualMeshRenderer = allMeshRenderers[i];
                }
            }
            m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].CloneToInspectorArray();
        }

        for(int i = 0; i < m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length; i++) {
            customLightmapDataArr[i].lightmapColor = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].lightmapColor;
            customLightmapDataArr[i].lightmapDir = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].lightmapDir;
            customLightmapDataArr[i].shadowMask = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].shadowMask;
        }
        LightmapSettings.lightmaps = customLightmapDataArr;

        Debug.Log("[BakeryLightmapManager][RESTORED] Current LightmapSettings.lightmaps (lightmap texture references) restored from m_savedSceneLightmaps!");
    }

    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
        if(m_SaveNewLightmapData) {
            m_SaveNewLightmapData = false;
            SaveLightmapData();
        }

        if(m_LoadLightmapFromSelection) {
            m_LoadLightmapFromSelection = false;

            LoadLightmapData(m_LightmapSelectionToRestoreFrom);
        }
#endif
    }
}