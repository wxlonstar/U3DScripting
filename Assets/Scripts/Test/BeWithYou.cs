using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeWithYou : MonoBehaviour {
    public Transform target;
    Vector3 currentLocalPosition;
    Vector3 currentDirection;
    // Start is called before the first frame update
    void Start() {
        currentLocalPosition = transform.localPosition;
        currentDirection = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit rh;
        /*
        Physics.Raycast(this.transform.position, this.transform.forward, out rh);
        if(rh.collider.name != "Sphere") {
            this.transform.position += this.transform.forward * 0.1f;
        }
        */
        Physics.Raycast(this.transform.position, this.transform.forward, out rh);
        if (rh.collider.name != "Sphere") {
            this.transform.position += this.transform.forward * 0.1f;
        }
    }
}
