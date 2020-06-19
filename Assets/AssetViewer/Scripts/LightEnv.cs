using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MileCode {
    [ExecuteInEditMode]
    public class LightEnv : MonoBehaviour {
        [HideInInspector]
        public string name = "Light env";
        [HideInInspector]
        GameObject lightGroup;
        [HideInInspector]
        GameObject reflectionProbe;
        private void OnValidate() {
            Transform[] childrenTransform = this.GetComponentsInChildren<Transform>();
            if(childrenTransform != null) {
                if(childrenTransform.Length >= 1) {
                    foreach(Transform transform in childrenTransform) {
                        //Debug.Log(transform.name);
                        if(transform.name == "LightGroup") {
                            this.lightGroup = transform.gameObject;
                            Debug.Log("found lightGroup");
                        }
                        if(transform.name == "Reflection Probe") {
                            this.reflectionProbe = transform.gameObject;
                            Debug.Log("found Probe");
                        }
                    }
                }
            }
            
        }

        [HideInInspector]
        public float turnTableSpeed = 0;
        
        public void RotateLightGroup(float speed) {
            if(this.lightGroup != null) {
                lightGroup.transform.Rotate(new Vector3(0, Time.deltaTime * 20f, 0));
            }

        }


        private void LightOnOff() {
            if(lightGroup != null) {
                Light[] lights = lightGroup.GetComponentsInChildren<Light>();
                //Debug.Log(lights.Length);
                foreach(Light light in lights) {

                    if(light.enabled == true) {
                        light.enabled = false;
                        Debug.Log(light.name + " : " + light.enabled);
                    } else {
                        light.enabled = true;
                        Debug.Log(light.name + " : " + light.enabled);
                    }
                }
            } 
        }
    }
}
