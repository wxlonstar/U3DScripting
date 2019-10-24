using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTimStamp : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        int timeStamp = (int)System.DateTime.Now.Ticks;
        if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(timeStamp);
        }
    }
}
