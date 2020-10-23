using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdaTest : MonoBehaviour {
    // Start is called before the first frame update
    List<int> list = new List<int>();
    void Start() {
        
        list[0] = 1;
        list[1] = 2;
        list[2] = 3;

    }
    // fast way to define readonly property
    public int I => list.Count;

    // Update is called once per frame
    void Update() {

    }
}
