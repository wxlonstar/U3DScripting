using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterCull : MonoBehaviour
{
    public float maxDistance = 20f;
    public float midDistance = 12f;
    public float minDistance = 5f;   
    float waterDistance;
    Vector3 waterPos;
    Vector3 playerPos;
    GameObject player;
    public bool isCull = false;
    public bool isNotFoam = false;
    public bool isNotCaustics = false;

    void Update() {
        player = Camera.main.gameObject;
        waterPos = transform.position;
        if(player == null) return;
        float distance = Mathf.Abs(player.transform.position.y - waterPos.y);
        if(distance < minDistance){
            isCull = false;
            isNotFoam = false;
            isNotCaustics = false;
        }else if (distance < midDistance){
            isNotCaustics = true;
            isCull = false;
            isNotFoam = false;
        }else if(distance <= maxDistance){
            isNotCaustics = true;
            isNotFoam = true;
            isCull = false;
        }else{
            Debug.Log("isCull");
            isCull = true;
        }

        if(isCull){
            Shader.EnableKeyword("_CULL_ON");
            Shader.DisableKeyword("_CULL_OFF");
        }else{
            
            Shader.EnableKeyword("_CULL_OFF");
            Shader.DisableKeyword("_CULL_ON");
            if(isNotFoam){
                Shader.EnableKeyword("_NOTFOAM");
                Shader.DisableKeyword("_FOAM");
            }else{
                Shader.EnableKeyword("_FOAM");
                Shader.DisableKeyword("_NOTFOAM");
            }
            if(isNotCaustics){
                Shader.EnableKeyword("_NOTCAUSTICS");
                Shader.DisableKeyword("_CAUSTICS");
            }else{
                Shader.EnableKeyword("_CAUSTICS");
                Shader.DisableKeyword("_NOTCAUSTICS");
            }
        }
    }
}
