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
    [Command(Description = "Creates configuration files for a target",
        ExtendedHelpText = @"
Overview:
  Creates configuration files for a target.  By default, files are generated for the Docker target.

  Docker configurations can subsequently be used by the run command.

Examples:
  Create the default configuration:
  $ st new-cfg

  Re-create the default configuration for a project in a specific directory:
  $ st new-cfg --force --project-dir src/MyProj

  Create a configuration for Kubernetes using the netcoreapp2.1 framework:
  $ st new-cfg --target k8s --framework netcoreapp2.1

  Create a named configuration with application arguments:
  $ st new-cfg --name MyCustomDockerConfig --application-arg arg1 --application-arg arg2

  Create a configuration that sets an environment variable for a dependency:
  $ st new-cfg --dependency-env MyDB:ACCEPT_EULA=Y

See Also:
  show-cfg
  run"
    )]
    public class NewConfigurationCommand : Command
    {
        public const string CommandName = "new-cfg";

        [Option("-T|--target <target>",
            Description =
                "Sets the configuration target (one of: ‘Docker’, ‘Kubernetes’, ‘Cloud-Foundry’); default is 'Docker'")]
        private string Target { get; set; } = "Docker";

        [Option("-n|--name <cfgname>", Description = "Sets the configuration name; default is the project name")]
        private string Name { get; set; }

        [Option("-o|--output <path>",
            Description = "Sets the location to place the generated configuration; default is the current directory")]
        private string OutputPath { get; set; }

        [Option("-f|--framework <framework>", Description = "Sets the framework; default is the project framework")]
        private string Framework { get; set; }

        [Option("-a|--arg <arg>", Description = "Sets a command line argument for the application")]
        private string Argument { get; set; }

        [Option("-e|--env <name>=<value>",
            Description = "Sets an environment variable for the application; may be specified multiple times")]
        private string ApplicationEnvironmentVariable { get; set; }

        [Option("--dep-arg <depname>:<arg>",
            Description = "Sets a command line argument for the named dependency")]
        private string DependencyArgument { get; set; }

        [Option("--dep-env <depname>:<name>=<value>",
            Description =
                "Sets an environment variable for the named dependency; may be specified multiple times")]
        private string DependencyEnvironmentVariable { get; set; }

        [Option("-F|--force",
            Description = "Forces configuration to be generated even if it would change existing files")]
        private bool Force { get; set; }

        public NewConfigurationCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            throw new NotImplementedException();
        }
    }
}
