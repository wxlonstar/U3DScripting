﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnableSRPBatcher : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }

    // Update is called once per frame
    void Update() {

    }
}
