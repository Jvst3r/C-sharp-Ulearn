[TestCase("text", new[] { "text" })]
[TestCase("hello world", new[] { "hello", "world" })]
[TestCase("a b c d", new[] { "a", "b", "c", "d" })]
[TestCase(" ", new string[] { })]
[TestCase("'", new[] { "" })]
[TestCase("'hello'\"world\"", new[] { "hello", "world" })]
[TestCase("\"hello 'world'\"", new[] { "hello 'world'" })]
[TestCase("'hello \"world\"'", new[] { "hello \"world\"" })]
[TestCase("\"many slashes\\\\\"", new[] { "many slashes\\" })]
[TestCase("'one quote", new[] { "one quote" })]
[TestCase("\"hello\\\" world\"", new[] { "hello\" world" })]
[TestCase("  space  ", new[] { "space" })]
[TestCase("word''", new[] { "word", "" })]
[TestCase("''word", new[] { "", "word" })]
[TestCase("' ", new[] { " " })]
[TestCase("'hello\\' world'", new[] { "hello' world" })]
// Вставляйте сюда свои тесты
public static void RunTests(string input, string[] expectedOutput)
{
    // Тело метода изменять не нужно
    Test(input, expectedOutput);
}