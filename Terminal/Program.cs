using System;
using System.IO;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FsTree;
using Additional;
using Microsoft.VisualBasic;


namespace Terminal
{
    public static class Terminal
    {
        
        public static DirectoryInfo currentDirectory = new DirectoryInfo("../..");
        
        public static Tree tree = new Tree(currentDirectory.FullName)
        {
            maxDepth = 10
        };
        
        public static List<string> commands = new List<string>()
        {
            "ls",
            "cls",
            "exit",
            "cat",
            "help",
            "cd",
            "dir",
            "writels"
        };

        public static void Main(string[] args)
        {
            ReadCommands();
        }

        public static void ReadCommands()
        {

            string? inputCommand = "";
            string[] commandParts;
            string command;
            string filename;
            string directoryPath;
            bool flag = true;
            
            while (flag)
            {
                inputCommand = Console.ReadLine();
                commandParts = inputCommand.Split();
                command = commandParts[0];

                if (string.IsNullOrEmpty(command))
                {
                    continue;
                }
                if (!commands.Contains(command))
                {
                    Additional.Additional.ShowErrorMessage(code:0);
                    continue;
                }

                if (command == "exit")
                {
                    flag = false;
                }
                else if (command == "help")
                {
                    Additional.Additional.Help();
                }
                else if (command == "dir")
                {
                    if (commandParts.Length == 1)
                    {
                        Console.WriteLine(currentDirectory.FullName);
                    }
                    else
                    {
                        Additional.Additional.ShowErrorMessage(code:7);
                    }
                }
                else if (command == "ls")
                {
                    if (commandParts.Length == 1)
                    {
                        ShowTree(currentDirectory);
                    }
                    else if (commandParts.Length > 1)
                    {
                        int depth;
                        if (ValidateShow(commandParts, out depth))
                        {
                            ShowTree(currentDirectory, true, depth);
                        }
                        else
                        {
                            Additional.Additional.ShowErrorMessage(code:1);
                        }
                    }
                }
                else if (command == "cls")
                {
                    Additional.Additional.CleanConsole();
                }
                else if (command == "cd")
                {
                    if (commandParts.Length == 1)
                    {
                        Additional.Additional.ShowErrorMessage(code:4);
                    }
                    else if (commandParts.Length > 1)
                    {
                        directoryPath = inputCommand[3..];
                        if (directoryPath.Trim() == "up")
                        {
                            if (currentDirectory.Parent?.FullName != null)
                            {
                                currentDirectory = new DirectoryInfo(currentDirectory.Parent?.FullName ?? string.Empty);
                                Console.WriteLine($"-> {currentDirectory.FullName}");
                            }
                            else
                            {
                                Additional.Additional.ShowErrorMessage(code:6);
                            }
                        }
                        else
                        {
                            if (!ValidateAndSetCd(directoryPath))
                            {
                                Additional.Additional.ShowErrorMessage(code:5);
                            }
                            else
                            {
                                Console.WriteLine($"-> {currentDirectory.FullName}");
                            }
                        }
                    }
                }
                else if (command == "cat")
                {
                    if (commandParts.Length == 1)
                    {
                        Additional.Additional.ShowErrorMessage(code:2);
                    }
                    else if (commandParts.Length > 1)
                    {
                        filename = inputCommand[4..];
                        FileInfo file;
                        if (ValidateCat(filename, out file))
                        {
                            ShowFileContent(file);
                        }
                        else
                        {
                            Additional.Additional.ShowErrorMessage(code:3);
                        }
                    }
                }
                else if (command == "writels")
                {
                    ShowTree(currentDirectory, false);
                    File.WriteAllLines("ls.txt", tree.treeLines);
                }
            }
        }

        public static bool ValidateShow(string[] commandParts, out int depth)
        {
            var check = int.TryParse(commandParts[1], out depth);
            if (check)
            {
                if (depth < 1)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool ValidateCat(string filename, out FileInfo file)
        {
            if (currentDirectory.EnumerateFiles().Select(x => x.Name).Contains(filename))
            {
                file = new FileInfo(Path.Combine(currentDirectory.FullName, filename));
                return true;
            }

            file = new FileInfo(Directory.GetCurrentDirectory());
            return false;
        }

        public static bool ValidateAndSetCd(string directoryName)
        {
            if (Directory.Exists(Path.Combine(currentDirectory.FullName, directoryName)))
            {
                currentDirectory = new DirectoryInfo((Path.Combine(currentDirectory.FullName, directoryName)));
                return true;
            }

            if (Directory.Exists(directoryName))
            {
                currentDirectory = new DirectoryInfo(directoryName);
                return true;
            }

            return false;
        }

        public static void ShowTree(DirectoryInfo currentDirectory, bool toConsole = true, int depth = 10)
        {
            tree = new Tree(currentDirectory.FullName)
            {
                maxDepth = depth
            };
            tree.Print(toConsole);
        }

        public static void ShowFileContent(FileInfo file)
        {
            var sr = file.OpenText();
            var lines = new List<string>();
            string line;
            Console.WriteLine($"{Environment.NewLine}Содержимое{file.Name}:");
            while (!string.IsNullOrEmpty(line = sr.ReadLine()!))
            {
                lines.Add(line);
            }

            PrintBorder(lines);
            PrintContent(lines);
            PrintBorder(lines);
        }

        public static void PrintBorder(List<string> lines, char chr = '—')
        {
            if (lines.Count == 0)
            {
                return;
            }
            var lineLength = lines.Max(x => x.Length);
            for (int i = 0; i < lineLength; i++)
            {
                Console.Write(chr);
            }

            Console.WriteLine();
        }

        public static void PrintContent(List<string> lines)
        {
            if (lines.Count == 0)
            {
                Console.WriteLine("<пустой файл>");
                return;
            }
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        
    }
}