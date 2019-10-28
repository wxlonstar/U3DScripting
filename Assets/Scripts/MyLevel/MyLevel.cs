using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MileCode.MileTest {
    public partial class MyLevel : MonoBehaviour {
        [SerializeField]
        private int _totolTime = 60;
        [SerializeField]
        private float gravity = -30;
        [SerializeField]
        private AudioClip bgm;
        [SerializeField]
        private Sprite background;
        [SerializeField]
        private int _totalColumns = 25;
        [SerializeField]
        private int _totalRows = 10;

        public const float GridSize = 1.28f;

        private readonly Color _normalColor = Color.grey;

        private readonly Color _selectedColor = Color.green;



        
        public int TotalTime {
            get {
                return _totolTime;
            }
            set {
                _totolTime = value;
            }
        }

        public float Gravity {
            get {
                return gravity;
            }
            set {
                gravity = value;
            }
        }

        public AudioClip Bgm {
            get {
                return bgm;
            }
            set {
                bgm = value;
            }
        }

        public Sprite Background {
            get {
                return background;
            }
            set {
                background = value;
            }
        }

        public int TotalColumns {
            get {
                return TotalColumns;
            }
            set {
                _totalColumns = value;
            }
        }

        public int TotalRows {
            get {
                return _totalRows;
            }
            set {
                _totalRows = value;
            }
        }

        // this method draws borders
        private void GridFrameGizmo(int cols, int rows) { 
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, rows * GridSize, 0));      //draw left line
            Gizmos.DrawLine(Vector3.zero, new Vector3(cols * GridSize, 0, 0));          //draw buttom line
            Gizmos.DrawLine(new Vector3(cols * GridSize, 0, 0), new Vector3(cols * GridSize, rows * GridSize, 0));      //draw right line
            Gizmos.DrawLine(new Vector3(0, rows * GridSize, 0), new Vector3(cols * GridSize, rows * GridSize, 0));      //draw top line
        }

        // this method draws patterns
        private void GridGizmo(int cols, int rows) { 
            for(int i = 1; i < cols; i++ ) {
                Gizmos.DrawLine(new Vector3(i * GridSize, 0, 0), new Vector3(i * GridSize, rows * GridSize, 0));        //Draw column lines
            }
            for(int j = 1; j < rows; j++ ) {
                Gizmos.DrawLine(new Vector3(0, j * GridSize, 0), new Vector3(cols * GridSize, j * GridSize, 0));
            }

        }

        private void OnDrawGizmos() {
            Color olderColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = _normalColor;
            GridGizmo(_totalColumns, _totalRows);
            GridFrameGizmo(_totalColumns, _totalRows);

            Gizmos.color = olderColor;
            Gizmos.matrix = oldMatrix;
        }

        private void OnDrawGizmosSelected() {
            Color olderColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;            // save matrix for gizimo first
            Gizmos.matrix = this.transform.localToWorldMatrix;      // make tranform matrix gizimo's


            Gizmos.color = _selectedColor;
            GridFrameGizmo(_totalColumns, _totalRows);
            Gizmos.color = olderColor;
            Gizmos.matrix = oldMatrix;
        }

        public Vector3 WorldToGridCoordinates(Vector3 point) {
            Vector3 gridPoint = new Vector3(
                    (int)((point.x - transform.position.x) / GridSize),
                    (int)((point.y - transform.position.y) / GridSize), 0.0f
                );
            return gridPoint;
        }

        public Vector3 GridToWorldCoordinates(int col, int row) {
            Vector3 worldPoint = new Vector3(
                    transform.position.x + (col * GridSize + GridSize / 2.0f),
                    transform.position.y + (row * GridSize + GridSize / 2.0f),
                    0.0f
                );
            return worldPoint;
        }

        public bool IsInsideGridBounds(Vector3 point) {
            float minX = transform.position.x;
            float maxX = minX + _totalColumns * GridSize;
            float minY = transform.position.y;
            float maxY = minY + _totalRows * GridSize;
            return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
        }

        public bool IsInsideGridBounds(int col, int row) {
            return (col >= 0 && col < _totalColumns && row >= 0 && row < _totalRows);
        }



    }
}
