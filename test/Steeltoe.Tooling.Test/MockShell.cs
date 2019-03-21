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

using System.Collections.Generic;
using System.IO;

namespace Steeltoe.Tooling.Test
{
    public class MockShell : Shell
    {
        public MockShell(TextWriter console) : base(console)
        {
        }

        public string NextResponse { get; set; } = "";

        public int NextExitCode;

        public string LastCommand { get; private set; }

        public List<string> Commands { get; } = new List<string>();

        public override Result Run(string command, string args = null, string workingDirectory = null)
        {
            LastCommand = $"{command} {args}";
            Commands.Add(LastCommand);
            var result = new Result();
            result.ExitCode = NextExitCode;
            if (NextExitCode == 0)
            {
                result.Out = NextResponse;
            }
            else
            {
                result.Error = NextResponse;
            }

            NextResponse = "";
            NextExitCode = 0;
            return result;
        }
    }
}
