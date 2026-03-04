using BookManager.Interfaces;
using BookManager.Models;
using Moq;
using System.Text;

namespace BookManager.Tests
{
    [TestFixture]
    public class BookManagerTests
    {
        private Mock<IFileContentProvider> _mockContentProvider;

        private BookManager _bookManager;

        [SetUp]
        public void Setup()
        {
            _mockContentProvider = new Mock<IFileContentProvider>();
            _bookManager = new BookManager(_mockContentProvider.Object);
        }

        [Test]
        public void GetBooks_FromGivenBooks_ReturnsOneItem()
        {
            _bookManager.Add(new Book()
            {
                Author = "a",
                Title = "b",
                PageCount = 1,
            });

            var books = new List<Book>(_bookManager.GetBooks());

            Assert.That(books, Has.Count.EqualTo(1));
            Assert.That(books[0].Author, Is.EqualTo("a"));
        }

        [Test]
        public void GetBooks_EmptyCollection_ReturnsEmptyCollection()
        {
            var books = new List<Book>(_bookManager.GetBooks());

            Assert.That(books, Has.Count.EqualTo(0));
        }

        [Test]
        public void Load_FromAValidFile_LoadsCorrectly()
        {
            var path = "path";
            var content = @"<Books><Book><Author>a</Author><Title>b</Title><PageCount>1</PageCount></Book></Books>";
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(content));
            _mockContentProvider
                .Setup(x => x.GetFileStream(path))
                .Returns(stream);

            _bookManager.Load(path);

            var books = new List<Book>(_bookManager.GetBooks());

            _mockContentProvider.Verify(x => x.GetFileStream(path), Times.Once());
            Assert.That(books, Has.Count.EqualTo(1));
            Assert.That(books[0].Author, Is.EqualTo("a"));
            Assert.That(books[0].Title, Is.EqualTo("b"));
            Assert.That(books[0].PageCount, Is.EqualTo(1));
        }

        [Test]
        public void Load_FromAMissingFile_Throws()
        {
            var path = "path";
            _mockContentProvider
                .Setup(x => x.GetFileStream(path))
                .Throws<FileNotFoundException>();

            TestDelegate act = () => _bookManager.Load(path);

            Assert.Throws<FileNotFoundException>(act);
        }

        [Test]
        public void Load_WithInvalidXml_Throws()
        {
            var path = "path";
            var content = @"<Books><Book><Author>a</Author><Title>b</Title><PageCount>1</PageCount></Book>";
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(content));
            _mockContentProvider
                .Setup(x => x.GetFileStream(path))
                .Returns(stream);

            TestDelegate act = () => _bookManager.Load(path);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Test]
        public void Load_WithWrongContent_Throws()
        {
            var path = "path";
            var content = @"<Items><Item><Text>a</Text></Item></Items>";
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(content));
            _mockContentProvider
                .Setup(x => x.GetFileStream(path))
                .Returns(stream);

            TestDelegate act = () => _bookManager.Load(path);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Test]
        public void Sort_WithDefaultComparer_ReturnsCorrectOrder()
        {
            _bookManager.Add(new Book()
            {
                Author = "d",
                Title = "b",
                PageCount = 1,
            });
            _bookManager.Add(new Book()
            {
                Author = "a",
                Title = "a",
                PageCount = 1,
            });
            _bookManager.Add(new Book()
            {
                Author = "d",
                Title = "a",
                PageCount = 1,
            });

            _bookManager.Sort();
            var books = new List<Book>(_bookManager.GetBooks());

            Assert.That(books[0].Author, Is.EqualTo("a"));
            Assert.That(books[1].Author, Is.EqualTo("d"));
            Assert.That(books[1].Title, Is.EqualTo("a"));
            Assert.That(books[2].Author, Is.EqualTo("d"));
            Assert.That(books[2].Title, Is.EqualTo("b"));
        }

        [Test]
        public void SearchByTitle_ByAGivenTitle_ReturnsCorrectItems()
        {
            _bookManager.Add(new Book()
            {
                Author = "d",
                Title = "cd",
                PageCount = 1,
            });
            _bookManager.Add(new Book()
            {
                Author = "a",
                Title = "aa",
                PageCount = 1,
            });
            _bookManager.Add(new Book()
            {
                Author = "d",
                Title = "ab",
                PageCount = 1,
            });

            var books = _bookManager.SearchByTitle("a");

            Assert.That(books.Count(), Is.EqualTo(2));
            Assert.That(books, Has.Exactly(1).Matches<Book>(x => x.Title == "aa"));
            Assert.That(books, Has.Exactly(1).Matches<Book>(x => x.Title == "ab"));
        }

        [Test]
        public void Save_ToAGivenPath_SavesXml()
        {
            string output = string.Empty;
            string path = "path";
            _bookManager.Add(new Book()
            {
                Author = "a",
                Title = "b",
                PageCount = 1,
            });
            _mockContentProvider
                .Setup(x => x.SaveFileStream(It.IsAny<Stream>(), path))
                .Callback((Stream stream, string path) => {
                    var buffer = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(buffer, 0, (int)stream.Length);
                    output = Encoding.UTF8.GetString(buffer);
                });

            _bookManager.Save(path);

            Assert.That(output, Contains.Substring(@"<Title>b</Title>"));
            Assert.That(output, Contains.Substring(@"<Author>a</Author>"));
            Assert.That(output, Contains.Substring(@"<PageCount>1</PageCount>"));
        }
    }
}