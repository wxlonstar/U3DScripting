using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UseRigidbody : MonoBehaviour {
    public Transform target;
    // Start is called before the first frame update
    void Start() {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
        this.GetComponent<Rigidbody>().useGravity = false;
        
    }

    // Update is called once per frame
    void Update() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        //this.GetComponent<Rigidbody>().position += this.transform.forward * Time.deltaTime * 3f;        // This is not recommonned
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position += this.transform.forward * Time.deltaTime * Random.Range(1f, 5f));
        UseRigidRotation(target);
        useRigidBodyAddForce(target);
    }

    void UseRigidRotation(Transform target) {
        Vector3 targetDir = target.position - this.transform.position;
        targetDir.y = 0;
        Quaternion rotateTo = Quaternion.LookRotation(targetDir);
        this.GetComponent<Rigidbody>().MoveRotation(rotateTo);
    }

    void useRigidBodyAddForce(Transform target) {
        float distance = Vector3.Distance(target.transform.position, this.transform.position);

        if (distance >= 20) {
            Debug.Log(this.name + "PowerUp!!!");
            this.GetComponent<Rigidbody>().AddForce(this.transform.forward * 100);
        }
    }
}
