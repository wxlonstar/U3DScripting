using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    private Transform myPlayer = null;
    public float speed = 1f;
    // Start is called before the first frame update
    void Awake() {
        myPlayer = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        myPlayer.transform.position += speed * myPlayer.transform.forward * Time.deltaTime;
    }
}
