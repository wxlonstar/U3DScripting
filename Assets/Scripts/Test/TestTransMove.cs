using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransMove : MonoBehaviour {
    // Start is called before the first frame update
    private float moveState = 0;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey(KeyCode.F)) {
            Debug.Log(moveState);
            moveState += 0.2f;
            moveState = Mathf.Clamp01(moveState);
            this.GetComponent<Animator>().SetFloat("MoveCarefully", moveState);
        } else {
            Debug.Log(moveState);
            moveState -= 0.5f * Time.deltaTime;
            moveState = Mathf.Clamp01(moveState);
            this.GetComponent<Animator>().SetFloat("MoveCarefully", moveState);
        }
        
    }
}
