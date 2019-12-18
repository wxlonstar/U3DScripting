using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSpline : MonoBehaviour
{
    public float Delta = 3;
    public BezierMasterNS.BezierMaster bezier;

    public MeshCollider Collider;
    public MeshFilter meshFilter;

    void Start()
    {
        Collider = bezier.GetComponentInChildren<MeshCollider>();
        meshFilter = Collider.GetComponent<MeshFilter>();

        StartCoroutine(Grow()); 
    }
   
    private IEnumerator Grow()
    {
        while (isActiveAndEnabled)
        {
            Vector3 lastPoint = bezier.spline.GetControlPointPosition(bezier.spline.ControlPointCount - 1);

            Vector3 point1 = lastPoint + Random.insideUnitSphere * Delta;
            Vector3 point2 = point1 + Random.insideUnitSphere * Delta;
            Vector3 point3 = point2 + Random.insideUnitSphere * Delta;

            bezier.spline.AddCurve(point1, point2, point3, BezierMasterNS.BezierControlPointMode.Mirrored);

            bezier.spline.RemoveCurve(0);

            bezier.UpdateMaster();


            // THATS NEEDED FOR UPDATE MESH COLLIDER
            Collider.sharedMesh = meshFilter.mesh;


            yield return new WaitForSeconds(1);
        }
    }
}
