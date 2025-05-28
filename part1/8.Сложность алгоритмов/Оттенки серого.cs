using NUnit.Framework;

namespace Recognizer;

public static class GrayscaleTask
{
    public static double[,] ToGrayscale(Pixel[,] original)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var grayPixels = new double[width, height]; // "серый" массив с пикселями
        for (var i = 0; i < width; i++) //идем по пикселям двумя массивами
        {
            for (var j = 0; j < height; j++)
                // вычисляем яркость пикселя
                grayPixels[i, j] =
                    (original[i, j].R * 0.299 +
                    original[i, j].G * 0.587 +
                     original[i, j].B * 0.114)
                    / 255; // делим чтобы получить значение  от 0.0 до 1.0
        }
        return grayPixels;
    }
}
