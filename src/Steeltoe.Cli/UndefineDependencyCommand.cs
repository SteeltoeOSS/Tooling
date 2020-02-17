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
    [Command(Description = "Removes a custom dependency definition",
        ExtendedHelpText = @"
Overview:
  *** under construction ***

Examples:
  Remove a dependency definition:
  $ st undef-dep MyService

See Also:
  def-dep
  list-deps"
    )]
    public class UndefineDependencyCommand : Command
    {
        public const string CommandName = "undef-dep";

        [Required(ErrorMessage = "Dependency not specified")]
        [Argument(0, Name = "dep", Description = "Dependency")]
        private string Dependency { get; set; }

        [Option("--scope <scope>",
            Description = "Sets the dependency definition scope (one of: project, global); default is project")]
        private string Scope { get; set; }

        public UndefineDependencyCommand(IConsole console) : base(console)
        {
        }

        protected override Controller GetController()
        {
            throw new NotImplementedException();
        }
    }
}
