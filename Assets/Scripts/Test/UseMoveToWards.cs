using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMoveToWards : MonoBehaviour {
    private float _distance = 3f;
    public float speed =0.2f;
    //private bool _isAway = false;
    private float _currentX;
    private void Start() {
        _currentX = this.transform.position.x;
    }
    void Update() {
        _currentX = Mathf.MoveTowards(_currentX, _distance, speed);
        this.transform.position = new Vector3(_currentX, 0, 0);
        Debug.Log(_currentX);
      
    }
}
