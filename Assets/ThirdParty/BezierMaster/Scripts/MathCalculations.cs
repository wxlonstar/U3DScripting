using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCalculations
{

    public static Vector3 RotateVector(Vector3 baseVector, Vector3 axis, float deg)
    {
        float angle = deg * Mathf.Deg2Rad;

        float x = axis.x;
        float y = axis.y;
        float z = axis.z;

        float c = Mathf.Cos(angle);
        float s = Mathf.Sin(angle);
        float c1 = 1 - c;

        float[,] rotationMatrix = new float[,] { { c + c1 * x * x, c1 * x * y - z * s, x * z * c1 + y * s },
                                                 { x * y * c1 + z * s, y * y * c1 + c, y * z * c1 - x * s },
                                                 { x * z * c1 - y * s, x * z * c1 + x * s, z * z * c1 + c } };

        Vector3 result = new Vector3(baseVector.x * rotationMatrix[0, 0] + baseVector.y * rotationMatrix[1, 0] + baseVector.z * rotationMatrix[2, 0],
                                     baseVector.x * rotationMatrix[0, 1] + baseVector.y * rotationMatrix[1, 1] + baseVector.z * rotationMatrix[2, 1],
                                     baseVector.x * rotationMatrix[0, 2] + baseVector.y * rotationMatrix[1, 2] + baseVector.z * rotationMatrix[2, 2]);

        return result;
    }

    /// <summary>
    /// From catlikecoding.com Curves And Splines Tutorial
    /// Thanks to Jasper Flick
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
    /// <summary> 
    /// From catlikecoding.com Curves And Splines Tutorial
    /// Thanks to Jasper Flick 
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}
