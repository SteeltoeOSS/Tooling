// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Steeltoe.Tooling
{
    /// <summary>
    /// A Cli (command line interface) that can be used to run system commands.
    /// </summary>
    public class Cli
    {
        /// <summary>
        /// The Cli command run or to be run.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// The shell with which to run a command.
        /// </summary>
        private Shell Shell { get; }

        /// <summary>
        /// Creates a new Cli with the specified command and shell.
        /// </summary>
        /// <param name="command">System command to be run.</param>
        /// <param name="shell">Shell with which to run command.</param>
        public Cli(string command, Shell shell)
        {
            Command = command;
            Shell = shell;
        }

        /// <summary>
        /// Run the Cli's command using the specified arguments.
        /// </summary>
        /// <param name="args">Cli command arguments.</param>
        /// <returns>The command output.</returns>
        /// <exception cref="CliException">Thrown if the command fails.</exception>
        public string Run(string args)
        {
            var result = Shell.Run(Command, args);
            if (result.ExitCode != 0)
            {
                throw new CliException(result.ExitCode, $"{Command} {args}", result.Error.Trim());
            }

            return result.Out;
        }
    }
}
