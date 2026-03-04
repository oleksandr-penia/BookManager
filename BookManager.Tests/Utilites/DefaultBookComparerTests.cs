using BookManager.Models;
using BookManager.Utilities;

namespace BookManager.Tests.Utilites
{
    [TestFixture]
    public class DefaultBookComparerTests
    {
        [TestCaseSource(nameof(_testCases))]
        public void Compare_WithGivenData_ReturnsExpectedResult(string author1, string title1, string author2, string title2, int expected)
        {
            var book1 = new Book()
            {
                Author = author1,
                Title = title1,
                PageCount = 1,
            };
            var book2 = new Book()
            {
                Author = author2,
                Title = title2,
                PageCount = 1,
            };
            var comparer = new DefaultBookComparer();

            var cmp = comparer.Compare(book1, book2);

            Assert.That(cmp, Is.EqualTo(expected));
        }

        private static object[] _testCases =
        {
            new object[] { "a", "aa", "b", "bb", -1 },
            new object[] { "A", "aa", "b", "bb", -1 },
            new object[] { "A", "aa", "B", "bb", -1 },
            new object[] { "A", "aa", "B", "bb", -1 },

            new object[] { "b", "aa", "a", "bb", 1 },
            new object[] { "B", "aa", "a", "bb", 1 },
            new object[] { "B", "aa", "A", "bb", 1 },
            new object[] { "B", "aa", "A", "bb", 1 },

            new object[] { "b", "aa", "b", "aa", 0 },
            new object[] { "B", "aa", "b", "AA", 0 },
            new object[] { "B", "aa", "B", "aA", 0 },

            new object[] { "a", "aa", "a", "bb", -1 },
            new object[] { "a", "aa", "a", "BB", -1 },
            new object[] { "a", "AA", "a", "bb", -1 },
            new object[] { "a", "AA", "a", "BB", -1 },

            new object[] { "a", "bb", "a", "aa", 1 },
            new object[] { "a", "bb", "a", "AA", 1 },
            new object[] { "a", "BB", "a", "aa", 1 },
            new object[] { "a", "BB", "a", "AA", 1 },
        };
    }
}
