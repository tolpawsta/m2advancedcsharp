using System;

namespace AdvancedCSharpCore
{
    public class FileSystemVisitorEventArgs:EventArgs
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
    }
}
