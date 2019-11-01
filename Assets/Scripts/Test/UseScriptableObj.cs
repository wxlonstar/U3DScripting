using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseScriptableObj : MonoBehaviour {
    public Obj01 obj = null;
    // Start is called before the first frame update
    void Start() {
        if(obj != null) {
            obj.Yell();
        }
    }

    
}
