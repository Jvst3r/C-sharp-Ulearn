using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count + 1];

        for (int i = 1; i <= first.Count; i++)
            for (int j = 1; j <= second.Count; j++)
            {
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1] + 1; //если совпадают, то идем по диагонали + 1
                else
                    opt[i, j] = Math.Max(opt[i - 1, j], opt[i, j - 1]); //иначе берем максимальную подпоследовательность из "окружения"
            }

        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var result = new List<string>();
        var i = first.Count;
        var j = second.Count;
        while (i > 0 && j > 0)
        {
            if (first[i - 1] == second[j - 1]) //еслм токены совпадают
            {
                result.Add(first[i - 1]); //добавляем токен в результат
                i--; j--; // Двигаемся по диагонали
            }
            else if (opt[i - 1, j] > opt[i, j - 1])
                i--; // Двигаемся вверх
            else
                j--; // Двигаемся влево
        }

        result.Reverse();

        return result;
    }
}
