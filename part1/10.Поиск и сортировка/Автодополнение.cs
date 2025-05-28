using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete;

public class AutocompleteTests
{
    [Test]
    public void TopByPrefix_IsEmpty_WhenNoPhrases()
    {
        var actualResult = AutocompleteTask.GetTopByPrefix(new List<string> { }, "", 5);
        var expectedResult = new string[0];

        Assert.AreEqual(actualResult, expectedResult);
    }

    [Test]
    public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
    {
        var phrases = new List<string> { "aaa_every", "aba_string", "abc_startswith", "acc_empty" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "", phrases.Count);
        Assert.AreEqual(phrases.ToArray(), result);
    }
}

internal class AutocompleteTask
{
    /// <returns>
    /// Возвращает первую фразу словаря, начинающуюся с prefix.
    /// </returns>
    /// <remarks>
    /// Эта функция уже реализована, она заработает, 
    /// как только вы выполните задачу в файле LeftBorderTask
    /// слава Богу
    /// </remarks>
    public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        //если слово попадает в границы и начинается с префикса
        if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            return phrases[index]; //возвращаем это слово

        return null;//иначе ничего не возвращаем 
    }

    /// <returns>
    /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
    /// элементов словаря, начинающихся с prefix.
    /// </returns>
    /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
    public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
    {
        count = Math.Min(count, GetCountByPrefix(phrases, prefix)); // длина массива ниже(Кол-во слов)
        var result = new string[count]; //массив со словами
        var startPosition = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;//находим левую границу
        Array.Copy(phrases.ToArray(), startPosition, result, 0, count);//заносим в массив
        return result;
    }

    /// <returns>
    /// Возвращает количество фраз, начинающихся с заданного префикса
    /// </returns>
    public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
        //i слова слева - i словa справа = кол-во слов
        return RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count)
                - LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
    }
}

