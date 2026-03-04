using BookManager.Models;

namespace BookManager.Interfaces
{
    public interface IBookManager
    {
        IEnumerable<Book> GetBooks();
        void Load(string path, string rootElement);
        void Add(Book book);
        void Sort(IComparer<Book> comparer);
        IEnumerable<Book> SearchByTitle(string search);
        void Save(string path, string rootElement);
    }
}
