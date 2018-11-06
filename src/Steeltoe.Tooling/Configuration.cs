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
using System.Linq;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    public class Configuration
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Configuration>();

        [YamlMember(Alias = "target")] public string Target { get; set; }

        [YamlMember(Alias = "apps")]
        public SortedDictionary<string, App> Apps { get; set; } = new SortedDictionary<string, App>();

        [YamlMember(Alias = "services")]
        public SortedDictionary<string, Service> Services { get; set; } = new SortedDictionary<string, Service>();

        private readonly List<IConfigurationListener> _listeners = new List<IConfigurationListener>();

        public void AddApp(string app)
        {
            Logger.LogDebug($"adding app {app}");
            if (Apps.ContainsKey(app))
            {
                throw new ToolingException($"App '{app}' already exists");
            }

            Apps[app] = new App();
            NotifyChanged();
        }

        public void RemoveApp(string app)
        {
            Logger.LogDebug($"removing app {app}");
            if (!Apps.Remove(app))
            {
                throw new NotFoundException(app, "app");
            }

            NotifyChanged();
        }

        public List<string> GetApps()
        {
            return Apps.Keys.ToList();
        }

        public AppInfo GetAppInfo(string app)
        {
            try
            {
                return new AppInfo(app);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException(app, "app");
            }
        }

        public void AddService(string service, string serviceType)
        {
            Logger.LogDebug($"adding service {serviceType} '{service}'");
            if (!Registry.GetServiceTypes().Contains(serviceType))
            {
                throw new NotFoundException(serviceType, "type");
            }

            if (Services.ContainsKey(service))
            {
                throw new ToolingException($"Service '{service}' already exists");
            }

            Services[service] = new Service {ServiceTypeName = serviceType};
            NotifyChanged();
        }

        public void RemoveService(string service)
        {
            Logger.LogDebug($"removing service {service}");
            if (!Services.Remove(service))
            {
                throw new NotFoundException(service, "service");
            }

            NotifyChanged();
        }

        public List<string> GetServices()
        {
            return Services.Keys.ToList();
        }

        public ServiceInfo GetServiceInfo(string service)
        {
            try
            {
                return new ServiceInfo(service, Services[service].ServiceTypeName);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException(service, "service");
            }
        }

        public void SetServiceDeploymentArgs(string service, string target, string args)
        {
            Logger.LogDebug($"setting service '{service}' args for target '{target} to '{args}'");
            if (!Registry.Targets.Contains(target))
            {
                throw new NotFoundException(target, "target");
            }

            if (!Services.ContainsKey(service))
            {
                throw new NotFoundException(service, "service");
            }

            Services[service].Args[target] = args;
            NotifyChanged();
        }

        public string GetServiceDeploymentArgs(string service, string target)
        {
            if (!Registry.Targets.Contains(target))
            {
                throw new NotFoundException(target, "target");
            }

            try
            {
                var svc = Services[service];
                string args;
                svc.Args.TryGetValue(target, out args);
                return args;
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException(service, "service");
            }
        }

        public void AddListener(IConfigurationListener listener)
        {
            _listeners.Add(listener);
        }

        public void NotifyChanged()
        {
            Logger.LogDebug("configuration changed");
            foreach (var listener in _listeners)
            {
                listener.ConfigurationChangeEvent();
            }
        }

        public class App
        {
        }

        public class Service
        {
            [YamlMember(Alias = "type")] public string ServiceTypeName { get; set; }

            [YamlMember(Alias = "args")]
            public SortedDictionary<string, string> Args { get; set; } = new SortedDictionary<string, string>();
        }
    }
}
