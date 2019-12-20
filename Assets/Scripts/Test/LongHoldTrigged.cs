using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongHoldTrigged : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Fly() {
        Debug.Log("I'm Flying...");
    }

    public void SetTrue() {
        Debug.Log("Machine true");
    }

    public void SetFalse() {
        Debug.Log("Machine false");
    }


    int boost = 0;
    public void Jump1() {
        if(boost == 0) {
            Debug.Log("Jump1");
            StartCoroutine("Counter");           
        }
        if(boost == 1) {
            Jump2();  
        }
        boost++;
    }

    public void Jump2() {
        Debug.Log("Jump2");
    }

    public IEnumerator Counter() {
        for (float i = 10f; i > 0; i--) {
            Debug.Log("Counter: " + i);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Ended");
        boost = 0;
    }
}
