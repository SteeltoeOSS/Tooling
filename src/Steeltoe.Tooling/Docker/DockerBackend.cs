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
    public class DockerBackend : IBackend
    {
        private const string Localhost = "127.0.0.1";

        private readonly Context _context;

        private readonly DockerCli _cli;

        internal DockerBackend(Context context)
        {
            _context = context;
            _cli = new DockerCli(_context.Shell);
        }

        public void DeployApp(string application)
        {
            throw new NotImplementedException();
        }

        public void UndeployApp(string application)
        {
            throw new NotImplementedException();
        }

        public Lifecycle.Status GetAppStatus(string application)
        {
            throw new NotImplementedException();
        }

        public void DeployService(string service)
        {
            var dockerInfo = new DockerCli(_context.Shell).Run("info");
            var dockerOs = new Regex(@"OSType:\s*(.+)", RegexOptions.Multiline).Match(dockerInfo).Groups[1].ToString();
            DeployService(service, dockerOs);
        }

        public void DeployService(string service, string os)
        {
            var svcInfo = _context.Configuration.GetServiceInfo(service);
            var port = GetPort(svcInfo.ServiceType);
            var image = LookupImage(svcInfo.ServiceType, os);
            var args = _context.Configuration.GetServiceDeploymentArgs(service, "docker");
            if (args == null)
            {
                args = "";
            }
            if (args.Length > 0)
            {
                args += " ";
            }

            _cli.Run($"run --name {service} --publish {port}:{port} --detach --rm {args}{image}");
        }

        public void UndeployService(string service)
        {
            _cli.Run($"stop {service}");
        }

        public Lifecycle.Status GetServiceStatus(string service)
        {
            var containerInfo = _cli.Run($"ps --no-trunc --filter name=^/{service}$").Split('\n');
            if (containerInfo.Length <= 2)
            {
                return Lifecycle.Status.Offline;
            }

            var statusStart = containerInfo[0].IndexOf("STATUS", StringComparison.Ordinal);
            if (!containerInfo[1].Substring(statusStart).StartsWith("Up "))
            {
                return Lifecycle.Status.Unknown;
            }

            var imageStart = containerInfo[0].IndexOf("IMAGE", StringComparison.Ordinal);
            var image = new Regex(@"\S+").Match(containerInfo[1].Substring(imageStart)).ToString();
            var svc = LookupServiceType(image);
            var port = GetPort(svc);
            try
            {
                new TcpClient(Localhost, port).Dispose();
                return Lifecycle.Status.Online;
            }
            catch (SocketException)
            {
                return Lifecycle.Status.Starting;
            }
        }

        private string LookupImage(string type, string os)
        {
            var images = _context.Target.Configuration.ServiceTypeProperties[type];
            return images.TryGetValue($"image-{os}", out var image) ? image : images["image"];
        }

        private string LookupServiceType(string image)
        {
            var svcTypes = _context.Target.Configuration.ServiceTypeProperties;
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

        private int GetPort(string serviceType)
        {
            return Registry.GetServiceTypeInfo(serviceType).Port;
        }
    }
}
