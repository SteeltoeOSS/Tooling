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

using System;
using System.IO;
using System.Linq;

namespace Steeltoe.Tooling.Cli.Executors.Service
{
    public class AddServiceExecutor : ServiceExecutor
    {
        private string Type { get; }

        public AddServiceExecutor(string name, string type) : base(name)
        {
            Type = type;
        }

        public override bool Execute(Configuration config, Shell shell, TextWriter output)
        {
            if (config.services.ContainsKey(Name))
            {
                throw new ArgumentException($"Service '{Name}' already exists");
            }

            var type = Type.ToLower();
            if (!ServiceTypeRegistry.GetNames().Contains(type))
            {
                throw new ArgumentException($"Unknown service type '{Type}'");
            }

            config.services.Add(Name, new Configuration.Service(type));
            output.WriteLine($"Added {type} service '{Name}'");
            return true;
        }
    }
}
