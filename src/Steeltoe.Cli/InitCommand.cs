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

using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executor;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Steeltoe.Cli
{
    [Command(Description = "Initialize Steeltoe Developer Tools")]
    public class InitCommand : Command
    {
        public const string Name = "init";

        [Option("-F|--force", Description = "Initialize the project even if already initialized")]
        private bool Force { get; }

        public InitCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            return new InitializationExecutor(Program.ProjectConfigurationPath, Force);
        }
    }
}
