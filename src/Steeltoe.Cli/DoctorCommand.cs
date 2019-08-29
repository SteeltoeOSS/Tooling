// Copyright 2018 the original author or authors.
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

using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling;
using Steeltoe.Tooling.Executors;

namespace Steeltoe.Cli
{
    [Command(Description = "Check for potential problems")]
    public class DoctorCommand : Command
    {
        public const string Name = "doctor";

        public DoctorCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            return new DoctorExecutor();
        }
    }

    internal class DoctorExecutor : Executor
    {
        protected override void Execute()
        {
            Context.Console.WriteLine($"Steeltoe Developer Tools version ... {Program.GetVersion()}");

            // dotnet
            Context.Console.Write("DotNet ... ");
            var dotnetVersion = new Tooling.Cli("dotnet", Context.Shell).Run("--version", "getting dotnet version")
                .Trim();
            Context.Console.WriteLine($"dotnet version {dotnetVersion}");

            // is intialized?
            Context.Console.Write("initialized ... ");
            var cfgFile = new ConfigurationFile(Context.ProjectDirectory);
            if (!cfgFile.Exists())
            {
                Context.Console.WriteLine($"!!! no (run '{Program.Name} {InitCommand.Name}' to initialize)");
                return;
            }

            Context.Console.WriteLine("yes");

            // target deployment environment
            Context.Console.Write("target ... ");
            string target = cfgFile.Configuration.Target;
            if (target == null)
            {
                Context.Console.WriteLine($"!!! not set (run '{Program.Name} {TargetCommand.Name} <env>' to set)");
                return;
            }

            Context.Console.WriteLine(target);
            Registry.GetTarget(target).IsHealthy(Context);
        }
    }
}
