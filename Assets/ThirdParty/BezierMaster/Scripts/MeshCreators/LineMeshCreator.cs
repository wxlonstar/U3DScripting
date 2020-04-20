using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BezierMasterNS.MeshesCreating
{
   
    public class LineMeshCreator : CreateMeshBase
    {

        /// <summary>
        /// Create instance of mesh generator
        /// </summary>
        /// <param name="spline"></param>
        /// <returns></returns>
        public static LineMeshCreator CreateLineMesh(BezierSpline spline)
        {
            var line = CreateInstance<LineMeshCreator>();
            line.spline = spline;
            return line;
        }
        public override Mesh CreateMesh()
        {
            SetVertices();
            SetTriangles();

            var mesh = new Mesh();
            mesh.name = "LineMesh";

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;


            return mesh;
        }

        public override MeshCreatorType GetMeshCreatorType()
        {
            return MeshCreatorType.Line;
        }

       

        void SetVertices()
        {
            int index = 0;

            int vertexCount = !twoSided ? lenghtSegmentsCount * widhtSegmentsCount : lenghtSegmentsCount * widhtSegmentsCount * 2;

            normals = new Vector3[vertexCount];
            vertices = new Vector3[vertexCount];
            uv = new Vector2[vertexCount];

            for (int i = 0; i < lenghtSegmentsCount; i++)
            {
                float t = (i) / (float)(lenghtSegmentsCount - 1);

                Vector3 point = spline.GetPointOnCurve(t);

                Vector3 forward = spline.GetDirection(t);


                Vector3 perpendicular = -Vector3.Cross( forward, Vector3.up);
            
                Vector3 result = MathCalculations.RotateVector(perpendicular, forward, spline.GetRotationZ(t)) ;

                perpendicular = result.normalized * Radius1 * spline.GetScale(t).magnitude;

                Vector3 perpendicularUp = Vector3.Cross(forward, perpendicular);

                point += perpendicular;

                for (int n = 0; n < widhtSegmentsCount; n++)
                {
                    float r = n / (float)(widhtSegmentsCount - 1);
                    normals[index] = perpendicularUp;
                    vertices[index] = point - perpendicular * r * 2;

                    float uvY = fixTextureStretching ? spline.GetLenghtFromStart(t) / spline.GetCurveLenght() : t;
                     uvY *= textureScale.y;

                    float uvX = r * textureScale.x;
                    // uvY 

                    Vector2 vertexUV = textureOrientation ? new Vector2(uvY, uvX) : new Vector2(uvX, uvY);

                    uv[index] = vertexUV;

                    index++;
                }
            }

            if (twoSided)
                for (int i = 0; i < lenghtSegmentsCount; i++)
                {
                    float t = (i) / (float)(lenghtSegmentsCount - 1);

                    Vector3 point = spline.GetPointOnCurve(t);

                    Vector3 forward = spline.GetDirection(t);

                    Vector3 perpendicular = -Vector3.Cross(forward, Vector3.up);

                    Vector3 result = MathCalculations.RotateVector(perpendicular, forward, spline.GetRotationZ(t));

                    perpendicular = result.normalized * Radius1 * spline.GetScale(t).magnitude;

                    Vector3 perpendicularUp = Vector3.Cross(forward, perpendicular);

                    point += perpendicular;

                    for (int n = 0; n < widhtSegmentsCount; n++)
                    {
                        float r = n / (float)(widhtSegmentsCount - 1);
                        normals[index] = -perpendicularUp;


                        vertices[index] = point - perpendicular * r * 2;

                        float uvY = fixTextureStretching ? spline.GetLenghtFromStart(t) / spline.GetCurveLenght() : t;
                        uvY *= textureScale.y;

                        float uvX = r * textureScale.x;

                        Vector2 vertexUV = textureOrientation ? new Vector2(uvY, uvX) : new Vector2(uvX, uvY);

                        uv[index] = vertexUV;

                        index++;
                    }
                }
        }
     
        void SetTriangles()
        {
            int trianglesCount = (widhtSegmentsCount - 1) * 6 * (lenghtSegmentsCount - 1);
            int _index = 0;

            if (twoSided)
                trianglesCount *= 2;

            triangles = new int[trianglesCount];

            for (int i = 0; i < lenghtSegmentsCount - 1; i++)
            {
                for (int j = 0; j < widhtSegmentsCount - 1; j++)
                {
                    triangles[_index] = (widhtSegmentsCount) * i + j;
                    triangles[_index + 1] = (widhtSegmentsCount) * i + 1 + j;
                    triangles[_index + 2] = (widhtSegmentsCount) * (1 + i) + j;

                    triangles[_index + 3] = triangles[_index + 1];
                    triangles[_index + 4] = (widhtSegmentsCount) * (1 + i) + 1 + j;
                    triangles[_index + 5] = triangles[_index + 2];

                    _index += 6;
                }
            }

            int offset = lenghtSegmentsCount * widhtSegmentsCount;
            if (twoSided)
            {
                for (int i = 0; i < lenghtSegmentsCount - 1; i++)
                {
                    for (int j = 0; j < widhtSegmentsCount - 1; j++)
                    {
                        triangles[_index] = offset + (widhtSegmentsCount) * i + j;
                        triangles[_index + 2] = offset + (widhtSegmentsCount) * i + 1 + j;
                        triangles[_index + 1] = offset + (widhtSegmentsCount) * (1 + i) + j;

                        triangles[_index + 3] = triangles[_index + 1];
                        triangles[_index + 4] = offset + (widhtSegmentsCount) * (1 + i) + 1 + j;
                        triangles[_index + 5] = triangles[_index + 2];

                        _index += 6;
                    }
                }
            }
        }
    }
}