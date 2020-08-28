using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUIntanceTest01 : MonoBehaviour {
    public Transform prefab;
    public int instancesNumber = 5000;
    public float instrancesRadius = 50f;

    private void Start() {
        for(int i = 0; i < instancesNumber; i ++) {
            Transform temp = Instantiate(prefab);
            temp.localPosition = Random.insideUnitSphere * instrancesRadius;
            temp.SetParent(this.transform);
        }
    }
}
