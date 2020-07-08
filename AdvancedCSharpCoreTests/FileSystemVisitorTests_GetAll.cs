using AdvancedCSharpCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdvancedCSharpCoreTests
{
    public class FileSystemVisitorTests_GetAll
    {
        private FileSystemVisitor visitor;
        private string _rootPath;
        public FileSystemVisitorTests_GetAll()
        {
            _rootPath = "d:\\example";
            visitor = new FileSystemVisitor();
        }
        [Fact]
        public void GetAll_root_pass_is_null_shoud_throw_ArgumentNullExeption()
        {
            //Array
            _rootPath = null;

            //Act
            var ex = Record.Exception(() => visitor.GetAll(_rootPath).Count());

            //Assert
            Assert.IsType<ArgumentException>(ex);
        }
        [Fact]
        public void GetAll_Root_Pass_not_exists_or_empty_shoud_throw_ArgumentNullExeption()
        {
            //Array
            _rootPath = "";

            //Act
            var ex =Record.Exception(()=> visitor.GetAll(_rootPath).Count());

            //Assert
            Assert.IsType<ArgumentException>(ex);
        }
        [Fact]
        public void GetAll_predicate_to_stop_valid_shoud_return_first_found_item_and_stop()
        {
            //Array
            visitor.StopVisitor(item => item.Name.Contains("docx"));
            int actual = 3;
            
            //Act
            var list = new List<FileSystemInfo>(visitor.GetAll(_rootPath));

            //Assert
            Assert.Equal(list.Count, actual);
        }
        [Fact]
        public void GetAll_predicate_to_stop_not_valid_or_null_shoud_return_all_elements()
        {
            //Array
            visitor.StopVisitor(item => item.Name.Contains("docx"));

            //Act
            var list = new List<FileSystemInfo>(visitor.GetAll(_rootPath));

            //Assert
            Assert.NotEmpty(list);
        }
    }
}
