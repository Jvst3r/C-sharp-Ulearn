using System.Collections.Generic;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var result = new Dictionary<string, string>();
        var frequencies = BuildFrequencies(text);
        foreach (var ngramm in frequencies)
            result.Add(ngramm.Key, GetMostFrequent(ngramm.Value));

        return result;
    }

    private static Dictionary<string, Dictionary<string, int>> BuildFrequencies(List<List<string>> text)
    {
        var frequencies = new Dictionary<string, Dictionary<string, int>>();

        foreach (var sentence in text)
        {
            for (var i = 0; i < sentence.Count - 1; i++)
            {
                IncrementFrequency(frequencies, sentence[i], sentence[i + 1]);
                if (i < sentence.Count - 2)
                    IncrementFrequency(frequencies, sentence[i] + " " + sentence[i + 1], sentence[i + 2]);
            }
        }

        return frequencies;
    }

    private static string GetMostFrequent(Dictionary<string, int> wordFrequencies)
    {
        var result = "";
        var maxFrequency = -1;

        foreach (var word in wordFrequencies)
        {
            var wordValue = word.Value;
            var wordKey = word.Key;

            if (wordValue > maxFrequency || (wordValue == maxFrequency && string.CompareOrdinal(wordKey, result) < 0))
            {
                result = wordKey;
                maxFrequency = wordValue;
            }
        }

        return result;
    }

    private static void IncrementFrequency(Dictionary<string, Dictionary<string, int>> frequencies,
        string start, string nextWord)
    {
        if (!frequencies.ContainsKey(start))
            frequencies[start] = new Dictionary<string, int>();

        if (!frequencies[start].ContainsKey(nextWord))
            frequencies[start][nextWord] = 0;

        frequencies[start][nextWord]++;
    }
}