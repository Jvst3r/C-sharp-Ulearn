namespace Passwords;

public class CaseAlternatorTask
{
    //Тесты будут вызывать этот метод
    public static List<string> AlternateCharCases(string lowercaseWord)
    {
        var result = new List<string>();
        AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
        return result;
    }

    static void AlternateCharCases(char[] word, int startIndex, List<string> result)
    {
        if (startIndex == word.Length)
        {
            if (!result.Contains(new string(word))) result.Add(new string(word)); // если такого варианта еще нет до добавляем
            return;
        }
        var symbol = word[startIndex];
        if (char.IsLetter(symbol) && symbol != 223 && (symbol < 1425 || symbol > 1524)) // проверка на символы без регистра
        {
            word[startIndex] = char.ToLower(word[startIndex]);
            AlternateCharCases(word, startIndex + 1, result);
            word[startIndex] = char.ToUpper(word[startIndex]);
            AlternateCharCases(word, startIndex + 1, result);
        }
        else
        {
            AlternateCharCases(word, startIndex + 1, result);
        }
        if (!result.Contains(new string(word))) result.Add(new string(word)); // если такого варианта еще нет до добавляем
    }
}