using System;
using System.Collections.Generic;
using System.Linq;


// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var results = new List<ComparisonResult>();

        //попарное сравнение
        for (var i = 0; i < documents.Count; i++)
            for (var j = i + 1; j < documents.Count; j++)
                results.Add(CompareDocumentWith(documents[i], documents[j]));
        return results;
    }

    private ComparisonResult CompareDocumentWith(DocumentTokens first, DocumentTokens second)
    {
        var opt = new double[first.Count + 1, second.Count + 1];

        // Инициализация 
        //забавно, что бот ругается на стиль написания кода самих разработчиков)))
        for (var i = 0; i <= first.Count; i++)
            opt[i, 0] = i;
        for (var j = 0; j <= second.Count; j++)
            opt[0, j] = j;

        for (var i = 1; i <= first.Count; i++)
            for (var j = 1; j <= second.Count; j++)
                opt[i, j] = CalculateOptCellValue(opt, first, second, i, j);

        return new ComparisonResult(first, second, opt[first.Count, second.Count]);
    }

    private double CalculateOptCellValue(double[,] opt, DocumentTokens first, DocumentTokens second, int i, int j)
    {
        //стоимость для замены с алгоритмом Жаккара
        var cost = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);

        return Math.Min(
            Math.Min(
                opt[i - 1, j] + 1,     // Удаление
                opt[i, j - 1] + 1      // Вставка
            ),
            opt[i - 1, j - 1] + cost  // Замена 
        );
    }
}
