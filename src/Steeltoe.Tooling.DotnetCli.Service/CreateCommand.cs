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
using Steeltoe.Tooling.DotnetCli.Base;

namespace Steeltoe.Tooling.DotnetCli.Service
{
    [Command(Description = "Create a service, e.g. a Cloud Foundry Config Server.")]
    public class CreateCommand : DotnetCliCommand
    {
        [Argument(0, Description = "The name of the service being created.")]
        private string Name { get; }

        [Argument(1, Description = "The type of the service being created. One of: cloud-foundry-config-server")]
        private string Type { get; }

        protected override void OnCommandExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new UsageException("name not specified");
            }

            app.Out.WriteLine($"creating service named '{Name}' of type {Type}");
        }
    }
}
