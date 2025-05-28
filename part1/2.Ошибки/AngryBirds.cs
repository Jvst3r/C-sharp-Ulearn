using System;
namespace AngryBirds;

public static class AngryBirdsTask
{
    public static double FindSightAngle(double v, double distance)
    {
        var g = 9.8;//ускорение свободного падения
        var sin2a = (g * distance) / (v * v); //sin2a для упрощения записи return
        return 0.5 * Math.Asin(sin2a); //искомый угол
    }
}
