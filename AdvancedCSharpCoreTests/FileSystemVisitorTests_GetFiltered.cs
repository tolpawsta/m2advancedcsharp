using AdvancedCSharpCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdvancedCSharpCoreTests
{
    public class FileSystemVisitorTests_GetFiltered
    {
        private FileSystemVisitor visitor;
        private string _rootPath;
        public FileSystemVisitorTests_GetFiltered()
        {
            _rootPath = "d:\\example";
            visitor = new FileSystemVisitor();
        }
        [Fact]
        public void GetFiltered_root_pass_is_null_shoud_throw_ArgumentNullExeption()
        {
            //Array
            _rootPath = null;

            //Act
            var ex = Record.Exception(() => visitor.GetFiltered(_rootPath).Count());

            //Assert
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void GetFiltered_Root_Pass_not_exists_or_empty_shoud_throw_ArgumentNullExeption()
        {
            //Array
            _rootPath = "";

            //Act
            var ex = Record.Exception(() => visitor.GetFiltered(_rootPath).Count());

            //Assert
            Assert.IsType<ArgumentException>(ex);
        }
        [Fact]
        public void GetFiltered_predicate_to_stop_valid_and_not_filter_shoud_return_first_found_item_and_stop()
        {
            //Array
            visitor.StopVisitor(item => item.Name.Contains("docx"));
            int actual = 0;

            //Act
            var list = new List<FileSystemInfo>(visitor.GetFiltered(_rootPath));

            //Assert
            Assert.Equal(list.Count, actual);
        }
        [Fact]
        public void GetFiltered_predicate_to_stop_and_filter_same_shoud_return_all_elements()
        {
            //Array
            var _visitor = new FileSystemVisitor(item => item.Name.Contains("docx"));
            _visitor.StopVisitor(item => item.Name.Contains("docx"));
            int actual = 1;

            //Act
            var list = new List<FileSystemInfo>(_visitor.GetFiltered(_rootPath));

            //Assert
            Assert.Equal(list.Count,actual);
        }
    }
}
