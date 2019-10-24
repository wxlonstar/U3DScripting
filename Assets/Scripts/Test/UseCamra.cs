using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCamra : MonoBehaviour {
    private Camera mainCam;
    // Start is called before the first frame update
    void Start() {
        mainCam = Camera.main;

        Debug.Log(mainCam.actualRenderingPath.ToString());

    }
    private void OnPreRender() {
        Debug.Log("HeyICANT");
    }
    private void OnPostRender() {
        Debug.Log("Finished Rendering!");
    }

    // Update is called once per frame
    void Update() {
        GetClickedObject();
        Debug.DrawRay(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0));
        
    }

    void GetClickedObject() {
        Ray mouseRay = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(mouseRay, out hitInfo);
        if (isHit) {
            Debug.Log(hitInfo.collider.ToString());
        }
    }
}
