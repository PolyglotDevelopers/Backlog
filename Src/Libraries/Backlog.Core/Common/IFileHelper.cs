using System.Text;

namespace Backlog.Core.Common
{
    public interface IFileHelper
    {
        string Combine(params string[] paths);

        bool DirectoryExists(string path);

        string GetParentDirectory(string directoryPath);

        void CreateDirectory(string path);

        void DeleteDirectory(string path);

        bool FileExists(string filePath);

        long FileLength(string path);

        void CreateFile(string path);

        void DeleteFile(string filePath);

        string GetFileExtension(string filePath);

        string GetFileName(string path);

        string GetFileNameWithoutExtension(string filePath);

        string GetAbsolutePath(params string[] paths);

        string GetVirtualPath(string path);

        string MapPath(string path);

        Task<byte[]> ReadAllBytesAsync(string filePath);

        Task<string> ReadAllTextAsync(string path, Encoding encoding);

        string ReadAllText(string path, Encoding encoding);

        Task WriteAllTextAsync(string path, string contents, Encoding encoding);

        void WriteAllText(string path, string contents, Encoding encoding);
    }
}
