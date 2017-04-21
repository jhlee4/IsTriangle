using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Geometry;

namespace IsTriangle
{
    class CurveToLineConversion
    {
        public static List<Line> Conversion(Curve[] segments)
        {
            List<Line> lines = new List<Line>();
            for(int i = 0; i < segments.Length; i++)
            {
                lines.Add(new Line(segments[i].PointAtStart, segments[i].PointAtEnd));
            }
            return lines;
        }
    }
}
