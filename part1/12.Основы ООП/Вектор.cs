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
    }
}
