//
//Large part from catlikecoding.com Curves And Splines Tutorial
//Thanks to Jasper Flick 
//
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;


namespace BezierMasterNS
{
    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }

    [Serializable]
    public class BezierSpline : ScriptableObject
    {
        [SerializeField]
        private Vector3[] points;

        [SerializeField]
        private BezierControlPointMode[] modes;

        [SerializeField]
        public float[] zRotationAtPoint;

        [SerializeField]
        public Vector3[] scaleFactor3d;

        [SerializeField]
        private bool loop;
        private const int lenghtResolution = 10;

        public bool Loop
        {
            get
            {
                return loop;
            }
            set
            {

                loop = value;
                if (value == true)
                {
                    float dist = Vector3.Distance(GetControlPointPosition(0), GetControlPointPosition(ControlPointCount));

                    //float dist2 = Vector3.Distance(GetControlPointPosition(ControlPointCount - 2), GetControlPointPosition(ControlPointCount));

                    Debug.Log(dist + " > " + GetCurveLenght() * 0.06f);

                    if (dist > GetCurveLenght() * 0.06f)
                        AddCurve();

                    modes[modes.Length - 1] = modes[0];
                    zRotationAtPoint[zRotationAtPoint.Length - 1] = zRotationAtPoint[0];
                    scaleFactor3d[scaleFactor3d.Length - 1] = scaleFactor3d[0];
                    SetControlPointPosition(points.Length - 1, points[0]);
                }
            }
        }

        public int ControlPointCount
        {
            get
            {
                return points.Length;
            }
        }
        public int CurveCount
        {
            get
            {
                return (points.Length - 1) / 3;
            }
        }


        public static BezierSpline CreateSpline()
        {
            var spline = CreateInstance<BezierSpline>();
            //Debug.Log("spline created");
            spline.Reset();
            return spline;
        }
        public static BezierSpline CreateSpline(BezierSpline reference)
        {
            var spline = Instantiate(reference);
            //Debug.Log("spline created");
            return spline;
        }

        public void Reset()
        {
            points = new Vector3[] {
            new Vector3(-20f, 0f, -20f),
            new Vector3(10f, 0f, 10f),
            new Vector3(40f, 0f, 40f),
            new Vector3(80f, 0f, 80f)
        };

            modes = new BezierControlPointMode[] {
            BezierControlPointMode.Aligned,
            BezierControlPointMode.Aligned
        };

            zRotationAtPoint = new float[] { 0, 0 };

            scaleFactor3d = new Vector3[] { Vector3.one, Vector3.one };

            loop = false;
        }     

        public Vector3 GetControlPointPosition(int index)
        {
            return points[index > points.Length - 1 ? points.Length - 1 : index];
        }

        public void SetControlPointPosition(int index, Vector3 point)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - points[index];
                if (loop)
                {
                    if (index == 0)
                    {
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[points.Length - 1] = point;
                    }
                    else if (index == points.Length - 1)
                    {
                        points[0] = point;
                        points[1] += delta;
                        points[index - 1] += delta;
                    }
                    else
                    {
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        points[index - 1] += delta;
                    }
                    if (index + 1 < points.Length)
                    {
                        points[index + 1] += delta;
                    }
                }
            }
            points[index] = point;
            EnforceMode(index);
        }

        public Vector3 GetPointOnCurve(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return MathCalculations.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3], t);
        }

        const int lenghtCheckPressijn = 20;

        public float GetLenghtFromStart(float t)
        {
            float lenght = 0;
            Vector3 lastPoint = GetPointOnCurve(0);

            float l = 0;

            int count = Mathf.Clamp((int)(t * lenghtCheckPressijn), 1, lenghtCheckPressijn);

            for(int i = 0; i < count; i++)
            {
                l += t / count;

                var next = GetPointOnCurve(l);
                lenght += Vector3.Magnitude(next - lastPoint);
                lastPoint = next;           
            }

            return lenght;
        }

        public float GetRotationZ(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = zRotationAtPoint.Length - 1;

                return zRotationAtPoint[i];
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = Mathf.FloorToInt(t);
                t -= i;
            }


            float rot = Mathf.Lerp(zRotationAtPoint[i], zRotationAtPoint[i + 1], t);
            return rot;
        }

        public Vector3 GetScale(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = scaleFactor3d.Length - 1;

                return scaleFactor3d[i];
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
            }

            Vector3 scale = Vector3.Lerp(scaleFactor3d[i], scaleFactor3d[i + 1], t);
            return scale;
        }

        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return MathCalculations.GetFirstDerivative(
                points[i], points[i + 1], points[i + 2], points[i + 3], t);
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public float GetCurveLenght()
        {
            float lenght = 0;
            Vector3 lastPoint = GetPointOnCurve(0);

            for (int i = 1; i < lenghtResolution; i++)
            {
                var next = GetPointOnCurve((float)i / lenghtResolution);
                lenght += Vector3.Magnitude(next - lastPoint);
                lastPoint = next;
            }

            return lenght;
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            int ind = (index + 1) / 3;

            return modes[ind >= modes.Length ? 0 : ind];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;
            if (loop)
            {
                if (modeIndex == 0)
                {
                    modes[modes.Length - 1] = mode;
                }
                else if (modeIndex == modes.Length - 1)
                {
                    modes[0] = mode;
                }
            }
            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
            {
                return;
            }
            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;

            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = points.Length - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Length)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = points.Length - 2;
                }
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
            }
            points[enforcedIndex] = middle + enforcedTangent;
        }

        public void AddCurve()
        {
            Vector3 point = points[points.Length - 1];

            Vector3 delta =  point - points[points.Length - 2];

            Array.Resize(ref points, points.Length + 3);
            point += delta;
            points[points.Length - 3] = point;
            point += delta;
            points[points.Length - 2] = point;
            point += delta;
            points[points.Length - 1] = point;

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Length - 4);

            Array.Resize(ref zRotationAtPoint, modes.Length);
            Array.Resize(ref scaleFactor3d, modes.Length);
            scaleFactor3d[scaleFactor3d.Length - 1] = Vector3.one;

            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                zRotationAtPoint[zRotationAtPoint.Length - 1] = zRotationAtPoint[0];
                scaleFactor3d[scaleFactor3d.Length - 1] = scaleFactor3d[0];
                EnforceMode(0);
            }
        }
        public void AddCurve(Vector3 point1, Vector3 point2, Vector3 point3, BezierControlPointMode mode = BezierControlPointMode.Aligned)
        {
            Vector3 point0 = points[points.Length - 1];

            //Vector3 delta = point0 - points[points.Length - 2];

            Array.Resize(ref points, points.Length + 3);
          
            points[points.Length - 3] = point1;
            points[points.Length - 2] = point2;
            points[points.Length - 1] = point3;

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = mode;
            EnforceMode(points.Length - 4);

            Array.Resize(ref zRotationAtPoint, modes.Length);
            Array.Resize(ref scaleFactor3d, modes.Length);
            scaleFactor3d[scaleFactor3d.Length - 1] = Vector3.one;

            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                zRotationAtPoint[zRotationAtPoint.Length - 1] = zRotationAtPoint[0];
                scaleFactor3d[scaleFactor3d.Length - 1] = scaleFactor3d[0];
                EnforceMode(0);
            }
        }
        public void InsertPoint(int index)
        {
            // Vector3 point = points[points.Length - 1];

            //Vector3 delta = point - points[points.Length - 2];

            if (index < 0 || index >= points.Length-1)
            {
                Debug.LogWarning("You need select point first"); 
                return;
            }
              

            List<Vector3> pointsList = new List<Vector3>(points);


            if (index % 3 == 1)
            {
                index++;
            }
            else if (index % 3 == 0)
            {
                index += 2;
            }
            else if (index % 3 == 2)
            {
                index += 3;
            }


            float t = (float)index / ControlPointCount;
            float deltaT = 0.5f / ControlPointCount;

            Vector3 point = GetPointOnCurve(t + 1.5f * deltaT);
            Vector3 point2 = GetPointOnCurve(t + deltaT);
            Vector3 point3 = GetPointOnCurve(t + 0.5f * deltaT); 

            pointsList.Insert(index, point);       
            pointsList.Insert(index, point2);
            pointsList.Insert(index, point3);

            points = pointsList.ToArray();

            int modeIndex = (index + 1) / 3;

            int newLenght = modes.Length + 1;

            Array.Resize(ref modes, newLenght);
            Array.Resize(ref zRotationAtPoint, newLenght);
            Array.Resize(ref scaleFactor3d, newLenght);

            //modes[newLenght - 1] = modes[newLenght - 2];


            //modes[modeIndex] = modes[modeIndex];

            for (int i = newLenght - 1; i <= modeIndex; i--)
            {
                modes[i] = modes[i - 1];
                zRotationAtPoint[i] = zRotationAtPoint[i - 1];
                scaleFactor3d[i] = scaleFactor3d[i - 1];
            }

           
            //EnforceMode(index);

           
          
            /*
            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                zRotationAtPoint[zRotationAtPoint.Length - 1] = zRotationAtPoint[0];
                scaleFactor3d[scaleFactor3d.Length - 1] = scaleFactor3d[0];
                EnforceMode(0);
            }*/
        }
        public void RemoveCurve(int index)
        {

            if (CurveCount <= 1 || index < 0)
                return;

            if (index > 1 && index % 3 != 2)
                index -= 1 + index % 3;
            else if (index == 1)
                index--;


            for (int i = index; i < points.Length - 3; i++)
            {
                int modeIndex = (i + 1) / 3;
                points[i] = points[i + 3];

                if (modeIndex < CurveCount)
                {
                    modes[modeIndex] = modes[modeIndex + 1];
                    zRotationAtPoint[modeIndex] = zRotationAtPoint[modeIndex + 1];
                }
            }


            Array.Resize(ref points, points.Length - 3);
            Array.Resize(ref modes, modes.Length - 1);
            Array.Resize(ref zRotationAtPoint, modes.Length);
            Array.Resize(ref scaleFactor3d, modes.Length);

            EnforceMode(points.Length - 4);

            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                zRotationAtPoint[zRotationAtPoint.Length - 1] = zRotationAtPoint[0];
                scaleFactor3d[scaleFactor3d.Length - 1] = scaleFactor3d[0];
                EnforceMode(0);
            }

        }

      
    }

    
  
}