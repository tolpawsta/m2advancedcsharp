using AdvancedCSharpCore;
using Microsoft.VisualBasic;
using System;
using Xunit;

namespace AdvancedCSharpCoreTests
{
    public class FileSystemVisitorTests
    {
        private readonly FileSystemVisitor visitor;
        private readonly string _rootPath;
        public FileSystemVisitorTests()
        {
            _rootPath = null;
            visitor = new FileSystemVisitor(_rootPath);
        }
        [Fact]
        public void GetAll_Root_Pass_is_Null()
        {
            Assert.Throws<ArgumentNullException>(()=>visitor.GetAll());
        }
    }
}
