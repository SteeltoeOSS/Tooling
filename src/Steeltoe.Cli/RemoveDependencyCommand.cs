﻿// Copyright 2020 the original author or authors.
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
    [Command(Description = "Removes a dependency that was added using the add-dep command",
        ExtendedHelpText = @"
Overview:
  Remove the named dependency from the project.

Examples:
  Remove the dependency named MyRedis:
  $ st rem-dep MyRedis

See Also:
  add-dep
  list-deps"
    )]
    public class RemoveDependencyCommand : Command
    {
        public const string CommandName = "rem-dep";

        [Required(ErrorMessage = "Dependency name not specified")]
        [Argument(0, Name = "depname", Description = "The name of the dependency to be removed")]
        private string DependencyName { get; set; }

        public RemoveDependencyCommand(IConsole console) : base(console)
        {
        }

        protected override Controller GetController()
        {
            throw new NotImplementedException();
        }
    }
}