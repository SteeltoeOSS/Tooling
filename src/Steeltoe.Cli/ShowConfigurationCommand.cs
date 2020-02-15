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
    [Command(Description = "Displays configuration details",
        ExtendedHelpText = @"
Overview:
  *** under construction ***

Examples:
  Display the details of the default configuration:
  $ st show-cfg

  Display the details of a specific configuration:
  $ st show-cfg --name MyCustomDockerConfig")]
    public class ShowConfigurationCommand : Command
    {
        public const string CommandName = "show-cfg";

        [Option("-n|--name <name>",
            Description = "Sets the name of the configuration to be displayed")]
        private string Name { get; set; }

        public ShowConfigurationCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            throw new NotImplementedException();
        }
    }
}
