using System;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class ManipulatorTask
{
    /// <summary>
    /// Возвращает массив углов (shoulder, elbow, wrist),
    /// необходимых для приведения эффектора манипулятора в точку x и y 
    /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
    /// См. чертеж manipulator.png!
    /// </summary>
    public static double[] MoveManipulatorTo(double x, double y, double alpha)
    {
        // делаем по немного измененному плану из условия задачи!
        //находим координаты точки Wrist
        var wristX = x - Palm * Math.Cos(alpha);
        var wristY = y + Palm * Math.Sin(alpha);
        var distanceToWrist = Math.Sqrt(wristX * wristX + wristY * wristY); //расстояние от начала координат до точки Wrist

        var elbow = TriangleTask.GetABAngle(UpperArm, Forearm, distanceToWrist); //угол elbow
        var shoulder = TriangleTask.GetABAngle(UpperArm, distanceToWrist, Forearm) + Math.Atan2(wristY, wristX);//угол shoulder
        var wrist = -alpha - shoulder - elbow; // угол wrist
                                               //если можем достать до точки, то возвращаем углы
        if (CheckAvailability(elbow, shoulder, wrist, x, y)) return new[] { shoulder, elbow, wrist };
        //иначе плачем в подушку
        return new[] { double.NaN, double.NaN, double.NaN };
    }

    //данный метод проверяет, может ли манипулятор достать до точки
    public static bool CheckAvailability(double elbow, double shoulder, double wrist, double x, double y) // дословно "проверить доступность"
    {
        //if (elbow < 0 || shoulder < 0 || wrist < 0) return false;
        //if (elbow > Math.PI ||  shoulder > Math.PI || wrist > Math.PI) return false; //углы больше 180
        if (UpperArm + Forearm + Palm < Math.Sqrt(x * x + y * y)) return false;//расстояние до точки больше чем вся "рука"
                                                                               //в прямом состоянии
                                                                               //if (x < 0 || y < 0) return false; // координаты где то не там
        return true;
    }
}

[TestFixture]
public class ManipulatorTask_Tests
{
    [Test]
    public void TestMoveManipulatorTo()
    {
        var rnd = new Random();
        for (int i = 0; i < 10; i++)
        {
            //бесполезные тесты
            var angle = rnd.NextDouble() * Math.PI;
            var x = rnd.NextDouble() * 400;
            var y = rnd.NextDouble() * 400;
            bool check = Math.Sqrt(x * x + y * y) <= UpperArm + Palm + Forearm;
            var angles = ManipulatorTask.MoveManipulatorTo(x, y, angle);
            Assert.AreEqual(check, ManipulatorTask.CheckAvailability(angles[0], angles[1], angles[2], x, y));
        }
    }
}