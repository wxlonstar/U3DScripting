using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingEventType : MonoBehaviour {
    private Event e;
   
    // Start is called before the first frame update
    void Start() {
       
    }

    // Update is called once per frame
    private void OnGUI() {
        e = Event.current;
        if(e.type == EventType.MouseUp) {
            Debug.Log(e.mousePosition);
        }
    }
}
