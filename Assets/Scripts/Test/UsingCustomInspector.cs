using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingCustomInspector : MonoBehaviour {
    private int exp;
    public int Exp {
        set {
            if(value >= 0) {
                exp = value;
            }else {
                exp = 0;
            }
        }
        get {
            return exp;
        }
    }
    public int Level {
        get {
            return (int)exp / 70;
        }
    }
}
