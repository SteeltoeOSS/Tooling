// Copyright 2020 the original author or authors.
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

using System;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executors;

namespace Steeltoe.Cli
{
    [Command(Description = "Displays the project details",
        ExtendedHelpText = @"
Overview:
  *** under construction ***

Examples:
  Show the details of the project in the current directory:
  $ st show

  Show the details of the project in a specific directory:
  $ st show --project src/MyProj")]
    public class ShowCommand : Command
    {
        public const string CommandName = "show";

        [Option("--project-dir <path>",
            Description = "Sets the location of the project; default is the current directory")]
        private string ProjectDirectory { get; set; }

        public ShowCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            throw new NotImplementedException();
        }
    }
}
