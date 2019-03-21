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

using System.IO;

namespace Steeltoe.Tooling
{
    public abstract class Shell
    {
        public TextWriter Console { get; }

        protected Shell(TextWriter console)
        {
            Console = console;
        }

        public abstract Result Run(string command, string args = null, string workingDirectory = null);

        public struct Result
        {
            public int Id { get; set; }
            public string Command { get; set; }
            public string Arguments { get; set; }
            public string WorkingDirectory { get; set; }
            public int ExitCode { get; set; }
            public string Out { get; set; }
            public string Error { get; set; }
        }
    }
}
