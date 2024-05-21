using Backlog.Core.Common;
using Microsoft.Extensions.FileProviders;
using System.Text;

namespace Backlog.Web.Helpers.Common
{
    public class FileHelper : PhysicalFileProvider, IFileHelper
    {
        #region Properties

        public string WebRootPath { get; }

        #endregion

        #region Ctor

        public FileHelper(IWebHostEnvironment webHostEnv)
            : base(File.Exists(webHostEnv.ContentRootPath) ? Path.GetDirectoryName(webHostEnv.ContentRootPath)! : webHostEnv.ContentRootPath)
        {

        }

        #endregion

        #region Methods

        public virtual string Combine(params string[] paths)
        {
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? [p] : p.Split('\\', '/')).ToArray());

            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
                //add leading slash to correctly form path in the UNIX system
                path = "/" + path;

            return path;
        }

        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public virtual string GetParentDirectory(string directoryPath)
        {
            return Directory.GetParent(directoryPath).FullName;
        }

        public virtual void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        public virtual void DeleteDirectory(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);

            //find more info about directory deletion
            //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                DeleteDirectoryRecursive(path);
            }
            catch (IOException)
            {
                DeleteDirectoryRecursive(path);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteDirectoryRecursive(path);
            }
        }

        public virtual bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public virtual long FileLength(string path)
        {
            if (!FileExists(path))
                return -1;

            return new FileInfo(path).Length;
        }

        public virtual void CreateFile(string path)
        {
            if (FileExists(path))
                return;

            var fileInfo = new FileInfo(path);
            CreateDirectory(fileInfo.DirectoryName);

            //we use 'using' to close the file after it's created
            using (File.Create(path))
            {
            }
        }

        public virtual void DeleteFile(string filePath)
        {
            if (!FileExists(filePath))
                return;

            File.Delete(filePath);
        }

        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public virtual string GetAbsolutePath(params string[] paths)
        {
            var allPaths = new List<string>();

            if (paths.Any() && !paths[0].Contains(WebRootPath, StringComparison.InvariantCulture))
                allPaths.Add(WebRootPath);

            allPaths.AddRange(paths);

            return Combine(allPaths.ToArray());
        }

        public virtual string GetVirtualPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            if (!DirectoryExists(path) && FileExists(path))
                path = new FileInfo(path).DirectoryName;

            path = path?.Replace(WebRootPath, string.Empty).Replace('\\', '/').Trim('/').TrimStart('~', '/');

            return $"~/{path ?? string.Empty}";
        }

        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('/');

            //if virtual path has slash on the end, it should be after transform the virtual path to physical path too
            var pathEnd = path.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

            return Combine(Root ?? string.Empty, path) + pathEnd;
        }

        public virtual async Task<byte[]> ReadAllBytesAsync(string filePath)
        {
            return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath) : Array.Empty<byte>();
        }

        public virtual async Task<string> ReadAllTextAsync(string path, Encoding encoding)
        {
            await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream, encoding);

            return await streamReader.ReadToEndAsync();
        }

        public virtual string ReadAllText(string path, Encoding encoding)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream, encoding);

            return streamReader.ReadToEnd();
        }

        public virtual async Task WriteAllBytesAsync(string filePath, byte[] bytes)
        {
            await File.WriteAllBytesAsync(filePath, bytes);
        }

        public virtual async Task WriteAllTextAsync(string path, string contents, Encoding encoding)
        {
            await File.WriteAllTextAsync(path, contents, encoding);
        }

        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        public new virtual IFileInfo GetFileInfo(string subpath)
        {
            subpath = subpath.Replace(Root, string.Empty);

            return base.GetFileInfo(subpath);
        }


        #endregion

        #region Helpers

        protected virtual void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
            const int maxIterationToWait = 10;
            var curIteration = 0;
            while (Directory.Exists(path))
            {
                curIteration += 1;
                if (curIteration > maxIterationToWait)
                    return;

                Thread.Sleep(100);
            }
        }

        protected static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
        }

        #endregion
    }
}
