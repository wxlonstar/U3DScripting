using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCC : MonoBehaviour {
    private CharacterController cc = null;
    private 
    // Start is called before the first frame update
    void Awake() {
        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        
        cc.Move(this.transform.forward * Time.deltaTime);
    }
}
