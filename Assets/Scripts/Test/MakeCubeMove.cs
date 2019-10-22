using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCubeMove : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime * 0.5f);
        transform.Translate(Vector3.up * Time.deltaTime * 0.5f);
        transform.Translate(Vector3.right * Time.deltaTime * 0.5f);


    }
}
