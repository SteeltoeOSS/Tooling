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
        private readonly string _name;

        private readonly string _type;

        public AddServiceExecutor(string name, string type)
        {
            _name = name;
            _type = type;
        }

        public bool Execute(Configuration config, Shell shell, TextWriter output)
        {
            if (config.services.ContainsKey(_name))
            {
                throw new CommandException($"Service '{_name}' already exists");
            }

            switch (_type.ToLower())
            {
                case "cloud-foundry-config-server":
                    break;
                default:
                    throw new CommandException($"Unknown service type '{_type}'");
            }

            config.services.Add(_name, new Configuration.Service(_type));
            output.WriteLine($"Added {_type} service '{_name}'");
            return true;
        }
    }
}
