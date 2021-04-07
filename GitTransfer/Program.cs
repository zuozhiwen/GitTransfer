using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GitTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Any() != true || args.Length == 1 && (args[0] == "/?" || args[0] == "--help"))
            {
                Console.WriteLine("gitTransfer --since \"(2021/4/1 | 2021/4/1 12:00)\" SrcSourceCodePath DstSourceCodePath");
                return;
            }


            var argsList = args.ToList();
            var sinceIndex = argsList.IndexOf("--since");
            if (sinceIndex > args.Length - 1)
            {
                Console.WriteLine("Arguments Invalid");
                return;
            }

            var sinceDate = argsList[sinceIndex + 1];
            argsList.RemoveRange(sinceIndex, 2);

            if (argsList.Count < 2)
            {
                Console.WriteLine("Arguments Invalid");
                return;
            }

            var src = new DirectoryInfo(argsList[0]);
            var dst = new DirectoryInfo(argsList[1]);

            if (!src.Exists || !dst.Exists)
            {
                Console.WriteLine("Directory not Exist!");
                return;
            }

            var startupInfo = new ProcessStartInfo
            {
                FileName = "git.exe",
                Arguments = $"--no-pager log --since {sinceDate} --name-only --format=\"\" --no-merges",
                UseShellExecute = false,
                //WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = src.FullName,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(startupInfo);
            string output = process.StandardOutput.ReadToEnd();
            if (output.Any())
            {
                var affectedFiles = output.Split('\n').Where(o => o?.Any() == true).Select(o => o).Distinct().ToList();
                foreach (var item in affectedFiles)
                {
                    var srcFileName = new FileInfo(Path.Combine(src.FullName, item));
                    if (!srcFileName.Exists)
                    {
                        Console.WriteLine($"File Not Exist : {srcFileName.FullName} SKIPPED!");
                        continue;
                    }

                    var dstFileName = new FileInfo(Path.Combine(dst.FullName, item));
                    srcFileName.CopyTo(dstFileName.FullName, true);
                    Console.WriteLine($"{item} COPY!");
                }
            }
        }
    }
}

