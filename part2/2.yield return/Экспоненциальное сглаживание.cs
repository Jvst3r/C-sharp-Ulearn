using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        double lastCurrent = double.NaN; //St из формулы
                                         // double newCurrent = double.NaN;
        foreach (var element in data)
        {
            if (double.IsNaN(lastCurrent))
            {
                lastCurrent = element.OriginalY;
                yield return element.WithExpSmoothedY(element.OriginalY);
            }
            // простейшая форма экспоненциального сглаживания для последующих элементов коллекции
            else
            {
                //newCurrent = alpha * element.OriginalY + (1 - alpha) * lastCurrent;
                yield return element.WithExpSmoothedY(alpha * element.OriginalY + (1 - alpha) * lastCurrent);
                lastCurrent = alpha * element.OriginalY + (1 - alpha) * lastCurrent;
            }
        }
    }
}
