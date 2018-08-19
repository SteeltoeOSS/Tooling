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

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Cli.Executors.Target;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable InconsistentNaming

namespace Steeltoe.Tooling.Cli.Commands
{
    [Command(Description = "Set the target environment.")]
    public class SetTargetCommand : Command
    {
        [Required(ErrorMessage = "Environment not specified")]
        [Argument(0, Description = "The environment")]
        private string environment { get; }

        protected override IExecutor GetExecutor()
        {
            return new SetTargetExecutor(Configuration.Load(), environment);
        }
    }
}
