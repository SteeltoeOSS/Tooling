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

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.System
{
    public class Shell
    {
        private static int count = 0;

        public string Command { get; private set; }
        public string Arguments { get; private set; }
        public int ExitCode { get; private set; }
        public string Out { get; private set; }
        public string Error { get; private set; }

        private ILogger<Shell> Logger { get; set; }

        public Shell(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<Shell>();
        }

        public void Run(string command, string arguments = null, string workingDirectory = null)
        {
            var commandId = ++count;

            Command = command;
            Arguments = arguments;
            Logger.LogDebug(string.IsNullOrEmpty(Arguments)
                ? $"[{commandId}] command: {command}"
                : $"[{commandId}] command: {command} {arguments}");

            var pinfo = new ProcessStartInfo(Command, Arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };
            var proc = Process.Start(pinfo);
            proc.WaitForExit();
            ExitCode = proc.ExitCode;
            Logger.LogDebug($"[{commandId}] exit code: {ExitCode}");
            using (var pout = proc.StandardOutput)
            {
                Out = pout.ReadToEnd();
            }
            Logger.LogDebug(string.IsNullOrEmpty(Out)
                ? $"[{commandId}] stdout: <none>"
                : $"[{commandId}] stdout: {Out}");

            using (var perr = proc.StandardError)
            {
                Error = perr.ReadToEnd();
            }
            Logger.LogDebug(string.IsNullOrEmpty(Error)
                ? $"[{commandId}] stderr: <none>"
                : $"[{commandId}] stderr: {Error}");
        }
    }
}
