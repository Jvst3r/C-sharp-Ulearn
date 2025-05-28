namespace Recognizer;

using NUnit.Framework;
using System;
using System.Collections.Generic;

internal static class MedianFilterTask
{
    /* 
	 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
	 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
	 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
	 * https://en.wikipedia.org/wiki/Median_filter
	 * 
	 * Используйте окно размером 3х3 для не граничных пикселей,
	 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
	 */
    public static double[,] MedianFilter(double[,] original)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        if (height == 1 && width == 1) return original;
        var originalAfterFilter = new double[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                originalAfterFilter[i, j] = GetMedianValue(original, i, j);
            }
        }
        return originalAfterFilter;
    }

    public static double GetMedianValue(double[,] original, int i, int j)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var widthLowerBound = Math.Max(0, i - 1);
        var heightLowerBound = Math.Max(0, j - 1);
        var widthUpperBound = Math.Min(width - 1, i + 1);
        var heightUpperBound = Math.Min(height - 1, j + 1);
        var result = new List<double>();
        for (int x = widthLowerBound; x <= widthUpperBound; x++)
        {
            for (int y = heightLowerBound; y <= heightUpperBound; y++)
                result.Add(original[x, y]);
        }
        result.Sort();
        if ((result.Count - 1) % 2 == 0) return result[result.Count / 2];
        else return (result[(result.Count / 2) - 1] + result[result.Count / 2]) / 2;
    }
}
