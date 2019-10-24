using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingPingPong : MonoBehaviour {
    //float a = 10f;
    private void Start() {

        StartCoroutine("CheckPingPong");
    }
    private void Update() {

        //UsePingPong();
    }
    //PingPong returns value that will NEVER EVER be larger than length and smaller than 0
    void UsePingPong() {
        this.transform.position = new Vector3(Mathf.PingPong(Time.time * 10, 5), this.transform.position.y, this.transform.position.z);
    }

    IEnumerator CheckPingPong() { 
        for(float i = 1.0f; i <= 100f; i += 1.0f) {
            Debug.Log(Mathf.PingPong(i, 10));
            yield return new WaitForSeconds(1);
        }

    }

}
