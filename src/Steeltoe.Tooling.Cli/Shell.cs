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
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.Cli
{
    public class Shell
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Shell>();

        private static int _count;

        public struct Result
        {
            public int Id { get; internal set; }
            public string Command { get; internal set; }
            public string Arguments { get; internal set; }
            public string WorkingDirectory { get; internal set; }
            public int ExitCode { get; internal set; }
            public string Out { get; internal set; }
            public string Error { get; internal set; }
        }

        public static Result Run(string command, string arguments = null, string workingDirectory = null)
        {
            var result = new Result()
            {
                Id = ++_count,
                Command = command,
                Arguments = arguments,
                WorkingDirectory = workingDirectory
            };
            Logger.LogDebug($"[{result.Id}] command: {result.Command}");
            Logger.LogDebug($"[{result.Id}] arguments: {result.Arguments}");
            Logger.LogDebug($"[{result.Id}] working directory: {result.WorkingDirectory}");

            var pinfo = new ProcessStartInfo(result.Command, result.Arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = result.WorkingDirectory
            };
            var proc = Process.Start(pinfo);
            if (proc == null)
            {
                // TODO: better exception implementation
                throw new Exception("OOPS, proc is null. Something bac happened.");
            }

            proc.WaitForExit();
            result.ExitCode = proc.ExitCode;
            Logger.LogDebug($"[{result.Id}] exit code: {result.ExitCode}");
            using (var pout = proc.StandardOutput)
            {
                result.Out = pout.ReadToEnd();
            }

            Logger.LogDebug($"[{result.Id}] stdout: {result.Out}");

            using (var perr = proc.StandardError)
            {
                result.Error = perr.ReadToEnd();
            }

            Logger.LogDebug($"[{result.Id}] stderr: {result.Error}");

            return result;
        }
    }
}
