using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public static class Geometry
    {
        public static double GetLength(Vector vector)
        {
            var x = vector.X;
            var y = vector.Y;
            return Math.Sqrt(x * x + y * y);
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            Vector vectorSum = new Vector();
            vectorSum.X = vector1.X + vector2.X;
            vectorSum.Y = vector1.Y + vector2.Y;
            return vectorSum;
        }

        public static double GetLength(Segment segment)
        {
            var x = Math.Abs(segment.Begin.X - segment.End.X);
            var y = Math.Abs(segment.Begin.Y - segment.End.Y);
            return Math.Sqrt(x * x + y * y);
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            var segment1 = new Segment();
            var segment2 = new Segment();
            segment1.Begin = segment.Begin;
            segment1.End = vector;
            segment2.Begin = vector;
            segment2.End = segment.End;
            var sum = GetLength(segment1) + GetLength(segment2);
            return sum == GetLength(segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;
    }
}
