using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UsingVector3 : MonoBehaviour {
    public GameObject obj;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        //ICanSeeYou(obj);
        GradSeeYou(obj);
    }

    void ICanSeeYou(GameObject target) {
        this.transform.LookAt(target.transform);
    }

    void GradSeeYou(GameObject target) {
    }
}
