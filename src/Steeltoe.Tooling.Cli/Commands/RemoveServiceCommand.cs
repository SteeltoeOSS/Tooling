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
using Steeltoe.Tooling.Cli.Executors.Service;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable InconsistentNaming

namespace Steeltoe.Tooling.Cli.Commands
{
    [Command(Description = "Remove a service.")]
    public class RemoveServiceCommand : Command
    {
        [Required(ErrorMessage = "Service name not specified")]
        [Argument(0, Description = "The service name")]
        private string name { get; }

        protected override IExecutor GetExecutor()
        {
            return new RemoveServiceExecutor(name);
        }
    }
}
