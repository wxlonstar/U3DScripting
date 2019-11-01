using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obj01", menuName = "MyScriptObj/MyObj")]
public class Obj01 : ScriptableObject {
    public new string name = "None";

    

    public void Yell() {
        Debug.Log(this.name);
    }
}
