using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hey))]
class testGizmo : Editor {       // The Class still works, but will be compiled when building
    [DrawGizmo(GizmoType.InSelectionHierarchy |     
        GizmoType.NotInSelectionHierarchy |
        GizmoType.Active
        )]
    private static void MyGizmos(Hey h, GizmoType gizmoType) {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(h.transform.position, Vector3.one);
        Gizmos.DrawIcon(h.transform.position, "GizmoIcon01");       // "GizmoIcon01" is a png texture in Assets/Gizmos/ folder

    }
}