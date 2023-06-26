using FileScanner.Models;
using MimeTypes.Core;

namespace FileScanner
{
    public class FileScanner
    {
        private string _path;

        public FileScanner(string path)
        {
            _path = path;
        }

        public Directories GetDir(ref MimeTypeGroup mime)
        {
            return ScanDirectory(_path, mime);
        }

        public Directories ScanDirectory(string path, MimeTypeGroup mime)
        {
            var directoryInfo = new DirectoryInfo(path);
            var dirsAndFiles = new Directories();

            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                var mimeTypeGroup = new MimeTypeGroup();
                mimeTypeGroup.Count = 1;
                mimeTypeGroup.Value = file.Length;
                if (files.Any() && file != null)
                {
                    mime.TotalFilesCount += 1;
                    string mimeType = MimeTypeMap.GetMimeType(file.Extension);
                    if (!mime.MimeTypes.TryAdd(mimeType, mimeTypeGroup))
                    {
                        mime.MimeTypes[mimeType].Count += 1;
                        mime.MimeTypes[mimeType].Value += file.Length;
                    }
                }
            }
            var stack = new Stack<string>();
            var directories = directoryInfo.GetDirectories();

            dirsAndFiles.DirectoryName = directoryInfo.Name;
            dirsAndFiles.Files = files;


            foreach (var dir in directories)
            {
                dirsAndFiles.DirectoryInf.Add(ScanDirectory(dir.FullName, mime));
            }

            return dirsAndFiles;
        }
    }
}
