using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierMasterNS;

public class MoveAlongCurveExample : MonoBehaviour {

    public BezierMaster bezier;
    public float speed = 5f;
    public int pathResolution = 20;

    Vector3[] points;
    public int n = 0;

	void Start () {
        points = bezier.GetPathInWorldCoordinates(pathResolution);
        n = Random.Range(0, points.Length);
    }
	

	void FixedUpdate () {

        transform.position = Vector3.MoveTowards(transform.position, points[n], speed * Time.deltaTime);

        if(Vector3.Magnitude(transform.position  - points[n]) < 0.2f)
        {
            n++;

            if (n == points.Length)
                n = 0;
        }
	}
}
