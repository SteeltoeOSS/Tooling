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

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents a system command shell.
    /// </summary>
    public abstract class Shell
    {
        /// <summary>
        /// Implemetations return the result of running a command.
        /// </summary>
        /// <param name="command">System command.</param>
        /// <param name="args">Command arguments.</param>
        /// <param name="workingDirectory">Command working directory.</param>
        /// <returns></returns>
        public abstract Result Run(string command, string args = null, string workingDirectory = null);

        /// <summary>
        /// A struct representing the result of shell command.
        /// </summary>
        public struct Result
        {
            /// <summary>
            /// An arbitrary ID that can be used to identify a command, such as in a log file.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// System command.
            /// </summary>
            public string Command { get; set; }

            /// <summary>
            /// Command arguments.
            /// </summary>
            public string Arguments { get; set; }

            /// <summary>
            /// Command working directory.
            /// </summary>
            public string WorkingDirectory { get; set; }

            /// <summary>
            /// Command exit code as reported by system.
            /// </summary>
            public int ExitCode { get; set; }

            /// <summary>
            /// Command stdout.
            /// </summary>
            public string Out { get; set; }

            /// <summary>
            /// Command stderr.
            /// </summary>
            public string Error { get; set; }
        }
    }
}
