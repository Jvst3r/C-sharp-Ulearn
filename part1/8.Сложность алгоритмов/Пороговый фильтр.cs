using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Recognizer;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        int tValue = (int)(height * width * whitePixelsFraction);
        var result = new double[width, height];
        var list = new List<double>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                list.Add(original[i, j]);
        }
        list.Sort();
        list.Reverse();
        list.RemoveRange(tValue, list.Count - tValue);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (list.Contains(original[i, j]))
                    result[i, j] = 1.0;
                else result[i, j] = 0.0;
            }
        }
        return result;
    }
}
