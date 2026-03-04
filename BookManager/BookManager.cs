using BookManager.Interfaces;
using BookManager.Models;
using BookManager.Utilities;
using System.Xml.Serialization;

namespace BookManager
{
    public class BookManager : IBookManager
    {
        private readonly IFileContentProvider _fileContentProvider;

        private List<Book> _books;

        public BookManager(IFileContentProvider fileContentProvider = null)
        {
            _fileContentProvider = fileContentProvider ?? new FileSystemFileContentProvider();

            _books = new List<Book>();
        }

        public IEnumerable<Book> GetBooks()
        {
            foreach (var book in _books)
            {
                yield return book;
            }
        }

        public void Load(string path, string rootElement = "Books")
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Book>), new XmlRootAttribute(rootElement));
            using var stream = _fileContentProvider.GetFileStream(path);
            var content = xmlSerializer.Deserialize(stream);
            if (content is IEnumerable<Book> list)
            {
                _books = list.ToList();
            }
            else
            {
                throw new InvalidOperationException("File exists, but is not of accepted format");
            }
        }

        public void Add(Book book)
        {
            _books.Add(book);
        }

        public void Sort(IComparer<Book> comparer = null)
        {
            comparer ??= new DefaultBookComparer();
            _books.Sort(comparer);
        }

        public IEnumerable<Book> SearchByTitle(string search)
        {
            return _books.Where(x => x.Title.Contains(search));
        }

        public void Save(string path, string rootElement = "Books")
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Book>), new XmlRootAttribute(rootElement));
            var contentStream = new MemoryStream();
            xmlSerializer.Serialize(contentStream, _books);
            _fileContentProvider.SaveFileStream(contentStream, path);
        }
    }
}
