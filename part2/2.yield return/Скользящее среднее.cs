using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var queue = new Queue<DataPoint>();
        var sumY = 0.0;
        foreach (var item in data)
        {
            if (queue.Count == 0)
            {
                queue.Enqueue(item);
                sumY += item.OriginalY;
                yield return item.WithAvgSmoothedY(item.OriginalY);
            }
            else
            {
                if (queue.Count == windowWidth)
                    sumY -= queue.Dequeue().OriginalY;
                sumY += item.OriginalY;
                queue.Enqueue(item);
                yield return item.WithAvgSmoothedY(sumY / queue.Count);
                if (queue.Count == windowWidth)
                    sumY -= queue.Dequeue().OriginalY;
            }
        }
    }
}