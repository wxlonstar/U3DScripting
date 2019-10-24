using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMoveToWards2 : MonoBehaviour {
    private float _num = 12f;
    private void Update() {
        GoMove(_num);
        Debug.Log(_num);
    }
    private void GoMove(float num) {
        _num = Mathf.MoveTowards(num, 100f, 2f);

    }

}
