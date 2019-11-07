using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestMoveMatrix : MonoBehaviour {
    [Range(1f, 20f)]
    public float speed = 1f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        SimpleMoveMatrix();
    }

    private void SimpleMove() {
        float h = Input.GetAxis("Horizontal") * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * Time.deltaTime; 
        this.transform.Translate(new Vector3(0, 0, v) * speed);
        this.transform.Rotate(new Vector3(0, h, 0) * 8f * speed);
    }

    private void SimpleMoveMatrix() {
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            Vector3 dir = new Vector3(h * Time.deltaTime * speed, 0, v * Time.deltaTime * speed);
            Quaternion targetDir = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetDir, Time.deltaTime * 500.0f);
            if (transform.rotation == targetDir) {
                this.transform.position += dir * Time.deltaTime * 300;
            }
        }
        

        //this.transform.position += newPos;
    }
}
