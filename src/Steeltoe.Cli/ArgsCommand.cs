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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executor;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Steeltoe.Cli
{
    [Command(
        Description = "Set or get the deployment environment arguments for a service.",
        ExtendedHelpText =
            "If run with no deployment environment arguments, show the service's current deployment environment arguments.",
        AllowArgumentSeparator = true
    )]
    public class ArgsCommand : Command
    {
        public const string Name = "args";

        [Required(ErrorMessage = "Deployment environment not specified")]
        [Argument(0, Name = "environment", Description = "Deployment environment")]
        private string EnvironmentName { get; }

        [Required(ErrorMessage = "Service name not specified")]
        [Argument(1, Name = "service", Description = "Service name")]
        private string ServiceName { get; }

        [Argument(2, Name = "args", Description = "Deployment environment arguments")]
        private List<string> Arguments { get; }

        private List<string> RemainingArguments { get; }

        protected override IExecutor GetExecutor()
        {
            var args = new List<string>();
            if (Arguments != null)
            {
                args.AddRange(Arguments);
            }

            if (RemainingArguments != null)
            {
                args.AddRange(RemainingArguments);
            }

            if (args.Count == 0)
            {
                return new GetServiceDeploymentArgsExecutor(EnvironmentName, ServiceName);
            }

            return new SetServiceDeploymentArgsExecutor(EnvironmentName, ServiceName, string.Join(" ", args));
        }
    }
}
