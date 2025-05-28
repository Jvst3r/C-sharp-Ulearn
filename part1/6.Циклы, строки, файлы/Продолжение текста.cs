using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
            if (wordsCount == 0 || phraseBeginning == "") return phraseBeginning; //если количество слов для продолжения == 0
            var finalPhrase = new StringBuilder(phraseBeginning); //будем строить с помощью builder`a
            var lastWords = phraseBeginning; // переменная с предыдущим словом (в начале со всей фразой)
            var wordsInBuilder = 0;
            var result = AddWords(wordsInBuilder, lastWords, phraseBeginning, wordsCount, nextWords);
            return result;
        }

        public static string AddWords(int wordsInBuilder, string lastWords, string phraseBeginning, int wordsCount, Dictionary<string, string> nextWords)
        {
            var finalPhrase = new StringBuilder(phraseBeginning);
            string lastOneWord;
            for (var i = 0; i < wordsCount; i++) //пока слова для добавления не закончатся
            {
                //через одну строку не получалось, так что разбил для понятности
                wordsInBuilder = (finalPhrase.ToString()).Split(" ").Length;
                if (wordsInBuilder > 1)
                    lastWords = (finalPhrase.ToString()).Split(" ")[wordsInBuilder - 2] + // предпоследнее + последнее
                        " " + (finalPhrase.ToString()).Split(" ")[wordsInBuilder - 1];

                lastOneWord = (finalPhrase.ToString()).Split(" ")[wordsInBuilder - 1]; //последнее слово из buildera

                if (!nextWords.ContainsKey(lastWords) && !nextWords.ContainsKey(lastOneWord))
                    break;
                if (nextWords.ContainsKey(lastWords))
                {
                    lastWords = nextWords[lastWords]; //новое последнее слово
                    finalPhrase.Append(" " + lastWords); //добавляем новое слово
                }
                else if (nextWords.ContainsKey(lastOneWord))
                {
                    lastWords = nextWords[lastOneWord];
                    finalPhrase.Append(" " + lastWords);
                }
            }
            return finalPhrase.ToString();
        }
    }
}