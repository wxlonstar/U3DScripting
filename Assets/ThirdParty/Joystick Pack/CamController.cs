using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CamController : MonoBehaviour {
    Camera cam;
    public Joystick joystick;
    public Transform[] ResetLocation;
    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if(cam != null) {
            CamDir(cam);
        }
        
    }

    void CamDir(Camera cam) {
        /*
        float hor = Input.GetAxis("Horizontal") * Time.deltaTime * 80f;
        float ver = Input.GetAxis("Vertical") * Time.deltaTime;
        */
        float hor = joystick.Horizontal * Time.deltaTime * 80f;
        float ver = joystick.Vertical * Time.deltaTime;
        Vector3 ToDirection = new Vector3(0, ver, 0);
        Vector3 CurrentDirection = this.transform.forward;
        CurrentDirection += ToDirection;
        this.transform.rotation = Quaternion.LookRotation(CurrentDirection, Vector3.up);
        transform.Rotate(0, hor, 0);
    }

    public void CamFwd() {
        this.transform.position += this.transform.forward * Time.deltaTime;
    }

    public void CamBwd() {
        this.transform.position += (-1 * this.transform.forward) * Time.deltaTime;
    }

    public void CamReset() {
        if(ResetLocation.Length > 0) {
            int index = Random.Range(0, ResetLocation.Length);
            if (ResetLocation[index] != null) {
                this.transform.position = ResetLocation[index].position;
                this.transform.eulerAngles = ResetLocation[index].eulerAngles;
            } else {
                CamZeroOut();
            }
        }else {
            CamZeroOut();
        }
    }

    void CamZeroOut() {
        Vector3 resetValue = new Vector3(0, 0, 0);
        this.transform.position = resetValue;
        this.transform.eulerAngles = resetValue;
    }

}
