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
        Description = "Set or get the arguments for an app or service",
        ExtendedHelpText =
            "If run with no arguments, show the current arguments for the app or service.",
        AllowArgumentSeparator = true
    )]
    public class ArgsCommand : Command
    {
        public const string Name = "args";

        [Required(ErrorMessage = "App or service name not specified")]
        [Argument(0, Name = "name", Description = "App or service name")]
        private string AppOrServiceName { get; }

        [Argument(2, Name = "args", Description = "App or service arguments")]
        private List<string> Arguments { get; }

        [Option("-t|--target", Description = "Apply the args to the deployment on the specified target")]
        private string Target { get; }

        [Option("-F|--force", Description = "Overwrite existing arguments")]
        private bool Force { get; }

        private List<string> RemainingArguments { get; }

        public ArgsCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
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
                return new GetArgsExecutor(AppOrServiceName, Target);
            }

            return new SetArgsExecutor(AppOrServiceName, Target, string.Join(" ", args), Force);
        }
    }
}
