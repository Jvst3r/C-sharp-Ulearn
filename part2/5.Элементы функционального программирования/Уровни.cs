using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new Physics();
    static readonly Rocket standardRocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
    static readonly Vector standardTarget = new Vector(600, 200);
    static readonly Vector targetForUpLevel = new Vector(700, 500);

    //количество хардкода поражает воображение (4 версия кода все еще поражает)
    public static IEnumerable<Level> CreateLevels()
    {
        yield return CreateZeroLevel();
        yield return CreateHeavyLevel();
        yield return CreateUpLevel();
        yield return CreateWhiteHoleLevel();
        yield return CreateBlackHoleLevel();
        yield return CreateBlackAndWhiteLevel();
    }

    private static Level CreateZeroLevel()
    {
        return new Level("Zero",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
        (size, v) => Vector.Zero,
            standardPhysics
        );
    }

    private static Level CreateHeavyLevel()
    {
        return new Level("Heavy",
            standardRocket,
            standardTarget,
            (size, v) => new Vector(0, 0.9),
            standardPhysics
        );
    }

    private static Level CreateUpLevel()
    {
        // минус не трогать!!!
        return new Level("Up",
            standardRocket,
            targetForUpLevel,
            (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300.0)),
            standardPhysics
        );
    }

    private static Level CreateWhiteHoleLevel()
    {
        return new Level("WhiteHole",
            standardRocket,
            standardTarget,
            (size, v) =>
            {
                var delta = v - standardTarget;
                return 140 * delta / (delta.Length * delta.Length + 1);
            },
            standardPhysics
        );
    }

    private static Level CreateBlackHoleLevel()
    {
        return new Level("BlackHole",
            standardRocket,
            standardTarget,
            (size, v) =>
            {
                var delta = (standardTarget + standardRocket.Location) / 2 - v;
                return 300 * delta / (delta.Length * delta.Length + 1);
            },
            standardPhysics
        );
    }

    private static Level CreateBlackAndWhiteLevel()
    {
        return new Level("BlackAndWhite",
            standardRocket,
            standardTarget,
            (size, v) =>
            {
                var blackHoleDelta = (standardTarget + standardRocket.Location) / 2 - v;
                var whiteHoleDelta = v - standardTarget;
                var sum = (
                    (300 * blackHoleDelta / (blackHoleDelta.Length * blackHoleDelta.Length + 1)) +
                    (140 * whiteHoleDelta / (whiteHoleDelta.Length * whiteHoleDelta.Length + 1))
                ) / 2;
                return sum;
            },
            standardPhysics
        );
    }
}