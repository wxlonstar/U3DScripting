using System;
using System.Collections.Generic;
using UnityEngine;

namespace BezierMasterNS.MeshesCreating
{
    public enum MeshCreatorType
    {
        Line,
        Cylinder,
        Tube
    }

    public abstract class CreateMeshBase : ScriptableObject
    {
        public BezierSpline spline;

        public int lenghtSegmentsCount = 20;       
        public int widhtSegmentsCount = 6;
     
        public float Radius1 = 5;

        protected Vector3[] vertices = new Vector3[0];
        protected Vector3[] normals = new Vector3[0];
        protected int[] triangles = new int[0];
        protected Vector2[] uv;

        public bool twoSided = false;
        public bool textureOrientation = false;
        public bool fixTextureStretching = false;

        public Vector2 textureScale = Vector2.one;

        public int GetVertexCount => vertices.Length;      
        public int GetTrianglesCount => triangles.Length / 3;
        


        public abstract MeshCreatorType GetMeshCreatorType();

        public abstract Mesh CreateMesh(); 

        /// <summary>
        /// Create instance of mesh generator
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static CreateMeshBase InstatiateCreator(CreateMeshBase reference)
        {
            //Debug.Log("CreateMeshBase created");
            return Instantiate(reference);
        }

        /// <summary>
        /// Create mesh generator of type
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static CreateMeshBase InstatiateCreator(Type t) 
        {
            if (t.IsSubclassOf(typeof(CreateMeshBase)))
                return CreateInstance(t) as CreateMeshBase;
            else
            {
                Debug.LogWarning("Invalid type! Type must be derived of CreateMeshBase!");
                return null;
            }
                
        }
    }
}
