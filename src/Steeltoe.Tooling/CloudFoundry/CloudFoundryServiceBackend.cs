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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryServiceBackend : IServiceBackend
    {
        private static Dictionary<string, string> serviceMap = new Dictionary<string, string>
        {
            {"config-server", "p-config-server"},
            {"registry", "p-service-registry"}
        };

        private readonly CloudFoundryCli _cli;

        public CloudFoundryServiceBackend(Shell shell)
        {
            _cli = new CloudFoundryCli(shell);
        }

        public void DeployService(string name, string type)
        {
            _cli.Run($"create-service {serviceMap[type]} standard {name}");
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
                Regex exp = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline);
                Match match = exp.Match(serviceInfo);
                if (match.Groups[1].ToString().TrimEnd().Equals("create succeeded"))
                {
                    return ServiceLifecycle.State.Online;
                }
            }
            catch (CliException)
            {
                // TODO: check to ensure error is due to missing service and not some other error
            }

            return ServiceLifecycle.State.Offline;
        }
    }
}
