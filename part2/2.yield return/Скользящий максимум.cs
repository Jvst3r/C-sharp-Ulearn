using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var canBeMaxItems = new LinkedList<double>();
        var queue = new Queue<DataPoint>();
        foreach (var item in data)
        {
            while (canBeMaxItems.Count > 0 &&
                canBeMaxItems.Last.Value < item.OriginalY)
                canBeMaxItems.RemoveLast();

            canBeMaxItems.AddLast(item.OriginalY);
            queue.Enqueue(item);
            if (queue.Count > windowWidth && canBeMaxItems.First.Value == queue.Dequeue().OriginalY)
                canBeMaxItems.RemoveFirst();
            yield return item.WithMaxY(canBeMaxItems.First.Value);
        }
    }
}