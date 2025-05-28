using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask
{
    /// <summary>
    /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
    /// </summary>
    public static double GetABAngle(double a, double b, double c)
    {
        if (a == b && a > 0 && c == 0) //треугольник существует с нулевым углом
            return 0.0;
        if (a > 0 && b > 0 && c > 0) // обычный случай
            return Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        return double.NaN; //такого треугольника не существует
    }
}

[TestFixture]
public class TriangleTask_Tests
{
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(0, 2, 3, double.NaN)]
    [TestCase(1, 0, 3, double.NaN)]
    [TestCase(1, -1, 1, double.NaN)]
    [TestCase(1, 2, 0, double.NaN)]
    // добавьте ещё тестовых случаев!
    public void TestGetABAngle(double a, double b, double c, double expectedAngle)
    {
        var actualAngle = TriangleTask.GetABAngle(a, b, c);
        Assert.AreEqual(actualAngle, expectedAngle, 9.9999999999999995E-08d);
    }
}