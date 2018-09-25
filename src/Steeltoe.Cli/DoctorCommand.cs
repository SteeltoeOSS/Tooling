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
using Steeltoe.Tooling;
using Steeltoe.Tooling.Executor;

namespace Steeltoe.Cli
{
    [Command(Description = "Check for potential problems.")]
    public class DoctorCommand : Command
    {
        public const string Name = "doctor";

        protected override IExecutor GetExecutor()
        {
            return new DoctorExecutor();
        }
    }

    internal class DoctorExecutor : IExecutor
    {
        public void Execute(Context context)
        {
            var console = context.Shell.Console;
            console.WriteLine($"Steeltoe Developer Tools version ... {Program.GetVersion()}");

            // is intialized?
            console.Write("initialized ... ");
            if (!Program.ConfigurationFile.Exists())
            {
                console.WriteLine($"!!! no (run '{Program.Name} {InitCommand.Name}' to initialize)");
                return;
            }

            console.WriteLine("yes");

            // target deployment environment
            console.Write("target deployment environment ... ");
            var target = Program.ConfigurationFile.Configuration.EnvironmentName;
            if (target == null)
            {
                console.WriteLine($"!!! not set (run '{Program.Name} {TargetCommand.Name} <env>' to set)");
                return;
            }

            console.WriteLine(target);
            EnvironmentRegistry.ForName(target).IsHealthy(context.Shell);
        }
    }
}
