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

using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executor;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Steeltoe.Cli
{
    [Command(Description =
            "Set or get the deployment target",
        ExtendedHelpText = "If run with no args, show the current deployment target.")]
    public class TargetCommand : Command
    {
        public const string Name = "target";

        [Argument(0, Name = "target", Description = "Deployment target name")]
        private string Target { get; }

        [Option("-F|--force", Description = "Set the deployment target even if checks fail")]
        private bool Force { get; }

        public TargetCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            if (Target == null)
            {
                return new GetTargetExecutor();
            }

            return new SetTargetExecutor(Target, Force);
        }
    }
}
