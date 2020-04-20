using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSplinePointExample : MonoBehaviour
{
    public BezierMasterNS.BezierMaster bezier;

    public float speed = 5;

    Vector3 vector;


    void FixedUpdate()
    {
        for (int i = 1; i < bezier.spline.ControlPointCount; i++)
        {

            vector = bezier.spline.GetControlPointPosition(i) + Vector3.left * Mathf.Sin(Time.timeSinceLevelLoad * speed) * speed + Vector3.forward * Mathf.Cos(Time.timeSinceLevelLoad * speed) * speed;

            bezier.spline.SetControlPointPosition(i, vector);
           
        }

        bezier.UpdateMaster();
    }
}
