using Avalonia.Controls.Shapes;
using System;

namespace DistanceTask;

public static class DistanceTask
{
    //метод нахождения длины отрезка АВ
    public static double GetDistance(double ax, double ay, double bx, double by)
    {
        return Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));
    }

    public static bool GetAngle(double lengthOfSection, double a, double b)
    {
        return (a * a + b * b - lengthOfSection * lengthOfSection) / (2 * a * b) < 0; // если true - тупой, если false - прямой
    }

    // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        double distanceAB = GetDistance(ax, ay, bx, by); //Длина отрезка АВ
        double distanceAC = GetDistance(ax, ay, x, y); // Длина между концов А отрезка АВ и точкой С
        double distanceBC = GetDistance(bx, by, x, y); //Длина между концом В отрезка АВ и точкой С

        if ((ax == x && ay == y) || (bx == x && by == y)) return 0.0; //если точка лежит на прямой
        if (distanceAB == 0) return distanceAC; //если дан отрезок - точка
        if (GetAngle(distanceAC, distanceBC, distanceAB)) return distanceBC; // близжайшее расстояние это BC
        if (GetAngle(distanceBC, distanceAC, distanceAB)) return distanceAC; // близжайшее расстояние это AB
        double p = (distanceAC + distanceBC + distanceAB) / 2;
        return 2 * Math.Sqrt(p * (p - distanceAB) * (p - distanceBC) * (p - distanceAC)) / distanceAB;
    }
}