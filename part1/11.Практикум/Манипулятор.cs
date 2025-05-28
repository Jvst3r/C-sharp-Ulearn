using System;
using System.Drawing;
using Avalonia;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
    /// <summary>
    /// По значению углов суставов возвращает массив координат суставов
    /// в порядке new []{elbow, wrist, palmEnd}
    /// </summary>
    public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
    {
        var elbowPos = new Point(UpperArm * Math.Cos(shoulder), //x
            UpperArm * Math.Sin(shoulder)); //y
        var wristPos = elbowPos - new Point(Forearm * Math.Cos(shoulder + elbow), //x
            Forearm * Math.Sin(shoulder + elbow)); //y
        var palmEndPos = wristPos + new Point(Palm * Math.Cos(elbow + shoulder + wrist), //x
            Palm * Math.Sin(elbow + shoulder + wrist)); //y

        return new Point[]
        {
            elbowPos,
            wristPos,
            palmEndPos
        };
    }
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
    // Доработайте эти тесты!
    // С помощью строчки TestCase можно добавлять новые тестовые данные.
    // Аргументы TestCase превратятся в аргументы метода.
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI,
        Forearm + Palm, UpperArm)]
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2,
        Forearm, UpperArm - Palm)]
    [TestCase(Math.PI / 2, 3 * Math.PI / 2, 3 * Math.PI / 2,
        -Forearm, UpperArm - Palm)]
    [TestCase(Math.PI / 2, Math.PI, 3 * Math.PI, Math.PI * 0,
        Forearm + UpperArm + Palm)]


    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
        Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
        Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
    }
}