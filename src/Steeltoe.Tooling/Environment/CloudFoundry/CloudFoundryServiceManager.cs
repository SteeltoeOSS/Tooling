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
using Steeltoe.Tooling.Service;
using Steeltoe.Tooling.System;

namespace Steeltoe.Tooling.Environment.CloudFoundry
{
    public class CloudFoundryServiceManager : IServiceManager
    {
        private static Dictionary<string, string> serviceMap = new Dictionary<string, string>
        {
            {"config-server", "p-config-server"},
            {"registry", "p-service-registry"}
        };

        public void StartService(Shell shell, string name, string type)
        {
            new CloudFoundryCli(shell).CreateService(name, serviceMap[type]);
        }

        public void StopService(Shell shell, string name)
        {
            new CloudFoundryCli(shell).DeleteService(name);
        }

        public string CheckService(Shell shell, string name)
        {
            var serviceInfo = new CloudFoundryCli(shell).GetServiceInfo(name);
            Regex exp = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline);
            Match match = exp.Match(serviceInfo);
            if (match.Groups[1].ToString().TrimEnd().Equals("create succeeded"))
            {
                return "online";
            }

            return "offline";
        }
    }
}
