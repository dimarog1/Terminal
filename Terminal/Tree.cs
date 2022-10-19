using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;
using Additional;

namespace FsTree
{
    public class Tree
    {
        public Tree(string startDir)
        {
            path = startDir;
        }

        public string path { get; }

        public int maxDepth { get; set; } = int.MaxValue;
        public List<string> treeLines { get; set; } = new List<string>();

        public void Print()
        {
            treeLines = new List<string>();
            Console.WriteLine(path);
            treeLines.AddRange(new[] { path, "" });
            PrintTree(path);
        }
        private bool IsDirectory(FileSystemInfo fsi)
        {
            return fsi.Attributes.HasFlag(FileAttributes.Directory);
        }

        private static long GetDirectorySize(DirectoryInfo dir) => dir.EnumerateFiles().Sum(x => x.Length);
        private static long GetFileSize(FileInfo file) => file.Length;

        private void PrintTree(string path, bool toConsole = true, string prefix = "", int depth = 0)
        {
            if (depth >= maxDepth)
            {
                return;
            }

            var di = new DirectoryInfo(path);
            var fsinfos = di.EnumerateFileSystemInfos().ToList();

            foreach (var fsi in fsinfos.Take(fsinfos.Count - 1))
            {
                try
                {
                    if (IsDirectory(fsi))
                    {
                        var di_ = (DirectoryInfo)fsi;
                        var dirSize = Additional.Additional.GetConvertedSize(GetDirectorySize(di_));
                        var line = $"{prefix}├── {fsi.Name} [{dirSize}; {di_.EnumerateFiles().Count()} Ф]";
                        if (toConsole)
                        {
                            Additional.Additional.PrintColorMessage(line, ConsoleColor.Magenta);
                        }
                        treeLines[^1] += line;
                    }
                    else
                    {
                        var fileSize = Additional.Additional.GetConvertedSize(GetFileSize((FileInfo)fsi));
                        var line = $"{prefix}├── {fsi.Name} ({fileSize})";
                        if (toConsole)
                        {
                            Additional.Additional.PrintColorMessage(line, ConsoleColor.DarkCyan);
                        }
                        treeLines[^1] += line;
                    }

                    treeLines.Add("");
                    if (IsDirectory(fsi))
                    {
                        PrintTree(fsi.FullName, toConsole, prefix + "│   ", depth + 1);
                    }
                }
                catch
                {
                    
                }
            }

            if (fsinfos.Count > 0)
            {
                var last = fsinfos[^1];
                try
                {
                    if (IsDirectory(last))
                    {
                        var di_ = (DirectoryInfo)last;
                        var lastDirSize = Additional.Additional.GetConvertedSize(GetDirectorySize(di_));
                        var line = $"{prefix}└── {last.Name} [{lastDirSize}; {di_.EnumerateFiles().Count()} Ф]";
                        var dirSize = Additional.Additional.GetConvertedSize(GetDirectorySize((DirectoryInfo)last));
                        if (toConsole)
                        {
                            Additional.Additional.PrintColorMessage(line, ConsoleColor.Magenta);
                        }
                        treeLines[^1] += line + Environment.NewLine;
                    }
                    else
                    {
                        var fileSize = Additional.Additional.GetConvertedSize(GetFileSize((FileInfo)last));
                        var line = $"{prefix}└── {last.Name} ({fileSize})";
                        if (toConsole)
                        {
                            Additional.Additional.PrintColorMessage(line, ConsoleColor.DarkCyan);
                        }
                        treeLines[^1] += line + Environment.NewLine;
                    }

                    if (IsDirectory(last))
                    {
                        PrintTree(last.FullName, toConsole, prefix + "    ", depth + 1);
                    }
                }
                catch
                {
                    
                }
            }
        }
    }
}