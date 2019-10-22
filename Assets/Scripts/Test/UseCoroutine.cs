using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCoroutine : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(HitYouTwice());
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("Just updating....");
    }

    IEnumerator HitYouTwice() {
        Debug.Log("Kick your ass.");
        //yield return null;
        yield return new WaitForSeconds(3);
        Debug.Log("Hit you in your face.");
    }
}
