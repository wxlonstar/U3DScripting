using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCoroutineColor : MonoBehaviour {
    private Color _currentColor;
    private bool _CoroStatus = false;
    private void Start() {
        _currentColor = this.GetComponent<MeshRenderer>().material.color;
        Debug.Log(_currentColor);
        StartCoroutine("ColorChange", _currentColor);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_CoroStatus == false) {
                StopCoroutine("ColorChange");
                _currentColor = this.GetComponent<MeshRenderer>().material.color;
                _CoroStatus = true;
            }else {
                StartCoroutine("ColorChange", _currentColor);
                _CoroStatus = false;
            }
        }
    }
    IEnumerator ColorChange(Color currentColor) {
        for (float i = 0.0f; i <= 1.0f; i += 0.01f) {
            this.GetComponent<MeshRenderer>().material.color = Color.Lerp(currentColor, Color.blue, i);
            yield return new WaitForSeconds(0.1f);
        }
    }

}   

