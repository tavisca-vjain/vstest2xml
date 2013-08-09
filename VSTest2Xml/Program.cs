using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace VSTest2JUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[]{@"/sd:D:\Code\GIT\vstest2junit\VSTest2JUnit\Xmls"};
            if (!ValidArgs(args.ToList()))
            {
                ShowUsage();
                Environment.Exit(1);
                
            }
            var argList = args.ToList();


            var sf = GetSourceFilePath(argList);
            var sd = GetSourceDirectoryPath(argList);
            var sourceFiles = new List<string>();
            if (string.IsNullOrEmpty(sf) || !File.Exists(sf))
            {
                if (string.IsNullOrEmpty(sd))
                {
                    Console.WriteLine("Could not find source file.");
                    Environment.Exit(1);
                    
                }
                if (!Directory.Exists(sd))
                {
                    Console.WriteLine("Could not find directory.");
                    Environment.Exit(1);
                    
                }
                var files = Directory.GetFiles(sd);
                foreach (var file in files)
                {
                    if (file.EndsWith(".trx", false, CultureInfo.CurrentCulture))
                        sourceFiles.Add(file);
                }

                if (sourceFiles.Count == 0)
                {
                    Console.WriteLine("Could not find any .trx files in directory.");
                    Environment.Exit(1);

                }
            }
            else
            {
                sourceFiles.Add(sf);
            }

            VsTest2Xml.Convert2Xml(sourceFiles, GetTarget(argList));

            Environment.Exit(0);

        }

        private static string GetTarget(List<string> argList)
        {
            var target = argList.Find(x => x.StartsWith("/t:", StringComparison.OrdinalIgnoreCase));
            return string.IsNullOrEmpty(target) ||
                   !string.Equals(target.Substring(4), "junit", StringComparison.OrdinalIgnoreCase)
                ? "nunit"
                : "junit";
        }

        
        private static string GetSourceDirectoryPath(List<string> argList)
        {
            var arg = argList.Find(x => x.StartsWith("/sd:", true, CultureInfo.CurrentCulture));
            if (!string.IsNullOrEmpty(arg))
                return arg.Substring(4);
            return null;
        }

        private static string GetSourceFilePath(List<string> argList)
        {
            var arg = argList.Find(x => x.StartsWith("/sf:", true, CultureInfo.CurrentCulture));
            if (!string.IsNullOrEmpty(arg))
                return arg.Substring(4);
            return null;
        }

        private static bool ValidArgs(List<string> argsList)
        {
            if (argsList == null || argsList.Count == 0)
                return false;

            var sourceFileArg = argsList.Find(x => x.StartsWith("/sf:", true, CultureInfo.CurrentCulture));
            var sourceDirArg = argsList.Find(x => x.StartsWith("/sd:", true, CultureInfo.CurrentCulture));

            if (string.IsNullOrEmpty(sourceFileArg) && string.IsNullOrEmpty(sourceDirArg))
                return false;

            if (!string.IsNullOrEmpty(sourceFileArg) && !File.Exists(sourceFileArg.Substring(4)))
                return false;

            if (!string.IsNullOrEmpty(sourceDirArg))
            {
                var directory = sourceDirArg.Substring(4);
                if (!Directory.Exists(directory))
                    return false;
            }

            return true;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage");
            Console.WriteLine("VSTest2NUnit.exe /sf:<SourceFile> /t:<Nunit|JUnit Nunit is default>");
            Console.WriteLine("VSTest2NUnit.exe /sd:<SourceDir> /t:<Nunit|JUnit Nunit is default>");
        }
    }
}
