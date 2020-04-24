using UnityEngine;
using UnityEditor;

namespace MileCode.MileTest {

    public class DrawGizmoExample {
        // attribute means when to to draw gizmos
        [DrawGizmo(GizmoType.NotInSelectionHierarchy |
                GizmoType.InSelectionHierarchy |
                GizmoType.Selected |
                GizmoType.Active |
                GizmoType.Pickable
            )]
        private static void MyCustomOnDrawGizmos(TargetExample targetExample, GizmoType gizmoType) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(targetExample.transform.position, Vector3.one);
            //Gizmos.DrawIcon(targetExample.transform.position, "zup!");
           
            
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy |
                GizmoType.Active
            )]
        private static void MyCustomOnDrawGizmosSelected(TargetExample targetExample, GizmoType gizmoType) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetExample.transform.position, 0.2f);

        }
    }
}