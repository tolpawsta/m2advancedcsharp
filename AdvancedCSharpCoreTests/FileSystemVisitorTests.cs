using AdvancedCSharpCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AdvancedCSharpCoreTests
{
    public class FileSystemVisitorTests
    {
        private FileSystemVisitor visitor;
        private string _rootPath;
        public FileSystemVisitorTests()
        {
            _rootPath = "d:\\example";
            visitor = new FileSystemVisitor();
        }
        [Fact]
        public void GetAll_root_pass_is_null_shoud_throw_ArgumentNullExeption()
        {
            _rootPath = null;
            var ex = Record.Exception(() => visitor.GetAll(_rootPath));
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void GetAll_Root_Pass_not_exists_or_empty_shoud_throw_ArgumentNullExeption()
        {
            _rootPath = "";
            Assert.Throws<ArgumentNullException>(() => visitor.GetAll(_rootPath));
        }
        [Fact]
        public void GetAll_predicate_to_stop_valid_shoud_return_first_found_item_and_stop()
        {
            visitor.StopVisitor(item => item.Name.Contains("docx"));
            var list = new List<FileSystemInfo>(visitor.GetAll(_rootPath));
            int actual = 3;
            Assert.Equal(list.Count, actual);
        }
        [Fact]
        public void GetAll_predicate_to_stop_not_valid_or_null_shoud_return_all_elements()
        {
            visitor.StopVisitor(item => item.Name.Contains("docx"));
            var list = new List<FileSystemInfo>(visitor.GetAll(_rootPath));            
            Assert.NotEmpty(list);
        }
    }
}
