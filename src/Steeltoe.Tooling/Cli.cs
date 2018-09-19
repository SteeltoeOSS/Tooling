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
    public class Cli
    {
        public string Command { get; }

        public Shell Shell { get; }

        public Cli(string command, Shell shell)
        {
            Command = command;
            Shell = shell;
        }

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
