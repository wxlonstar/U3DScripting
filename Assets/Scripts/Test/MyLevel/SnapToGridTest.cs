using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MileCode.MileTest;

[ExecuteInEditMode]
public class SnapToGridTest : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 gridCoord = MyLevel.Instance.WorldToGridCoordinates(transform.position);
        transform.position = MyLevel.Instance.GridToWorldCoordinates((int)gridCoord.x, (int)gridCoord.y);
    }

    private void OnDrawGizmos() {
        Color oldColor = Gizmos.color;
        Gizmos.color = (MyLevel.Instance.IsInsideGridBounds(transform.position)) ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * MyLevel.GridSize);
        Gizmos.color = oldColor;
    }
}
