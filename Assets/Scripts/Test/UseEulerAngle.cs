using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEulerAngle : MonoBehaviour {
    private void Start() {
        LetRot();
    }
    void LetRot() {
        //this.transform.Rotate(new Vector3(10, 10, 10), Space.World);      UseRotate 
        this.transform.eulerAngles = new Vector3(10, 10, 10);
        
    }
}
