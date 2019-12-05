// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Drivers.CloudFoundry
{
    internal class CloudFoundryDriver : IDriver
    {
        private readonly Context _context;

        private readonly Cli _cfCli;

        private readonly Cli _dotnetCli;

        internal CloudFoundryDriver(Context context)
        {
            _context = context;
            _cfCli = new CloudFoundryCli(context.Shell);
            _dotnetCli = new Cli("dotnet", _context.Shell);
        }

        public void DeploySetup()
        {
        }

        public void DeployTeardown()
        {
        }

        public void DeployApp(string app)
        {
            var manifestPath = Path.Combine(_context.ProjectDirectory, CloudFoundryManifestFile.DefaultFileName);
            if (File.Exists(manifestPath))
            {
                File.Delete(manifestPath);
            }

            var cloudFoundryApp = new CloudFoundryManifest.Application();
            cloudFoundryApp.Name = app;
            cloudFoundryApp.Memory = "512M";
            cloudFoundryApp.Environment = new Dictionary<string, string> {{"ASPNETCORE_ENVIRONMENT", "development"}};
            cloudFoundryApp.ServiceNames = _context.Configuration.GetServices();
            if (_context.Configuration.Apps[app].TargetRuntime.StartsWith("ubuntu"))
            {
                cloudFoundryApp.BuildPacks = new List<string> {"dotnet_core_buildpack"};
                cloudFoundryApp.Command = $"cd ${{HOME}} && ./{Path.GetFileName(_context.ProjectDirectory)}";
            }
            else
            {
                cloudFoundryApp.Stack = "windows";
                cloudFoundryApp.BuildPacks = new List<string> {"hwc_buildpack"};
                cloudFoundryApp.Command = $"cmd /c .\\{Path.GetFileName(_context.ProjectDirectory)}";
            }
            var manifestFile = new CloudFoundryManifestFile(manifestPath);
            manifestFile.CloudFoundryManifest.Applications.Add(cloudFoundryApp);
            manifestFile.Store();
            var framework = _context.Configuration.Apps[app].TargetFramework;
            var runtime = _context.Configuration.Apps[app].TargetRuntime;
            _dotnetCli.Run($"publish -f {framework} -r {runtime}", "publishing app");
            _cfCli.Run(
                $"push -f {CloudFoundryManifestFile.DefaultFileName} -p bin/Debug/{framework}/{runtime}/publish",
                "pushing app to Cloud Foundry");
        }

        public void UndeployApp(string app)
        {
            _cfCli.Run($"delete {app} -f", $"deleting Cloud Foundry app");
        }

        public Lifecycle.Status GetAppStatus(string app)
        {
            try
            {
                var appInfo = _cfCli.Run($"app {app}", "getting details for Cloud Foundry app");
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

                if (e.Error.Contains($"App '{app}' not found"))
                {
                    return Lifecycle.Status.Offline;
                }
            }

            return Lifecycle.Status.Unknown;
        }

        public void DeployService(string service)
        {
            var svcInfo = _context.Configuration.GetServiceInfo(service);
            _context.Target.Configuration.ServiceTypeProperties.TryGetValue(svcInfo.ServiceType, out var cfServiceDef);
            if (cfServiceDef == null)
            {
                throw new ToolingException($"No Cloud Foundry service available for '{svcInfo.Service}' [{svcInfo.ServiceType}]");
            }

            var cfCmd = $"create-service {cfServiceDef["service"]} {cfServiceDef["plan"]} {service}";
            var svcArgs = _context.Configuration.GetServiceArgs(service, "cloud-foundry") ?? "";
            if (svcArgs.Length > 0)
            {
                svcArgs = svcArgs.Replace("\"", "\"\"\"");
                cfCmd += $" {svcArgs}";
            }

            _cfCli.Run(cfCmd, "creating Cloud Foundry service");
        }

        public void UndeployService(string service)
        {
            _cfCli.Run($"delete-service {service} -f", "deleting Cloud Foundry service");
        }

        public Lifecycle.Status GetServiceStatus(string service)
        {
            try
            {
                var serviceInfo = _cfCli.Run($"service {service}", "getting details for Cloud Foundry service");
                var state = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline).Match(serviceInfo).Groups[1]
                    .ToString().Trim();
                switch (state)
                {
                    case "create in progress":
                        return Lifecycle.Status.Starting;
                    case "create succeeded":
                        return Lifecycle.Status.Online;
                    case "delete in progress":
                        return Lifecycle.Status.Stopping;
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
