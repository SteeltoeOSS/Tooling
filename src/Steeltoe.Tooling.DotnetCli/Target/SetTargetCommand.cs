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

using McMaster.Extensions.CommandLineUtils;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable InconsistentNaming

namespace Steeltoe.Tooling.DotnetCli.Target
{
    [Command(Description = "Set the target environment.")]
    public class SetTargetCommand : DotnetCliCommand
    {
        [Argument(0, Description = "The target environment")]
        private string environment { get; }

        protected override void OnCommandExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(environment))
            {
                throw new CommandException("Environment not specified");
            }

            switch (environment.ToLower())
            {
                case "cloud-foundry":
                    break;
                default:
                    throw new CommandException($"Unknown environment '{environment}'");
            }
            var cfg = new ToolingConfiguration();
            cfg.target = environment;
            cfg.Store(".");
            app.Out.WriteLine($"Target set to '{environment}'.");
        }
    }
}
