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
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executors;

namespace Steeltoe.Cli
{
    [Command(Description = "Adds a custom dependency definition",
        ExtendedHelpText = @"
Overview:
  *** under construction ***

Examples:
  Add a dependency definition for a service that listens on a couple of network ports:
  $ st def-dep MyService myrepo/myimage --port 9876 --port 9877
  Add a dependency definition for a service that can used in all projects:
  $ st def-dep MyService myrepo/myimage --scope global
  Add a dependency definition for a service that can be autodetected:
  $ st def-dep MyService myrepo/myimage --nuget-package My.Service.NuGet

See Also:
  undef-dep
  list-deps"
    )]
    public class DefineDependencyCommand : Command
    {
        public const string CommandName = "def-dep";

        [Required(ErrorMessage = "Dependency not specified")]
        [Argument(0, Name = "dep", Description = "Dependency")]
        private string Dependency { get; set; }

        [Required(ErrorMessage = "Docker image not specified")]
        [Argument(1, Name = "image", Description = "Docker image")]
        private string DockerImage { get; set; }

        [Option("-p|--port <port>",
            Description = "Sets a network port; may be specified multiple times")]
        private int Port { get; set; }

        [Option("--nuget-package <name>",
            Description = "Sets a NuGet package name for autodetection; may be specified multiple times")]
        private string NugetPackage { get; set; }

        [Option("--scope <scope>",
            Description = "Sets the dependency definition scope (one of: project, global); default is project")]
        private string Scope { get; set; }

        public DefineDependencyCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            throw new NotImplementedException();
        }
    }
}
