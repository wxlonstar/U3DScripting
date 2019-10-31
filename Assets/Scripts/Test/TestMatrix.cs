using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatrix : MonoBehaviour {

    public Transform trans;
    
    private void OnDrawGizmos() {


        Debug.Log("localPosition: " + this.transform.localPosition);
        Vector3 posWorld = this.transform.root.localToWorldMatrix.MultiplyPoint(this.transform.localPosition);
        Debug.Log("worldPosition: " + posWorld);
        Vector3 posCam = trans.worldToLocalMatrix.MultiplyPoint(posWorld);
        Debug.Log("CameraPosition: " + posCam);
    }

}
