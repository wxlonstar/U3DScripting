using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GizmoHelper : MonoBehaviour {
    public Transform x;
    public Transform y;
    public Transform z;
    // Start is called before the first frame update

    private void OnDrawGizmos() {
        Color col = Gizmos.color;
        DrawX(x);
        DrawY(y);
        DrawZ(z);
    }

    private void DrawX(Transform x) {
        Color OldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, x.transform.position);
        Gizmos.color = OldColor;
    }

    private void DrawY(Transform y) {
        Color OldColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, y.transform.position);
        Gizmos.color = OldColor;
    }
    private void DrawZ(Transform z) {
        Color OldColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, z.transform.position);
        Gizmos.color = OldColor;
    }
}
