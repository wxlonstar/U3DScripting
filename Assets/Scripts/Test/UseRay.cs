using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseRay : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Ray r = new Ray();
        r.direction = new Vector3(0, 1, 0);
        RaycastHit rh;
        Physics.Raycast(r, out rh);
        Debug.Log(rh.point);
    }
}
