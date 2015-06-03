using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFlow.Services.Interfaces
{
    public interface IFileService
    {
        bool FileExists(string filePath);      

        void MoveFile(string oldPath, string newPath);

        void RenameFile(string oldPath, string newPath);

        void DeleteFile(string path);

        IEnumerable<string> GetFilesFromDirectory(string directory);
    }
}
