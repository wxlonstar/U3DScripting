using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnim : MonoBehaviour {
    public GameObject target;
    public float speed = 0.1f;

    private void Update() {
        MoveToTarget(target);
    }

    private void MoveToTarget(GameObject target) {
        if(target == null) {
            Debug.Log("Can't find target positon.");
            return;
        }
        Vector3 targetPosition = target.transform.position;
        float targetX = targetPosition.x;
        float targetY = targetPosition.y;
        float targetZ = targetPosition.z;
        float currentX = this.transform.position.x;
        float currentY = this.transform.position.y;
        float currentZ = this.transform.position.z;
        this.transform.position = new Vector3(
                Mathf.Lerp(currentX, targetX, Time.deltaTime * speed),
                Mathf.Lerp(currentY, targetY, Time.deltaTime * speed),
                Mathf.Lerp(currentZ, targetZ, Time.deltaTime * speed)
            );
        
    }
}
