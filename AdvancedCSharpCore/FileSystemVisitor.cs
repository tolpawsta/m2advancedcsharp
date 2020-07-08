using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdvancedCSharpCore
{
    public class FileSystemVisitor
    {
        private Predicate<FileSystemInfo> _filter;
        private Predicate<FileSystemInfo> _predicate;
        public event Action Start;
        public event Action Stop;
        public event EventHandler<FileSystemVisitorEventArgs> FileFinded;
        public event EventHandler<FileSystemVisitorEventArgs> DirectoryFinded;
        public event EventHandler<FileSystemVisitorEventArgs> FilterFileFinded;
        public event EventHandler<FileSystemVisitorEventArgs> FilterDirectoryFinded;
        public FileSystemVisitor(Predicate<FileSystemInfo> filter = null)
        {            
            _filter = filter;
        }
        public IEnumerable<FileSystemInfo> GetAll(string rootPath)
        {
            if (rootPath == null || !Directory.Exists(rootPath))
            {
                throw new Exception();
            }
            Start?.Invoke();
            try
            {
                foreach (var item in GetSystemElements(rootPath))
                {
                    if (_predicate?.Invoke(item) ?? false)
                    {
                        yield return item;
                        yield break;
                    }
                    if (item is DirectoryInfo)
                    {
                        DirectoryFinded?.Invoke(this, new FileSystemVisitorEventArgs() { Name = item.Name, FullPath = item.FullName });
                    }
                    else
                    {
                        FileFinded?.Invoke(this, new FileSystemVisitorEventArgs() { Name = item.Name, FullPath = item.FullName });
                    }
                    yield return item;
                }
            }
            finally
            {
                Stop?.Invoke();
            }           
        }
        public IEnumerable<FileSystemInfo> GetFiltered(string rootPath,bool isEnableFiltered = true)
        {
            if (rootPath == null)
                throw new ArgumentNullException("Root path must be not Null");
            if (!Directory.Exists(rootPath))
                throw new ArgumentNullException($"Root directory not exists in {rootPath}");
            Start?.Invoke();
            try
            {
                foreach (var item in GetSystemElements(rootPath))
                {
                    if (_predicate?.Invoke(item) ?? false)
                    {
                        yield return item;
                        yield break;
                    }
                    if (isEnableFiltered)
                    {
                        if (_filter?.Invoke(item) ?? false)
                        {
                            yield return ChackFileSystemInfo(item);
                        }
                    }
                    else
                    {
                        if (!_filter?.Invoke(item) ?? false)
                        {
                            yield return item;
                        }
                    }
                }
            }
            finally
            {
                Stop?.Invoke();
            }
        }
        private FileSystemInfo ChackFileSystemInfo(FileSystemInfo file)
        {
            if (file is DirectoryInfo)
            {
                FilterDirectoryFinded?.Invoke(this, new FileSystemVisitorEventArgs() { Name = file.Name, FullPath = file.FullName });
            }
            else
            {
                FilterFileFinded?.Invoke(this, new FileSystemVisitorEventArgs() { Name = file.Name, FullPath = file.FullName });
            }
            return file;
        }
        private IEnumerable<FileSystemInfo> GetSystemElements(string rootPath)
        {
            foreach (var dir in GetDirectories(rootPath))
            {
                yield return dir;
            }
            foreach (var file in GetFiles(rootPath))
            {
                yield return file;
            }
        }
        private IEnumerable<FileSystemInfo> GetDirectories(string currenPathDirectory)
        {
            var directories = Directory.EnumerateDirectories(currenPathDirectory);
            foreach (var pathDir in directories)
            {
                DirectoryInfo directory = new DirectoryInfo(pathDir);
                yield return directory;
                foreach (var innerDir in GetSystemElements(pathDir))
                {
                    yield return innerDir;
                }
            }
        }
        private IEnumerable<FileSystemInfo> GetFiles(string currenPathDirectory)
        {
            var files = Directory.EnumerateFiles(currenPathDirectory);
            foreach (var filePath in files)
            {
                yield return new FileInfo(filePath);
            }
        }
        public void StopVisitor(Predicate<FileSystemInfo> predicate)
        {
            _predicate = predicate;
        }
    }
}
