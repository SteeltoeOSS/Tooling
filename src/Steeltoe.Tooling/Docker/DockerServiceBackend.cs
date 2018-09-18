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
using System.Collections.Generic;

namespace Steeltoe.Tooling.Docker
{
    public class DockerServiceBackend : IServiceBackend
    {
        internal struct ImageSpec
        {
            internal string Name { get; set; }
            internal int Port { get; set; }
        }

        private static Dictionary<string, ImageSpec> imageSpecs = new Dictionary<string, ImageSpec>
        {
            {
                "config-server", new ImageSpec
                {
                    Name = "steeltoeoss/configserver",
                    Port = 8888
                }
            },
            {
                "registry", new ImageSpec
                {
                    Name = "steeltoeoss/eurekaserver",
                    Port = 8761
                }
            }
        };

        private readonly DockerCli _cli;

        public DockerServiceBackend(Shell shell)
        {
            _cli = new DockerCli(shell);
        }

        public void DeployService(string name, string type)
        {
            var spec = imageSpecs[type];
            _cli.StartContainer(name, spec.Name, spec.Port);
        }

        public void UndeployService(string name)
        {
            _cli.StopContainer(name);
        }

        public ServiceLifecycle.State GetServiceLifecleState(string name)
        {
            var containerInfo = _cli.GetContainerInfo(name).Split('\n');
            if (containerInfo.Length > 2)
            {
                var statusStart = containerInfo[0].IndexOf("STATUS", StringComparison.Ordinal);
                if (containerInfo[1].Substring(statusStart).StartsWith("Up "))
                {
                    return ServiceLifecycle.State.Online;
                }
            }

            return ServiceLifecycle.State.Offline;
        }
    }
}
