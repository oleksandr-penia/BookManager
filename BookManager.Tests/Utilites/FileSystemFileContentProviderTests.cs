using BookManager.Utilities;
using System.Text;

namespace BookManager.Tests.Utilites
{
    [TestFixture]
    public class FileSystemFileContentProviderTests
    {
        private const string XmlData = @"<Books><Book><Author>a</Author><Title>b</Title><PageCount>1</PageCount></Book></Books>";
        private FileSystemFileContentProvider _provider;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            using var writer = new StreamWriter("a.xml", false);
            writer.Write(XmlData);
        }

        [SetUp]
        public void Setup()
        {
            _provider = new FileSystemFileContentProvider();
        }

        [Test]
        public void GetFileStream_FromCreatedFile_GetsAStream()
        {
            using var stream = _provider.GetFileStream("a.xml");

            var buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, (int)stream.Length);
            var content = Encoding.UTF8.GetString(buffer);

            Assert.That(content, Is.EqualTo(XmlData));
        }

        [Test]
        public void SaveFileStream_ToAGivenPath_SavesXml()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(XmlData));

            _provider.SaveFileStream(stream, "b.xml");

            using var reader = new StreamReader("b.xml");
            var content = reader.ReadToEnd();
            Assert.That(content, Is.EqualTo(XmlData));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (File.Exists("a.xml"))
            {
                File.Delete("a.xml");
            }

            if (File.Exists("b.xml"))
            {
                File.Delete("b.xml");
            }
        }
    }
}
