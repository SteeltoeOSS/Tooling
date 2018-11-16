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
using System.IO;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryBackend : IBackend
    {
        private readonly Context _context;

        private readonly Cli _cfCli;

        private readonly Cli _dotnetCli;

        public CloudFoundryBackend(Context context)
        {
            _context = context;
            _cfCli = new CloudFoundryCli(context.Shell);
            _dotnetCli = new Cli("dotnet", _context.Shell);
        }

        public void DeployApp(string app)
        {
            var manifestPath = Path.Combine(_context.ProjectDirectory, CloudFoundryManifestFile.DefaultFileName);
            if (File.Exists(manifestPath))
            {
                File.Delete(manifestPath);
            }

            var manifestFile = new CloudFoundryManifestFile(manifestPath);
            manifestFile.CloudFoundryManifest.Applications.Add(new CloudFoundryManifest.Application
                {
                    Name = app,
                    Command = $"cmd /c .\\{Path.GetFileName(_context.ProjectDirectory)}",
                    BuildPacks = new List<string> {"binary_buildpack"},
                    Stack = "windows2016",
                    Memory = "512M",
                    Environment = new Dictionary<string, string> {{"ASPNETCORE_ENVIRONMENT", "development"}},
                    ServiceNames = _context.Configuration.GetServices()
                }
            );
            manifestFile.Store();
            _dotnetCli.Run("publish -f netcoreapp2.1 -r win10-x64");
            _cfCli.Run(
                $"push -f {CloudFoundryManifestFile.DefaultFileName} -p bin/Debug/netcoreapp2.1/win10-x64/publish");
        }

        public void UndeployApp(string app)
        {
            _cfCli.Run($"delete {app} -f");
        }

        public Lifecycle.Status GetAppStatus(string app)
        {
            try
            {
                var appInfo = _cfCli.Run($"app {app}");
                var state = new Regex(@"^#0\s+(\S+)", RegexOptions.Multiline).Match(appInfo).Groups[1].ToString()
                    .Trim();
                switch (state)
                {
                    case "down":
                        return Lifecycle.Status.Starting;
                    case "running":
                        return Lifecycle.Status.Online;
                }
            }
            catch (CliException e)
            {
                if (e.Error.Contains($"App {app} not found"))
                {
                    return Lifecycle.Status.Offline;
                }
            }

            return Lifecycle.Status.Unknown;
        }

        public void DeployService(string service)
        {
            var svcInfo = _context.Configuration.GetServiceInfo(service);
            var cfServiceDef = _context.Target.Configuration.ServiceTypeProperties[svcInfo.ServiceType];
            _cfCli.Run($"create-service {cfServiceDef["service"]} {cfServiceDef["plan"]} {service}");
        }

        public void UndeployService(string service)
        {
            _cfCli.Run($"delete-service {service} -f");
        }

        public Lifecycle.Status GetServiceStatus(string service)
        {
            try
            {
                var serviceInfo = _cfCli.Run($"service {service}");
                var state = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline).Match(serviceInfo).Groups[1]
                    .ToString().Trim();
                switch (state)
                {
                    case "create in progress":
                        return Lifecycle.Status.Starting;
                    case "create succeeded":
                        return Lifecycle.Status.Online;
                }
            }
            catch (CliException e)
            {
                if (e.Error.Contains($"Service instance {service} not found"))
                {
                    return Lifecycle.Status.Offline;
                }
            }

            return Lifecycle.Status.Unknown;
        }
    }
}
