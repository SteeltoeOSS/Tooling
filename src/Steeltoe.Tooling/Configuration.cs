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

        private string _target;

        [YamlMember(Alias = "target")]
        public string Target
        {
            get => _target;
            set
            {
                _target = value;
                NotifyChanged();
            }
        }

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
                throw new ItemExistsException(app, "app");
            }

            Apps[app] = new App();
            NotifyChanged();
        }

        public void RemoveApp(string app)
        {
            Logger.LogDebug($"removing app {app}");
            if (!Apps.Remove(app))
            {
                throw new ItemDoesNotExistException(app, "app");
            }

            NotifyChanged();
        }

        public List<string> GetApps()
        {
            return Apps.Keys.ToList();
        }

        public AppInfo GetAppInfo(string app)
        {
            if (Apps.ContainsKey(app))
            {
                return new AppInfo(app);
            }
            else
            {
                throw new ItemDoesNotExistException(app, "app");
            }
        }

        public void SetAppArgs(string app, string args)
        {
            Logger.LogDebug($"setting app '{app}' args to '{args}'");
            if (!Apps.ContainsKey(app))
            {
                throw new ItemDoesNotExistException(app, "app");
            }

            Apps[app].Args = args;
            NotifyChanged();
        }

        public void SetAppArgs(string app, string target, string args)
        {
            Logger.LogDebug($"setting app '{app}' args for target '{target}' to '{args}'");
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target, "target");
            }

            if (!Apps.ContainsKey(app))
            {
                throw new ItemDoesNotExistException(app, "app");
            }

            Apps[app].DeployArgs[target] = args;
            NotifyChanged();
        }

        public string GetAppArgs(string app)
        {
            try
            {
                return Apps[app].Args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(app, "app");
            }
        }

        public string GetAppArgs(string app, string target)
        {
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target, "target");
            }

            try
            {
                Apps[app].DeployArgs.TryGetValue(target, out var args);
                return args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(app, "app");
            }
        }

        public void AddService(string service, string serviceType)
        {
            Logger.LogDebug($"adding service {serviceType} '{service}'");
            if (!Registry.GetServiceTypes().Contains(serviceType))
            {
                throw new ItemDoesNotExistException(serviceType, "service type");
            }

            if (Services.ContainsKey(service))
            {
                throw new ItemExistsException(service, "service");
            }

            Services[service] = new Service {ServiceTypeName = serviceType};
            NotifyChanged();
        }

        public void RemoveService(string service)
        {
            Logger.LogDebug($"removing service {service}");
            if (!Services.Remove(service))
            {
                throw new ItemDoesNotExistException(service, "service");
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
                throw new ItemDoesNotExistException(service, "service");
            }
        }

        public void SetServiceArgs(string service, string args)
        {
            Logger.LogDebug($"setting service '{service}' args to '{args}'");
            if (!Services.ContainsKey(service))
            {
                throw new ItemDoesNotExistException(service, "service");
            }

            Services[service].Args = args;
            NotifyChanged();
        }

        public void SetServiceArgs(string service, string target, string args)
        {
            Logger.LogDebug($"setting service '{service}' args for target '{target} to '{args}'");
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target, "target");
            }

            if (!Services.ContainsKey(service))
            {
                throw new ItemDoesNotExistException(service, "service");
            }

            Services[service].DeployArgs[target] = args;
            NotifyChanged();
        }

        public string GetServiceArgs(string service)
        {
            try
            {
                return Services[service].Args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(service, "service");
            }
        }

        public string GetServiceArgs(string service, string target)
        {
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target, "target");
            }

            try
            {
                Services[service].DeployArgs.TryGetValue(target, out var args);
                return args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(service, "service");
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
            [YamlMember(Alias = "args")]
            public string Args { get; set; }

            [YamlMember(Alias = "deployArgs")]
            public SortedDictionary<string, string> DeployArgs { get; set; } = new SortedDictionary<string, string>();
        }

        public class Service : App
        {
            [YamlMember(Alias = "type")] public string ServiceTypeName { get; set; }
        }
    }
}
