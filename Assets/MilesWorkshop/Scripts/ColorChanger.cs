using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour {
    [SerializeField]
    Color color = Color.white;
    private new Renderer renderer;
    private MaterialPropertyBlock propBlock;
    // Update is called once per frame
    private void Awake() {
        this.renderer = GetComponent<Renderer>();
        this.propBlock = new MaterialPropertyBlock();
    }
    void Update() {
        SetColorForRobot(this.color);
    }

    private void SetColorForRobot(Color color) {
        if(propBlock != null) {
            Debug.Log(this.gameObject.name);
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", color);
            renderer.SetPropertyBlock(propBlock);
        }
    }
}
