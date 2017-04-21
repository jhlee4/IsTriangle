using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Geometry;
namespace IsTriangle
{
    class TriIdentifier
    {
        public static bool Identify(Curve land)
        {
            bool isTri = false;
            //set the orientation of the curve to counter clockwise
            if (land.ClosedCurveOrientation(Vector3d.ZAxis) == CurveOrientation.CounterClockwise)
            {
                land.Reverse();
            }

            //find the inner angle of each points
            Curve[] segments = land.DuplicateSegments();
            List<Line> lineSegments = CurveToLineConversion.Conversion(segments);
            List<double> innerAngles = new List<double>();
            
            for (int i = 0; i < lineSegments.Count; i++)
            {
               Line l1 = lineSegments[i];
                Line l2 = lineSegments[(i + 1) % lineSegments.Count];
                double a, b;
                Vector3d v1, v2;
                Rhino.Geometry.Intersect.Intersection.LineLine(l1,l2,out a,out b,0,false);
                if(a>0 && b <= 0)
                {
                   v1 = lineSegments[i].UnitTangent*-1;
                   v2 = lineSegments[(i + 1) % lineSegments.Count].UnitTangent;
                }else if (b > 0 && a <= 0)
                {
                    v1 = lineSegments[i].UnitTangent;
                    v2 = lineSegments[(i + 1) % lineSegments.Count].UnitTangent *-1;
                }else if (b > 0 && a > 0)
                {
                    v1 = lineSegments[i].UnitTangent*-1;
                    v2 = lineSegments[(i + 1) % lineSegments.Count].UnitTangent * -1;
                }else
                {
                    v1 = lineSegments[i].UnitTangent;
                    v2 = lineSegments[(i + 1) % lineSegments.Count].UnitTangent;
                }
                double innerAngle = RhinoMath.ToDegrees(Vector3d.VectorAngle(v2,v1));
                innerAngles.Add(innerAngle);
            }
         
                //check the inner angles and include points 
                List<Point3d> validPoints = new List<Point3d>();
                for (int i = 0; i < innerAngles.Count; i++)
                {
                    double a1 = innerAngles[i];
               // double a2 = innerAngles[(i+1)%innerAngles.Count];
                if (a1 >150)
                    {
                        continue;
                    }
                    else
                    {
                        validPoints.Add(lineSegments[i].From);
                    }
                }
                //check if its tri or not
                if (validPoints.Count == 3 || lineSegments.Count ==3)
                {
                    isTri = true;
            }else{
                    isTri = false;
                }
            
            return isTri;
        }//method : Identify
    }//class : TriIdentifier
}//namespace : isTriangle
