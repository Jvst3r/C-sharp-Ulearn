using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }


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

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static Token ReadSimpleField(string line, int startIndex)
        {
            var tokenBuilder = new StringBuilder();
            var index = startIndex;
            while ((index < line.Length) && !((line[index] == ' ')
                || (line[index] == '\'') || (line[index] == '"')))
            {
                tokenBuilder.Append(line[index]);
                index++;
            }
            return new Token(tokenBuilder.ToString(), startIndex, tokenBuilder.Length);
        }

        public static List<Token> ParseLine(string line)
        {
            var listOfTokens = new List<Token>();
            var index = 0;
            while (index < line.Length)
            {
                var field = new Token(null, 0, 0);
                if ((line[index] == '\'') || (line[index] == '\"'))
                    field = ReadQuotedField(line, index);
                else if (line[index] != ' ')
                    field = ReadSimpleField(line, index);
                if (field.Value == null)
                    index++;
                else
                {
                    index += field.Length;
                    listOfTokens.Add(field);
                }
            }
            return listOfTokens;
        }

        private static Token ReadField(string line, int startIndex)
        {
            return new Token(line, 0, line.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}