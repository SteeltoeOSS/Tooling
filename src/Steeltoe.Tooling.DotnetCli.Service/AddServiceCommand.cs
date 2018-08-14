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
using Steeltoe.Tooling.Base;
using Steeltoe.Tooling.DotnetCli.Base;

namespace Steeltoe.Tooling.DotnetCli.Service
{
    [Command(Description = "Add a service to the target environment.")]
    public class AddServiceCommand : DotnetCliCommand
    {
        [Argument(0, Description = "The service name")]
        private string name { get; }

        [Option("-s|--service-type", Description = "The service type")]
        private string type { get; }

        protected override void OnCommandExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new UsageException("Service name not specified");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new UsageException("Service type not specified");
            }

            switch (type.ToLower())
            {
                case "cloud-foundry-config-server":
                    break;
                default:
                    throw new CommandException($"Unknown service type '{type}'");
            }

            var cfg = new ToolingConfiguration();
            var svc = new ToolingConfiguration.Service();
            svc.type = type;
            cfg.services.Add(name, svc);
            cfg.Store(".");
            app.Out.WriteLine($"Added {type} service '{name}'");
        }
    }
}
