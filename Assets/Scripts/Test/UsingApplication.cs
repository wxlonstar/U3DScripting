using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingApplication : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //datapath
        //streamingAssets       //StreamingAssets Folder
        //persistentDataPath
        //temporaryPath

        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.temporaryCachePath);
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
