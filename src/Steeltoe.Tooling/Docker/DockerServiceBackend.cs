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
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Docker
{
    public class DockerServiceBackend : IServiceBackend
    {
        private static Dictionary<string, string> _imageMap = new Dictionary<string, string>
        {
            {"dummy-svc", "steeltoeoss/dummyserver"},
            {"config-server", "steeltoeoss/config-server:2.0"},
            {"eureka-server", "steeltoeoss/eureka-server:2.0"},
            {"hystrix-dashboard", "steeltoeoss/hystrix-dashboard:1.4"},
        };

        private static Dictionary<string, string> _serviceMap = new Dictionary<string, string>();

        static DockerServiceBackend()
        {
            foreach (var entry in _imageMap)
            {
                _serviceMap[entry.Value] = entry.Key;
            }
        }

        private readonly Context _context;

        private readonly DockerCli _cli;

        internal DockerServiceBackend(Context context)
        {
            _context = context;
            _cli = new DockerCli(_context.Shell);
        }

        public void DeployService(string name, string type)
        {
            var image = _imageMap[type];
            var port = GetServicePort(type);
            var args = _context.ServiceManager.GetServiceDeploymentArgs("docker", name);
            if (args.Length > 1)
            {
                args += " ";
            }

            _cli.Run($"run --name {name} --publish {port}:{port} --detach --rm {args}{image}");
        }

        public void UndeployService(string name)
        {
            _cli.Run($"stop {name}");
        }

        public ServiceLifecycle.State GetServiceLifecleState(string name)
        {
            var containerInfo = _cli.Run($"ps --no-trunc --filter name=^/{name}$").Split('\n');
            if (containerInfo.Length > 2)
            {
                var statusStart = containerInfo[0].IndexOf("STATUS", StringComparison.Ordinal);
                if (containerInfo[1].Substring(statusStart).StartsWith("Up "))
                {
                    var imageStart = containerInfo[0].IndexOf("IMAGE", StringComparison.Ordinal);
                    var image = new Regex(@"\S+").Match(containerInfo[1].Substring(imageStart)).ToString();
                    var svc = _serviceMap[image];
                    var port = GetServicePort(svc);
                    try
                    {
                        new TcpClient("localhost", port).Dispose();
                        return ServiceLifecycle.State.Online;
                    }
                    catch (SocketException)
                    {
                        return ServiceLifecycle.State.Starting;
                    }
                }
            }

            return ServiceLifecycle.State.Offline;
        }

        private int GetServicePort(string name)
        {
            return ServiceTypeRegistry.ForName(name).Port;
        }
    }
}
