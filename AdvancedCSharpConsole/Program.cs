using AdvancedCSharpCore;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedCSharpConsole
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            string pathDir = config["rootPath"];
            FileSystemVisitor visitor = new FileSystemVisitor(item => item.Name.Contains("docx"));
            Subscribe(visitor);
            //set case when visitor was stoped
            visitor.StopVisitor(item => item.Name.Contains("docx"));
            ShowElements("All system elements (files and folders)", visitor.GetAll(pathDir));
            ShowElements("(Un)Filetered (files and forders)", visitor.GetFiltered(pathDir));
        }

        private static void ShowElements(string message, IEnumerable<FileSystemInfo> elements)
        {
            ColoredMessageOutput(message, ConsoleColor.Yellow, ConsoleColor.White);
            foreach (var element in elements)
            {
                logger.Info(element.FullName);
            }
        }

        private static void Subscribe(FileSystemVisitor visitor)
        {
            visitor.Start += Visitor_Start;
            visitor.Stop += Visitor_Stop;
            visitor.FileFinded += Visitor_FileFinded;
            visitor.DirectoryFinded += Visitor_DirectoryFinded;
            visitor.FilterDirectoryFinded += Visitor_FilterDirectoryFinded;
            visitor.FilterFileFinded += Visitor_FilterFileFinded;
        }

        private static void Visitor_FilterFileFinded(object sender, FileSystemVisitorEventArgs e)
        {
            logger.Info($"{"Filter file found: ",-24}");
        }

        private static void Visitor_FilterDirectoryFinded(object sender, FileSystemVisitorEventArgs e)
        {
            logger.Info($"Filter directory found: ");
        }

        private static void Visitor_DirectoryFinded(object sender, FileSystemVisitorEventArgs e)
        {
            logger.Info($"Directory found: ");
        }

        private static void Visitor_FileFinded(object sender, FileSystemVisitorEventArgs e)
        {
            logger.Info($"{"File found: ",-17}");
        }

        private static void Visitor_Stop()
        {
            ColoredMessageOutput("Visitor stoped.", ConsoleColor.Red, ConsoleColor.White);
        }

        private static void Visitor_Start()
        {
            ColoredMessageOutput("Visitor start.", ConsoleColor.Green, ConsoleColor.White);
        }
        private static void ColoredMessageOutput(string message, ConsoleColor colorMessage, ConsoleColor colorDisable)
        {
            Console.ForegroundColor = colorMessage;
            logger.Info(message);
            Console.ForegroundColor = colorDisable;
        }
    }
}
