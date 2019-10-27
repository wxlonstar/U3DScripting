using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMover : MonoBehaviour {
    private Transform myPlayer = null;
    public float speed = 1f;
    public AnimationCurve AnimCrv;
    // Start is called before the first frame update
    void Awake() {
        myPlayer = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        myPlayer.position += speed * myPlayer.forward * Time.deltaTime;
        float YMovment = AnimCrv.Evaluate(Mathf.PingPong(Time.time, 1f));
        //Debug.Log(YMovment);
        myPlayer.position = new Vector3(myPlayer.position.x, YMovment, myPlayer.position.z);
    }
}
