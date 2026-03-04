using BookManager.Models;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BookManager.Tests")]

namespace BookManager.Utilities
{
    internal class DefaultBookComparer : IComparer<Book>
    {
        public int Compare(Book? x, Book? y)
        {
            int authorCmp = string.Compare(x.Author, y.Author, StringComparison.InvariantCultureIgnoreCase);
            return authorCmp == 0 ?
                string.Compare(x.Title, y.Title, StringComparison.InvariantCultureIgnoreCase) :
                authorCmp;
        }
    }
}
