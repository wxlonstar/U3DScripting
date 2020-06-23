using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneEnv : ScriptableObject {
    string sceneName = "Mankind";
    List<Cubemap> reflectionProbes;
    List<Light> lights;
    Material skybox;
    DefaultReflectionMode currentReflectionMode;
    float reflectionIntensity;

    public void Initialize() {
        this.lights = new List<Light>();
        this.reflectionProbes = new List<Cubemap>();
        this.currentReflectionMode = DefaultReflectionMode.Skybox;
        this.reflectionIntensity = 1;

    }
    public void AddReflectionProbes() { 
    }

    public void ParseEnvironmentLighting(AmbientMode mode) {
        if(mode == AmbientMode.Flat) { 
        }

        if(mode == AmbientMode.Skybox) { 
        }

        if(mode == AmbientMode.Trilight) {
            
        }
    }

    public void ApplySceneEnv() { 
    }

    public string GetName() {
        return this.sceneName;
    }
}


