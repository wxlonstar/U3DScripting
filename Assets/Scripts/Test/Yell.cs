using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yell : MonoBehaviour {

    public GameObject target;

    private void Start() {
        LetEmHey(target);

    }

    void LetEmHey(GameObject gameObj) {
        Transform[] tempTrans = gameObj.GetComponentsInParent<Transform>();
        Transform rootTrans = tempTrans[tempTrans.Length - 1];
        Transform[] trans = rootTrans.GetComponentsInChildren<Transform>();
        foreach(Transform t in trans) {
            GameObject.Find(t.name).AddComponent<YellToHey>();
            t.BroadcastMessage("Hey", null, SendMessageOptions.RequireReceiver);
        }

        

    }

}
