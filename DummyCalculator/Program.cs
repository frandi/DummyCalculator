using DummyCalculator.Extensions;
using DummyCalculator.Plugins.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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

            var pluginLocation = Path.Combine(AppContext.BaseDirectory, "plugins");

            var pluginManager = new PluginManager(new []{ pluginLocation });
            pluginManager.RefreshPlugins();

            var opType = GetOperationType(args[0]);
            if (opType != OperationType.Undefined)
            {
                var (_, pluginDll) = pluginManager.GetPlugin(opType);
                
                new Program().InvokeOperation(pluginDll, args.Slice<string, double>(1)).Wait();
            }

            Console.WriteLine("[Master] Calculation complete.");
            Console.ReadKey();
        }

        private async Task InvokeOperation(string pluginDll, params double[] args)
        {
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
                if (process != null)
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

        private static OperationType GetOperationType(string input)
        {
            switch (input.ToLower())
            {
                case "addition":
                case "add":
                case "plus":
                case "+":
                    return OperationType.Addition;
                case "subtraction":
                case "subtract":
                case "minus":
                case "-":
                    return OperationType.Subtraction;
                case "fibonacci":
                    return OperationType.Fibonacci;
                default:
                    return OperationType.Undefined;
            }
        }
    }
}
