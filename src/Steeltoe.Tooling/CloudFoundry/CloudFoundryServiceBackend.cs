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

using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryServiceBackend : IServiceBackend
    {
        private readonly Context _context;

        private readonly CloudFoundryCli _cli;

        public CloudFoundryServiceBackend(Context context)
        {
            _context = context;
            _cli = new CloudFoundryCli(_context.Shell);
        }

        public void DeployService(string name, string type)
        {
            var cfServiceDef = _context.Environment.Configuration.ServiceTypes[type];
            _cli.Run($"create-service {cfServiceDef["service"]} {cfServiceDef["plan"]} {name}");
        }

        public void UndeployService(string name)
        {
            _cli.Run($"delete-service {name} -f");
        }

        public ServiceLifecycle.State GetServiceLifecleState(string name)
        {
            try
            {
                var serviceInfo = _cli.Run($"service {name}");
                var state = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline).Match(serviceInfo).Groups[1]
                    .ToString().TrimEnd();
                switch (state)
                {
                    case "create in progress":
                        return ServiceLifecycle.State.Starting;
                    case "create succeeded":
                        return ServiceLifecycle.State.Online;
                }
            }
            catch (CliException e)
            {
                if (e.Error.Contains($"Service instance {name} not found"))
                {
                    return ServiceLifecycle.State.Offline;
                }
            }

            return ServiceLifecycle.State.Unknown;
        }
    }
}
