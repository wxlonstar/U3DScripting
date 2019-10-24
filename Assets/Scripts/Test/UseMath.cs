using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMath : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        float deg01 = 350f;
        float deg02 = 60f;
        Debug.Log(Mathf.DeltaAngle(deg01, deg02));
    }

}

