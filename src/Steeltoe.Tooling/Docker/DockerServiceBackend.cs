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
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Docker
{
    public class DockerServiceBackend : IServiceBackend
    {
        private const string Localhost = "127.0.0.1";

        private readonly Context _context;

        private readonly DockerCli _cli;

        internal DockerServiceBackend(Context context)
        {
            _context = context;
            _cli = new DockerCli(_context.Shell);
        }

        public void DeployService(string name, string type)
        {
            var image = LookupImage(type);
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
            if (containerInfo.Length <= 2)
            {
                return ServiceLifecycle.State.Offline;
            }

            var statusStart = containerInfo[0].IndexOf("STATUS", StringComparison.Ordinal);
            if (!containerInfo[1].Substring(statusStart).StartsWith("Up "))
            {
                return ServiceLifecycle.State.Unknown;
            }

            var imageStart = containerInfo[0].IndexOf("IMAGE", StringComparison.Ordinal);
            var image = new Regex(@"\S+").Match(containerInfo[1].Substring(imageStart)).ToString();
            var svc = LookupServiceType(image);
            var port = GetServicePort(svc);
            try
            {
                new TcpClient(Localhost, port).Dispose();
                return ServiceLifecycle.State.Online;
            }
            catch (SocketException)
            {
                return ServiceLifecycle.State.Starting;
            }
        }

        private string LookupImage(string type)
        {
            var dockerInfo = new DockerCli(_context.Shell).Run("info");
            var dockerOs = new Regex(@"OSType:\s*(.+)", RegexOptions.Multiline).Match(dockerInfo).Groups[1].ToString();
            var images = _context.Environment.Configuration.ServiceTypes[type];
            if (images.TryGetValue($"image-{dockerOs}", out var image))
            {
                return image;
            }

            return images["image"];
        }

        private string LookupServiceType(string image)
        {
            var svcTypes = _context.Environment.Configuration.ServiceTypes;
            foreach (var svcType in svcTypes.Keys)
            {
                var images = svcTypes[svcType];
                if (images.Values.Contains(image))
                {
                    return svcType;
                }
            }

            throw new ToolingException($"Failed to lookup service type for image '{image}'");
        }

        private int GetServicePort(string name)
        {
            return Registry.GetServiceType(name).Port;
        }
    }
}
