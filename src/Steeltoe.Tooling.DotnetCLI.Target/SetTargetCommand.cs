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

using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.CloudFoundry;
using Steeltoe.Tooling.DotnetCLI.Base;

namespace Steeltoe.Tooling.DotnetCLI.Target
{
    [Command(Description = "Set the target environment, e.g. Cloud Foundry.")]
    public class SetTargetCommand : DotnetCLICommand
    {
        [Argument(0, Description = "Specify one of: cloud-foundry.")]
        private string Environment { get; }

        private string name;
        [Option("-n|--name", Description =
            "The name for the output being created. If no name is specified, the name of the current directory is used.")]
        private string AppName
        {
            get => string.IsNullOrEmpty(name) ? Path.GetFileName(OutputDirectory) : name;
            set => name = value;
        }

        [Option("-o|--output", Description = "Location to place generated output.")]
        private string OutputDirectory { get; set; } = Directory.GetCurrentDirectory();

        [Option("--force", Description = "Forces content to be generated even if it would change existing files.")]
        private bool Force { get; }
        
        protected override void OnCommandExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(Environment))
            {
                throw new UsageException("environment not specified");
            }
            switch (Environment.ToLower())
            {
                case "cloud-foundry":
                    SetTargetToCloudFoundry(app);
                    break;
                default:
                    throw new UsageException("not a valid environment [" + Environment + "]");
            }
        }

        private void SetTargetToCloudFoundry(CommandLineApplication app)
        {
            var path = Path.Combine(OutputDirectory, "manifest.yml");
            if (File.Exists(path) && !Force)
            {
                app.Error.WriteLine("Running this command will make changes to the following file(s):");
                app.Error.WriteLine("  Overwrite  " + path);
                app.Error.WriteLine();
                app.Error.WriteLine("Rerun the command and pass --force.");
                return;
            }
            var config = new CloudFoundryConfiguration
            {
                applications = new []
                {
                    new Application
                    {
                        name = AppName
                    }
                }
            };
                
            Directory.CreateDirectory(OutputDirectory);
            config.store(path);
        }
    }
}
