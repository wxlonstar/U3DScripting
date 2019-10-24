using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingVector2 : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Vector2 v2 = new Vector2(1, 1);
        Debug.Log(Vector2.Angle(v2, new Vector2(1, 0)));
        transform.LookAt(new Vector3(1, 0, 0));
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
