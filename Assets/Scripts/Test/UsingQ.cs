using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingQ : MonoBehaviour {
    [Header("Give a title")]
    public Transform target;
    // Start is called before the first frame update
    void Start() {
        Quaternion q = Quaternion.identity;
        q = Quaternion.Euler(new Vector3(30, 30, 30));      //tranform euler angle to q
        transform.rotation = q;
        Vector3 eAq = q.eulerAngles;        //tranform q to euler
    }

    // Update is called once per frame
    void Update() {
        //ISeeYou(target);
        IWillSeeYou(target);
    }

    void ISeeYou(Transform target) {
        Vector3 targetDir = target.position - this.transform.position;
        targetDir.y = 0;
        this.transform.rotation = Quaternion.LookRotation(targetDir);

    }

    void IWillSeeYou(Transform target) {
        Vector3 targetDir = target.position - this.transform.position;
        targetDir.y = 0;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime);



    }
}
