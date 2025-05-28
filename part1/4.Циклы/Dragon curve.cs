using System;

namespace Fractals;

internal static class DragonFractalTask
{
    public static double GetCoordinateX(double x, double y, double angle)
    {
        return (x * Math.Cos(angle) - y * Math.Sin(angle)) / Math.Sqrt(2);
    }

    public static double GetCoordinateY(double x, double y, double angle)
    {
        return (x * Math.Sin(angle) + y * Math.Cos(angle)) / Math.Sqrt(2);
    }

    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        var randomNumber = new Random(seed);
        var x = 1.0;
        var y = 0.0;

        for (int i = 0; i < iterationsCount; ++i) //это очень трудно для понимания в час ночи((
        // чтоб понять что в википедии понять надо мозгов больше чем у меня
        {
            var lastX = x;
            var lastY = y;
            if (randomNumber.Next(2) == 0)
            {
                x = GetCoordinateX(x, y, Math.PI / 4);
                y = GetCoordinateY(lastX, y, Math.PI / 4);
            }
            else
            {
                x = GetCoordinateX(x, y, Math.PI * 3 / 4) + 1;
                y = GetCoordinateY(lastX, y, Math.PI * 3 / 4);
            }
            pixels.SetPixel(x, y);
        }
    }
}