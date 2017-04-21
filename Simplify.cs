using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Geometry;

namespace IsTriangle
{
    class Simplify
    {
        public static Curve Simplification(Curve shape, int epsilon)
        {
            if (shape.ClosedCurveOrientation(Vector3d.ZAxis) == CurveOrientation.CounterClockwise)
            {
                shape.Reverse();
            }
            Curve[] shapeSegments = shape.DuplicateSegments();
            List<Line> shapeLineSegs = LineConversion(shapeSegments);
            List<Point3d> finalPoints = new List<Point3d>();
            for (int i = 0; i < shapeLineSegs.Count; i++)
            {
                finalPoints.Add(shapeLineSegs[i].From);
            }
            finalPoints.Add(shapeLineSegs[shapeLineSegs.Count - 1].To);
            List<Point3d> simplifiedPoints = Douglas(finalPoints, epsilon);
            Curve simplifiedCurve = new Polyline(simplifiedPoints).ToNurbsCurve();
            return simplifiedCurve;
        }

        public static List<Line> LineConversion(Curve[] segments)
        {
            List<Line> shapeLineSeg = new List<Line>();
            for (int i = 0; i < segments.Length; i++)
            {
                Line line = new Line(segments[i].PointAtStart, segments[i].PointAtEnd);
                shapeLineSeg.Add(line);
            }
            return shapeLineSeg;
        }

        public static List<Point3d> Douglas(List<Point3d> validPoints, double epsilon)
        {
            double maxDistance = 0;
            int index = 0;
            int end = validPoints.Count;
            for (int i = 0; i < end; i++)
            {
                Point3d p1 = new Line(validPoints[0], validPoints[end - 1]).ClosestPoint(validPoints[i], false);
                double distance = new Line(p1, validPoints[i]).Length;

                if (distance > maxDistance)
                {
                    index = i;
                    maxDistance = distance;
                }
            }

            List<Point3d> finalResult = new List<Point3d>();
            if (maxDistance > epsilon)
            {
                List<Point3d> result1 = new List<Point3d>();
                List<Point3d> vp1 = new List<Point3d>();
                for (int i = 0; i < index + 1; i++)
                {
                    vp1.Add(validPoints[i]);
                }
                List<Point3d> result2 = new List<Point3d>();
                List<Point3d> vp2 = new List<Point3d>();
                for (int i = index + 1; i < end; i++)
                {
                    vp2.Add(validPoints[i]);
                }

                result1 = Douglas(vp1, epsilon);
                result2 = Douglas(vp2, epsilon);

                finalResult.AddRange(result1);
                finalResult.AddRange(result2);
            }
            else
            {
                finalResult.Add(validPoints[0]);
                finalResult.Add(validPoints[end - 1]);
            }

            return finalResult;
        }
    }
}
