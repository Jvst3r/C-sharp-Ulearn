using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace linq_slideviews;

public static class ExtensionsTask
{
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items)
    {
        var itemsArray = items.OrderBy(item => item).ToArray(); //сортируем OrderBy

        if (itemsArray.Length == 0) throw new InvalidOperationException(); //если массив пуст, выкидываем исключение.

        return (itemsArray.Length % 2 != 0)
            ? itemsArray[itemsArray.Length / 2]
            : (itemsArray[itemsArray.Length / 2]
            + itemsArray[itemsArray.Length / 2 - 1]) / 2.0;
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        //господи неужели пройдёт все эти тупые ограничениея
        var previousItem = default(T);
        var flag = false;
        //var countOfIteration = 0;
        foreach (var item in items)
        {
            if (!flag)
            {
                previousItem = item;
                flag = true;
                continue;
            }
            yield return (previousItem, item);
            previousItem = item;
        }
    }
}