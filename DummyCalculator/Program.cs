using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DummyCalculator.Extensions;

namespace DummyCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Ready for debugger to attach. Process ID: {Process.GetCurrentProcess().Id}.");
            Console.WriteLine("Press ENTER to continue.");
            Console.ReadLine();

            Console.WriteLine("[Master] Starting Calculator");

            new Program().InvokeOperation(args[0], args.Slice<string, double>(1)).Wait();

            Console.WriteLine("[Master] Calculation complete.");
            Console.ReadKey();
        }

        private async Task InvokeOperation(string operationName, params double[] args)
        {
            var pluginPath = Path.Combine(AppContext.BaseDirectory, "plugins");

            var pluginDll = Path.Combine(pluginPath, operationName, $"{operationName}.dll");
            var pluginArgs = string.Join(" ", args);

            var startInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $"\"{pluginDll}\" {pluginArgs}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                Console.WriteLine($"[Master] Command: {process.StartInfo.FileName} {process.StartInfo.Arguments}");

                var reader = process.StandardOutput;
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    Console.WriteLine($"[Plugin] {line}");
                }
            }
        }
    }
}
