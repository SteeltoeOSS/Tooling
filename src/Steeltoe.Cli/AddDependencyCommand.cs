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
using Steeltoe.Tooling.Controllers;

namespace Steeltoe.Cli
{
    [Command(Description = "Adds a dependency",
        ExtendedHelpText = @"
Overview:
  Explicitly add a dependency to the project.  Useful when autodetection fails.

Examples:
  Add a dependency on a Redis service:
  $ st add-dep redis

  Add a dependency on a Redis service and name it MyRedis:
  $ st add-dep redis --name MyRedis

See Also:
  rem-dep
  list-deps"
    )]
    public class AddDependencyCommand : Command
    {
        public const string CommandName = "add-dep";

        [Required(ErrorMessage = "Dependency not specified")]
        [Argument(0, Name = "dep", Description = "The dependency to be added")]
        private string Dependency { get; set; }

        [Option("-n|--name", Description = "Sets the dependency name; default is <dep>")]
        private string DependencyName { get; set; }

        public AddDependencyCommand(IConsole console) : base(console)
        {
        }

        protected override Controller GetController()
        {
            throw new NotImplementedException();
        }
    }
}
