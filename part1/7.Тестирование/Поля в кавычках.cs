using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'", 0, "", 1)]
        [TestCase("\"Hello world\"", 0, "Hello world", 13)]
        [TestCase("\"unquote last index", 0, "unquote last index", 19)]
        [TestCase("\"i\\\\hope you\\\\believe\\\\\" me", 0, "i\\hope you\\believe\\", 24)]

        public void Test(string line, int startIndex,
            string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var index = startIndex + 1;
            var tokenBuilder = new StringBuilder();
            while ((index < line.Length)
                && !(line[index] == line[startIndex]))
            {
                if (line[index] == '\\')
                    index++;
                tokenBuilder.Append(line[index]);
                index++;
            }
            if (index < line.Length)
                index++;
            return new Token(tokenBuilder.ToString(),
                startIndex, index - startIndex);
        }
    }
}