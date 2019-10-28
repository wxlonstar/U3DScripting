using UnityEngine;

namespace MileCode.MileTest {
    class UseGizmo : MonoBehaviour {
        // this method will be called when the scirpt is not collapsed, constantly.
        private void OnDrawGizmos() {
            Debug.Log("Gizmoing");
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
            Gizmos.DrawWireSphere(transform.position, 1f);
            Gizmos.DrawLine(transform.position, transform.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }

        // this method will be called when the gizmo of the gameobject is selected, constantly
        private void OnDrawGizmosSelected() {
            Debug.Log("Gizmo Selected");
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }

}