using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly char[] separators = { ' ', '.', ',', '!', '?', ':', '-', '\n', '\r' };

    private Dictionary<string, Dictionary<int, List<int>>> dictionary
            = new Dictionary<string, Dictionary<int, List<int>>>(); //по строке
    public void Add(int id, string documentText)
    {
        var words = documentText.Split(separators);
        var position = 0;

        foreach (var word in words)
        {
            if (!dictionary.ContainsKey(word))
                dictionary.Add(word, new Dictionary<int, List<int>>());

            if (!dictionary[word].ContainsKey(id))
                dictionary[word].Add(id, new List<int>());

            dictionary[word][id].Add(position);
            position += word.Length + 1;
        }
    }

    public List<int> GetIds(string word)
    {
        var idList = new List<int>();
        if (dictionary.ContainsKey(word))
        {
            foreach (var id in dictionary[word].Keys)
                idList.Add(id);
        }
        return idList;
    }

    public List<int> GetPositions(int id, string word)
    {
        var positionList = new List<int>();
        if (dictionary.ContainsKey(word))
            if (dictionary[word].ContainsKey(id))
                positionList = dictionary[word][id];

        return positionList;
    }

    public void Remove(int id)
    {
        foreach (var doc in dictionary.Keys.ToList())
            if (dictionary[doc].ContainsKey(id))
                dictionary[doc].Remove(id);
    }
}