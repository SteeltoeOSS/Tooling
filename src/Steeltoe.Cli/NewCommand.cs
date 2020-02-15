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
    [Command(Description = "Creates a new project using Steeltoe Initializr",
        ExtendedHelpText = @"
Overview:
  Create a new project using Steeltoe Initializr.  If the output directory exists and is not empty, --force must be specified.

Examples:
  Create a new project in the current directory:
  $ st new

  Re-create a project in the current directory:
  $ st new --force

  Create a new project in a new directory:
  $ st new --project-dir src/MyProj

  Create a new project with dependencies on SQL Server and Redis:
  $ st new --dependency sqlserver --dep redis

  Create a new project with dependencies on a SQL Server service named MyDB:
  $ st new --dependency sqlserver:MyDB

  Create a new project with a custom name and using the Steeltoe-React template:
  $ st new --name MyCompany.MySample --template Steeltoe-React

  Create a new project for netcoreapp2.2:
  $ st new --framework netcoreapp2.2

See Also:
  show
  list-templates
  list-deps"
    )]
    public class NewCommand : Command
    {
        public const string CommandName = "new";

        [Option("-n|--name <name>",
            Description = "Sets the project name; default is the name of the current directory")]
        private string Name { get; set; }

        [Option("--project-dir <path>",
            Description = "Sets the location to place the generated project files; default is the current directory")]
        private string ProjectDirectory { get; set; }

        [Option("-f|--framework <framework>", Description = "Sets the project framework")]
        private string Framework { get; set; }

        [Option("-t|--template <template>", Description = "Sets the Initializr template")]
        private string Template { get; set; }

        [Option("-d|--dependency <dep>|<dep>:<depname>",
            Description = "Adds the named Initializr dependency; may be specified multiple times")]
        private string Dependency { get; set; }

        [Option("-F|--force",
            Description = "Forces project files to be generated even if it would change existing files")]
        private bool Force { get; set; }

        public NewCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            throw new NotImplementedException();
        }
    }
}
