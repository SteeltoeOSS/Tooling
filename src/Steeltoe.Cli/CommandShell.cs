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

using System.ComponentModel;
using System.Diagnostics;
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
            Logger.LogDebug($"[{result.Id}] command: {result.Command}");
            Logger.LogDebug($"[{result.Id}] arguments: {result.Arguments}");
            Logger.LogDebug($"[{result.Id}] working directory: {result.WorkingDirectory}");

            var pinfo = new ProcessStartInfo(result.Command, result.Arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            if (result.WorkingDirectory != null)
            {
                pinfo.WorkingDirectory = result.WorkingDirectory;
            }

            try
            {
                var proc = Process.Start(pinfo);
                if (proc == null)
                {
                    throw new ShellException("Unable to start process; no additional information available");
                }

                proc.WaitForExit();
                result.ExitCode = proc.ExitCode;
                Logger.LogDebug($"[{result.Id}] exit code: {result.ExitCode}");
                using (var pout = proc.StandardOutput)
                {
                    result.Out = pout.ReadToEnd();
                }

                if (!string.IsNullOrWhiteSpace(result.Out))
                {
                    Logger.LogDebug($"[{result.Id}] stdout:{FormatOutputForLogger(result.Out)}");
                }

                using (var perr = proc.StandardError)
                {
                    result.Error = perr.ReadToEnd();
                }

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

        private static string FormatOutputForLogger(string output)
        {
            var newLine = System.Environment.NewLine;
            var buf = new StringBuilder();
            buf.Append(newLine);
            buf.Append("| ");
            buf.Append(output.TrimEnd().Replace(newLine, $"{newLine}| "));
            return buf.ToString();
        }
    }
}
