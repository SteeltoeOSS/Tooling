// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling;

namespace Steeltoe.Cli
{
    public class CommandShell : Shell
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<CommandShell>();

        private static int _count;

        public override Result Run(string command, string args = null, string workingDirectory = null)
        {
            var result = new Result()
            {
                Id = ++_count,
                Command = command,
                Arguments = args,
                WorkingDirectory = workingDirectory
            };
            var expanded = result.Arguments == null ? result.Command : $"{result.Command} {result.Arguments}";
            Logger.LogDebug($"[{result.Id}] command: {expanded}");
            Logger.LogDebug($"[{result.Id}] working directory: {result.WorkingDirectory}");
            OutputToConsole(expanded);

            var pInfo = new ProcessStartInfo(result.Command, result.Arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            if (result.WorkingDirectory != null)
            {
                pInfo.WorkingDirectory = result.WorkingDirectory;
            }

            try
            {
                var proc = new Process {StartInfo = pInfo};
                var outputWatcher = new Watcher();
                proc.OutputDataReceived += outputWatcher.WatchOutput;
                var errorWatcher = new Watcher();
                proc.ErrorDataReceived += errorWatcher.WatchOutput;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                proc.WaitForExit();
                result.ExitCode = proc.ExitCode;
                Logger.LogDebug($"[{result.Id}] exit code: {result.ExitCode}");
                result.Out = outputWatcher.Data.ToString();
                if (!string.IsNullOrWhiteSpace(result.Out))
                {
                    Logger.LogDebug($"[{result.Id}] stdout:{FormatOutputForLogger(result.Out)}");
                }

                result.Error = errorWatcher.Data.ToString();
                if (!string.IsNullOrWhiteSpace(result.Error))
                {
                    Logger.LogDebug($"[{result.Id}] stderr:{FormatOutputForLogger(result.Error)}");
                }
            }
            catch (Win32Exception e)
            {
                throw new ShellException(e.Message);
            }

            return result;
        }

        private static void OutputToConsole(string output)
        {
            if (!Settings.VerboseEnabled) return;
            var oldFg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Out.WriteLine(output);
            Console.ForegroundColor = oldFg;
        }

        private class Watcher
        {
            internal readonly StringWriter Data = new StringWriter();

            internal void WatchOutput(object sender, DataReceivedEventArgs eventArgs)
            {
                if (eventArgs.Data == null) return;
                Data.WriteLine(eventArgs.Data);
                OutputToConsole(eventArgs.Data);
            }
        }

        private static string FormatOutputForLogger(string output)
        {
            var newLine = Environment.NewLine;
            var buf = new StringBuilder();
            buf.Append(newLine);
            buf.Append("| ");
            buf.Append(output.TrimEnd().Replace(newLine, $"{newLine}| "));
            return buf.ToString();
        }
    }
}
