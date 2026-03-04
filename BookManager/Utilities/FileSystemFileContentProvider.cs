using BookManager.Interfaces;

namespace BookManager.Utilities
{
    internal class FileSystemFileContentProvider : IFileContentProvider
    {
        public Stream GetFileStream(string path)
        {
            return new FileStream(path, FileMode.Open);
        }

        public void SaveFileStream(Stream content, string path)
        {
            using var outputFileStream = new FileStream(path, FileMode.CreateNew);
            content.CopyTo(outputFileStream);
        }
    }
}
