using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Interfaces
{
    public interface IFileContentProvider
    {
        public Stream GetFileStream(string path);
        public void SaveFileStream(Stream content, string path);
    }
}
