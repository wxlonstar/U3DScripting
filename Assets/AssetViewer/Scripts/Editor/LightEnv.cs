using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightEnv : ScriptableObject {

    public float tempLightIntensity = 1f;
    public int tempLightOnCount = 3;
    GameObject tempLight;
    public float tempLightAngle = 0;
    public bool environmentLightingDone = false;
    public Material bakedSkybox;
    private Cubemap[] reflectionProbes;

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
            Light[] lights  = this.tempLight.GetComponentsInChildren<Light>();
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
        this.CreateAmbient();
        this.CreateReflection();
    }

    public void RemoveEnvironment() {
        if(this.tempLight != null) {
            GameObject.DestroyImmediate(GameObject.Find("Temp Light"));
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

    public void CreateAmbient() { 
    }

    public void CreateReflection() { 
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


