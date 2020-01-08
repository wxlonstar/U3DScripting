using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
    Camera cam;
    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        if(cam != null) {
            CamDir(cam);
            CamOfst(cam);
        }
        
    }

    void CamDir(Camera cam) {
        float hor = Input.GetAxis("Horizontal") * Time.deltaTime ;
        float ver = Input.GetAxis("Vertical") * Time.deltaTime;
        Vector3 ToDirection = new Vector3(hor, ver, 0);
        Vector3 CurrentDirection = this.transform.forward;
        CurrentDirection += ToDirection;
        this.transform.rotation = Quaternion.LookRotation(CurrentDirection, Vector3.up);
      
    }

    void CamOfst(Camera cam) {

    }
}
