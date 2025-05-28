using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete;

// Внимание!
// Есть одна распространенная ловушка при сравнении строк: строки можно сравнивать по-разному:
// с учетом регистра, без учета, зависеть от кодировки и т.п.
// В файле словаря все слова отсортированы методом StringComparison.InvariantCultureIgnoreCase.
// Во всех функциях сравнения строк в C# можно передать способ сравнения.

public class LeftBorderTask
{
    /// <returns>
    /// Возвращает индекс левой границы.
    /// То есть индекс максимальной фразы, которая не начинается с prefix и меньшая prefix.
    /// Если такой нет, то возвращает -1
    /// </returns>
    /// <remarks>
    /// Функция должна быть рекурсивной
    /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
    /// </remarks>
    public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        // комментарии отличные и конкурсы интересные.
        if (right - left == 1) //база рекурсии
            return left;
        var middle = left + (right - left) / 2; //вычисление серединного индекса
        if (StringsPartialOrderRelation(prefix, phrases[middle]))
            return GetLeftBorderIndex(phrases, prefix, left, middle);
        else return GetLeftBorderIndex(phrases, prefix, middle, right);
    }

    private static bool StringsPartialOrderRelation(string a, string b)
    {
        return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) < 0 //этот метод сравнивает строки чтобы a предшествовала b
                || b.StartsWith(a, StringComparison.OrdinalIgnoreCase); //начинается ли b с подстроки a
    }
}