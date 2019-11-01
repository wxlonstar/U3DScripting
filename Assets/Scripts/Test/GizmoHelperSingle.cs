using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GizmoHelperSingle : MonoBehaviour {

    private Vector3 posX = new Vector3(1, 0, 0);
    private Vector3 posY = new Vector3(0, 1, 0);
    private Vector3 posZ = new Vector3(0, 0, 1);

    private void OnEnable() {
        switch (this.transform.name) {
            case "X":
                this.transform.localPosition = posX;
                Debug.Log(this.transform.name);
                break;
            case "Y":
                this.transform.localPosition = posY;
                Debug.Log(this.transform.name);
                break;
            case "Z":
                this.transform.localPosition = posZ;
                Debug.Log(this.transform.name);
                break;
            default:
                Debug.Log("initialize failed.");
                break;
        }

    }


    private void OnDrawGizmosSelected() {
        if(this.transform.name == "X") {
            Debug.Log(this.transform.name);
            return;
        }
        //GenerateNode(this.transform);
    }


    void GenerateNode(Transform trans) {
        float length = Vector3.Distance(this.transform.position, this.transform.parent.position);
        int num = (int)length / 1;
        Debug.Log(num);

        
    }
}
