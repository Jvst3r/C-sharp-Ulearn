using System.Text;

namespace TextAnalysis;

static class SentencesParserTask
{
    /*
    БЕЗ ЭТОГО НЕ РАБОТАЕТ!!!!

    Отче наш,иже еси в моем PC,
    Да святится имя и расширение Твое,
    Да придет прерывание Твое и да будет воля Твоя
    BASIC наш насущный дай нам,
    И прости нам дизассемблеры и антивирусы наши,
    как Копирайты прощаем мы.И не введи нас в Exception Error,
    но избавь нас от зависания,
    ибо Твое есть адресное пространство,
    порты и регистры Во имя CTRLа,
    ALTа и святого DELа,
    всемогущего RESETа во веки веков.

    */

    public static List<List<string>> ParseSentences(string text)
    {
        var sentencesList = new List<List<string>>();
        var separators = new[] { '.', '!', '?', ';', ':', '(', ')' }; // массив разделителей предложений для метода .Split
        var sentences = text.Split(separators);

        foreach (var sentence in sentences)
        {
            var words = SplitSentenceForWords(sentence);
            if (words.Count > 0) sentencesList.Add(words);
        }

        return sentencesList;
    }

    public static List<string> SplitSentenceForWords(string sentence) //метод разделяет предложение на отдельные слова
    {
        var wordBuilder = new StringBuilder();
        var words = new List<string>();
        sentence = sentence + ".";

        foreach (var letter in sentence)
        {
            if (char.IsLetter(letter) || letter == '\'') //если буква или апостраф
                wordBuilder.Append(char.ToLower(letter)); //добавляем в builder
            else if (wordBuilder.Length > 0)
            {
                words.Add(wordBuilder.ToString()); // иначе добавляем слово в список
                wordBuilder.Clear(); //чистим builder
            }
        }

        return words;
    }
}