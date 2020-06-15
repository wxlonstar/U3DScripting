using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

[ExecuteInEditMode]
public class ReflectionProbeTools : MonoBehaviour {
    [SerializeField]
    public float intensity = 1;
    private void Update() {
        ReflectionProbe reflectionPorbe =  this.GetComponent<ReflectionProbe>();
        reflectionPorbe.intensity = intensity;
    }
}
