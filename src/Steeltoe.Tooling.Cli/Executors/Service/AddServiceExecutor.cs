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

namespace Steeltoe.Tooling.Cli.Executors.Service
{
    public class AddServiceExecutor : IExecutor
    {
        private ToolingConfiguration Config { get; }

        private string Name { get; }

        private string Type { get; }

        public AddServiceExecutor(ToolingConfiguration config, string name, string type)
        {
            Config = config;
            Name = name;
            Type = type;
        }

        public void Execute(TextWriter output)
        {
            if (Config.services.ContainsKey(Name))
            {
                throw new CommandException($"Service '{Name}' already exists");
            }

            switch (Type.ToLower())
            {
                case "cloud-foundry-config-server":
                    break;
                default:
                    throw new CommandException($"Unknown service type '{Type}'");
            }

            Config.services.Add(Name, new ToolingConfiguration.Service(Type));
            Config.Store(".");
            output.WriteLine($"Added {Type} service '{Name}'");
        }
    }
}
