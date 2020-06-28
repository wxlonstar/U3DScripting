using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightEnv : ScriptableObject {

    public string sceneName = "";
    [HideInInspector]
    public float tempLightIntensity = 1f;
    [HideInInspector]
    public int tempLightOnCount = 3;
    [HideInInspector]
    GameObject tempLight;
    [HideInInspector]
    public float tempLightAngle = 0;
    [HideInInspector]
    public bool environmentLightingDone = false;
    [HideInInspector]
    public Material bakedSkybox;

    public void GetEnvironmentLightingDone() {
        LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
        LightmapEditorSettings.bakeResolution = 10;
        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
        Lightmapping.bakeCompleted += this.TurnOffAutoGenerate;
    }

    IEnumerator JustWaitAndRun(float seconds) {
        yield return new EditorWaitForSeconds(seconds);
    }

    private void TurnOffAutoGenerate() {
        JustWaitAndRun(1.0f);
        this.environmentLightingDone = true;
        this.bakedSkybox = RenderSettings.skybox;
        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
        Lightmapping.bakeCompleted -= this.TurnOffAutoGenerate;
    }

    public void SetLightEnvAngle(float lightAngle) {
        if(this.tempLight != null) {
            this.tempLight.transform.localEulerAngles = new Vector3(0, lightAngle, 0);
        }
    }

    public void SetLightEnvIntensity(float intensity) {
        if(this.tempLight != null) {
            Light[] lights = this.tempLight.GetComponentsInChildren<Light>();
            if(lights != null && lights.Length >= 1) {
                foreach(Light light in lights) {
                    if(light.transform.name == "light_0") {
                        light.intensity = intensity;
                    } else {
                        light.intensity = intensity * 0.5f;
                    }
                }
            }
        }
    }


    public string GetName() {
        return this.sceneName;
    }

    public void SetLightEnvOnNumber(int number) {
        if(this.tempLight != null) {
            Light[] lights = this.tempLight.GetComponentsInChildren<Light>();
            if(lights != null && lights.Length >= 1) {
                switch(number) {
                    case 0:
                        lights[0].enabled = true;
                        lights[1].enabled = false;
                        lights[2].enabled = false;
                        break;
                    case 1:
                        lights[0].enabled = true;
                        lights[1].enabled = true;
                        lights[2].enabled = false;
                        break;
                    case 2:
                        lights[0].enabled = true;
                        lights[1].enabled = true;
                        lights[2].enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

    }


    public void CreateEnvironment() {
        if(GameObject.Find("Temp Light") != null) {
            Debug.Log("Temp Light exsits.");
            return;
        }
        this.CreateLight(3);
    }

    public void RemoveEnvironment() {
        if(this.tempLight != null) {
            GameObject.DestroyImmediate(GameObject.Find("Temp Light"));
        }
    }

    public void RemoveTempLight() {
        GameObject tobeRemovedTempLight = GameObject.Find(this.name);
        if(tobeRemovedTempLight != null) {
            GameObject.DestroyImmediate(tobeRemovedTempLight);
        }
    }

    public void CreateLight(int lightNumber) {

        this.tempLight = new GameObject("Temp Light");
        Vector3[] lightsAngles = new Vector3[] {
            new Vector3(40, 140, 0),
            new Vector3(50, -40, 0),
            new Vector3(-50, 0, 150)
        };
        for(int i = 0; i < lightNumber; i++) {
            GameObject lightObj = new GameObject("light_" + i);
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;

            if(i == 0) {
                light.intensity = this.tempLightIntensity;
                light.shadows = LightShadows.Soft;
            } else {
                light.intensity = this.tempLightIntensity * 0.5f;
                light.shadows = LightShadows.None;
            }
            light.transform.SetParent(this.tempLight.transform);
            lightObj.transform.localEulerAngles = lightsAngles[i];
        }
    }

    [SerializeField]
    private string[] lightNames;
    [SerializeField]
    private Vector3[] lightsTransform_rotation;
    [SerializeField]
    [ColorUsage(true, false)]
    private Color[] lightColors;
    [SerializeField]
    private LightmapBakeType[] lightBakeType;
    [SerializeField]
    private float[] lightIntensity;
    [SerializeField]
    private float[] lightIndirectIntensity;
    [SerializeField]
    private LightShadows[] lightShadowType;

    public void SaveLightParams() {
        Light[] lights = Light.GetLights(LightType.Directional, 0);
        if(lights.Length <= 0) {
            Debug.Log("This scene has no directional light saved.");
            return;
        }
        this.lightNames = new string[lights.Length];
        this.lightsTransform_rotation = new Vector3[lights.Length] ;
        this.lightColors = new Color[lights.Length];
        this.lightBakeType = new LightmapBakeType[lights.Length];
        this.lightIntensity = new float[lights.Length];
        this.lightIndirectIntensity = new float[lights.Length];
        this.lightShadowType = new LightShadows[lights.Length];
        for(int i = 0; i < lights.Length; i++) {
            lightNames[i] = lights[i].gameObject.name;
            lightsTransform_rotation[i] = lights[i].transform.rotation.eulerAngles;
            //Debug.Log(lightsTransform_rotation[i]);
            lightColors[i] = lights[i].color;
            lightBakeType[i] = lights[i].lightmapBakeType;
            lightIntensity[i] = lights[i].intensity;
            lightIndirectIntensity[i] = lights[i].bounceIntensity;
            lightShadowType[i] = lights[i].shadows;
        }
        //this.lightsTransform_rotation = lights[1].transform.rotation.eulerAngles;
    }

    public void UseLightFromScriptObject() {
        if(GameObject.Find(this.name) == null) {
            this.tempLight = new GameObject(this.name);
            for(int i = 0; i < lightNames.Length; i++) {
                GameObject lightObj = new GameObject(this.lightNames[i]);
                Light lightComponent = lightObj.AddComponent<Light>();
                lightComponent.type = LightType.Directional;
                lightComponent.color = this.lightColors[i];
                lightComponent.lightmapBakeType = this.lightBakeType[i];
                lightComponent.intensity = this.lightIntensity[i];
                lightComponent.bounceIntensity = this.lightIndirectIntensity[i];
                lightComponent.shadows = this.lightShadowType[i];
                lightObj.transform.eulerAngles = this.lightsTransform_rotation[i];
                lightObj.transform.SetParent(this.tempLight.transform);
            }
        }
    }

    [SerializeField]
    private AmbientMode ambientMode;
    // if ambient mode is skybox
    [SerializeField]
    private float ambientIntensity = 1f;
    [SerializeField]
    private Material ambientSkybox = null;
    [SerializeField]
    [ColorUsage(false, false)]
    private Color ambientSkyboxColor;
    [SerializeField]
    [ColorUsage(false, true)]
    private Color[] ambientColors;
    [SerializeField]
    [ColorUsage(false, true)]
    private Color ambientFlatColor;
    // if ambient mode is flat

    public void SaveEnvironmentLighting() {
        ambientMode = RenderSettings.ambientMode;
        if(this.ambientMode == AmbientMode.Skybox) {
            this.ambientIntensity = RenderSettings.ambientIntensity;
            this.ambientSkybox = RenderSettings.skybox;
            if(this.ambientSkybox == null) {
                this.ambientSkyboxColor = RenderSettings.ambientSkyColor;
            }
        }
        if(this.ambientMode == AmbientMode.Flat) {
            this.ambientFlatColor = RenderSettings.ambientSkyColor;
        }
        if(this.ambientMode == AmbientMode.Trilight) {
            this.ambientColors = new Color[3];
            this.ambientColors[0] = RenderSettings.ambientSkyColor;
            this.ambientColors[1] = RenderSettings.ambientEquatorColor;
            this.ambientColors[2] = RenderSettings.ambientGroundColor;
        }
    }

    public void UseEnvironmentLightFromScriptObject() {
        RenderSettings.ambientMode = this.ambientMode;
        if(this.ambientMode == AmbientMode.Skybox) {
            RenderSettings.ambientIntensity = this.ambientIntensity;
            if(this.ambientSkybox != null) {
                RenderSettings.skybox = this.ambientSkybox;
            } else {
                RenderSettings.skybox = null;
                RenderSettings.ambientSkyColor = this.ambientSkyboxColor;
            }
            
        }
        if(this.ambientMode == AmbientMode.Flat) {
            RenderSettings.ambientSkyColor = this.ambientFlatColor;
        }
        if(this.ambientMode == AmbientMode.Trilight) {
            RenderSettings.ambientSkyColor = this.ambientColors[0];
            RenderSettings.ambientEquatorColor = this.ambientColors[1];
            RenderSettings.ambientGroundColor = this.ambientColors[2];
        }
    }

    [SerializeField]
    private DefaultReflectionMode defaultReflectionMode;
    [SerializeField]
    private Cubemap customReflectionProbe = null;
    [SerializeField]
    private float reflectionIntensity = 1;
    public void SaveReflectionProbe() {
        this.defaultReflectionMode = RenderSettings.defaultReflectionMode;
        if(this.defaultReflectionMode == DefaultReflectionMode.Skybox) {
        }
        if(this.defaultReflectionMode == DefaultReflectionMode.Custom) {
            if(RenderSettings.customReflection != null) {
                this.customReflectionProbe = RenderSettings.customReflection;
            }
        }
        this.reflectionIntensity = RenderSettings.reflectionIntensity;
    }

    public void UseReflectionProbeFromScriptObject() {
        RenderSettings.defaultReflectionMode = this.defaultReflectionMode;
        if(this.defaultReflectionMode == DefaultReflectionMode.Skybox) {
        }
        if(this.defaultReflectionMode == DefaultReflectionMode.Custom) {
            if(this.customReflectionProbe != null) {
                RenderSettings.customReflection = this.customReflectionProbe;
            }
        }
        RenderSettings.reflectionIntensity = reflectionIntensity;
    }


    // ------------------- static ---------------------
    static List<GameObject> currentLights;
    public static void SaveCurrentLightEnv() {
        currentLights = GetSceneDirectionalLights();
    }

    private static List<GameObject>  GetSceneDirectionalLights() {
        List<GameObject> lights = new List<GameObject>();
        Light[] allLight = Light.GetLights(LightType.Directional, 0);
        foreach(Light light in allLight) {
            lights.Add(light.gameObject);
        }
        //Debug.Log(lights.Count + " dir-light found and saved.");
        return lights;
    }

    public static void TurnOffSavedLightEnv() {
        //Debug.Log(currentLights.Count + " dir-light off.");
        if(currentLights.Count >= 1) {
            foreach(GameObject light in currentLights) {
                light.SetActive(false);
            }
        }
    }

    public static void RestoreSavedLightEnv() {
        //Debug.Log(currentLights.Count + " dir-light restored.");
        if(currentLights.Count >= 1) {
            foreach(GameObject light in currentLights) {
                if(light != null) {
                    light.SetActive(true);
                }
                
            }
        }
    }
}


