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

namespace Steeltoe.Tooling.Cli.Environments.Docker
{
    public class DockerServiceManager : IServiceManager
    {
        private static Dictionary<string, string> imageMap = new Dictionary<string, string>
        {
            {"config-server", "steeltoeoss/configserver"}
        };

        public void StartService(Shell shell, string name, string type)
        {
            new DockerCli(shell).StartContainer(name, imageMap[type]);
        }

        public void StopService(Shell shell, string name)
        {
            new DockerCli(shell).StopContainer(name);
        }

        public string CheckService(Shell shell, string name)
        {
            var containerInfo = new DockerCli(shell).GetContainerInfo(name).Split('\n');
            if (containerInfo.Length <= 2) return "offline";
            var statusStart = containerInfo[0].IndexOf("STATUS", StringComparison.Ordinal);
            return containerInfo[1].Substring(statusStart).StartsWith("Up ") ? "online" : "offline";
        }
    }
}
