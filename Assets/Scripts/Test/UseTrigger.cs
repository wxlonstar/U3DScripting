using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log("KK");
        Debug.Log(other.gameObject.ToString());
        other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
    }
   
}
