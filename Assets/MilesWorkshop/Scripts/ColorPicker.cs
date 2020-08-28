using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class ColorPicker : MonoBehaviour {
    [Range(0, 6)]
    public int diceNumber;
    private Material material;

    private new Renderer renderer;

    private MaterialPropertyBlock propertyBlock;

    private void Start() {
        //this.material = this.GetComponent<MeshRenderer>().sharedMaterial;
        this.propertyBlock = new MaterialPropertyBlock();
        this.renderer = this.GetComponent<Renderer>();
    }

    private void Update() {
        //this.material.SetColor("_PickColor", GetPickColorFromDiceNumber(this.diceNumber));
        SetDiceNumber(this.diceNumber);
    }
    
    void SetDiceNumber(int diceNumber) {
        //Debug.Log(DiceNumberToColor(diceNumber).ToString());
        if(this.propertyBlock != null) {
            this.renderer.GetPropertyBlock(this.propertyBlock);
            this.propertyBlock.SetColor("_PickColor", GetPickColorFromDiceNumber(diceNumber));
            this.renderer.SetPropertyBlock(this.propertyBlock);
        }
    }
    
    private Color GetPickColorFromDiceNumber(int diceNumber) {
        switch(diceNumber) {
            case 0:
                return Color.yellow;
            case 1:
                return Color.green;
            case 2:
                return Color.cyan;
            case 3:
                return Color.magenta;
            case 4:
                return Color.blue;
            case 5:
                return Color.white;
            case 6:
                return Color.red;
            default:
                return Color.black;
        }
    }

}
