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
using System.Linq;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// Represents a configuration for a Steeltoe Tooling project.
    /// </summary>
    public class Configuration
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Configuration>();

        /// <summary>
        /// Project applications.
        /// </summary>
        [YamlMember(Alias = "apps")]
        public SortedDictionary<string, App> Apps { get; set; } = new SortedDictionary<string, App>();

        /// <summary>
        /// Project application services.
        /// </summary>
        [YamlMember(Alias = "services")]
        public SortedDictionary<string, Service> Services { get; set; } = new SortedDictionary<string, Service>();

        private readonly List<IConfigurationListener> _listeners = new List<IConfigurationListener>();

        /// <summary>
        /// Adds the named application to this configuration.
        /// </summary>
        /// <param name="app">Name of application to be added.</param>
        /// <param name="framework">Target framework.</param>
        /// <param name="runtime">Target runtime.</param>
        /// <exception cref="ItemExistsException">Thrown if an application of the same name has already been added to this configuration.</exception>
        public void AddApp(string app, string framework, string runtime)
        {
            Logger.LogDebug($"adding app {app} ({framework}/{runtime})");
            if (Apps.ContainsKey(app))
            {
                throw new ItemExistsException(app, "app");
            }

            Apps[app] = new App();
            Apps[app].TargetFramework = framework;
            Apps[app].TargetRuntime = runtime;
            NotifyChanged();
        }

        /// <summary>
        /// Removes the named application from the configuration.
        /// </summary>
        /// <param name="app">Name of application to be removed.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public void RemoveApp(string app)
        {
            Logger.LogDebug($"removing app {app}");
            if (!Apps.Remove(app))
            {
                throw new ItemDoesNotExistException(app);
            }

            NotifyChanged();
        }

        /// <summary>
        /// Returns this configuration's applications.
        /// </summary>
        /// <returns>This configuration's applications.</returns>
        public List<string> GetApps()
        {
            return Apps.Keys.ToList();
        }

        /// <summary>
        /// Returns the named application.
        /// </summary>
        /// <param name="app">Application name</param>
        /// <returns>Named application.</returns>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public AppInfo GetAppInfo(string app)
        {
            if (Apps.ContainsKey(app))
            {
                return new AppInfo(app);
            }
            else
            {
                throw new ItemDoesNotExistException(app);
            }
        }

        /// <summary>
        /// Sets the named application's arguments.
        /// </summary>
        /// <param name="app">Application name.</param>
        /// <param name="args">Application arguments.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public void SetAppArgs(string app, string args)
        {
            Logger.LogDebug($"setting app '{app}' args to '{args}'");
            if (!Apps.ContainsKey(app))
            {
                throw new ItemDoesNotExistException(app);
            }

            Apps[app].Args = args;
            NotifyChanged();
        }

        /// <summary>
        /// Sets the named application's deployment arguments.
        /// </summary>
        /// <param name="app">Application name.</param>
        /// <param name="target">Deployment target.</param>
        /// <param name="args">Application arguments.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public void SetAppArgs(string app, string target, string args)
        {
            Logger.LogDebug($"setting app '{app}' args for target '{target}' to '{args}'");
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target);
            }

            if (!Apps.ContainsKey(app))
            {
                throw new ItemDoesNotExistException(app);
            }

            Apps[app].DeployArgs[target] = args;
            NotifyChanged();
        }

        /// <summary>
        /// Gets the named application's arguments.
        /// </summary>
        /// <param name="app">Application name.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public string GetAppArgs(string app)
        {
            try
            {
                return Apps[app].Args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(app);
            }
        }

        /// <summary>
        /// Gets the named application's deployment arguments.
        /// </summary>
        /// <param name="app">Application name.</param>
        /// <param name="target">Deployment target.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application does not exist in this configuration.</exception>
        public string GetAppArgs(string app, string target)
        {
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target);
            }

            try
            {
                Apps[app].DeployArgs.TryGetValue(target, out var args);
                return args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(app);
            }
        }

        /// <summary>
        /// Adds the named application service to this configuration.
        /// </summary>
        /// <param name="service">Name of application service to be added.</param>
        /// <param name="serviceType">Name of application service type.</param>
        /// <exception cref="ItemExistsException">Thrown if an application service of the same name has already been added to this configuration.</exception>
        public void AddService(string service, string serviceType)
        {
            Logger.LogDebug($"adding service {serviceType} '{service}'");
            if (!Registry.GetServiceTypes().Contains(serviceType))
            {
                throw new ItemDoesNotExistException(serviceType);
            }

            if (Services.ContainsKey(service))
            {
                throw new ItemExistsException(service, "service");
            }

            Services[service] = new Service {ServiceTypeName = serviceType};
            NotifyChanged();
        }

        /// <summary>
        /// Removes the named application service from the configuration.
        /// </summary>
        /// <param name="service">Name of application service to be removed.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public void RemoveService(string service)
        {
            Logger.LogDebug($"removing service {service}");
            if (!Services.Remove(service))
            {
                throw new ItemDoesNotExistException(service);
            }

            NotifyChanged();
        }

        /// <summary>
        /// Returns this configuration's application services.
        /// </summary>
        /// <returns>This configuration's applications services.</returns>
        public List<string> GetServices()
        {
            return Services.Keys.ToList();
        }

        /// <summary>
        /// Returns the named application service.
        /// </summary>
        /// <param name="service">Application service name</param>
        /// <returns>Named application service.</returns>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public ServiceInfo GetServiceInfo(string service)
        {
            try
            {
                return new ServiceInfo(service, Services[service].ServiceTypeName);
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(service);
            }
        }

        /// <summary>
        /// Sets the named application service's arguments.
        /// </summary>
        /// <param name="service">Application service name.</param>
        /// <param name="args">Application service arguments.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public void SetServiceArgs(string service, string args)
        {
            Logger.LogDebug($"setting service '{service}' args to '{args}'");
            if (!Services.ContainsKey(service))
            {
                throw new ItemDoesNotExistException(service);
            }

            Services[service].Args = args;
            NotifyChanged();
        }

        /// <summary>
        /// Sets the named application services's deployment arguments.
        /// </summary>
        /// <param name="service">Application service name.</param>
        /// <param name="target">Deployment target.</param>
        /// <param name="args">Application service arguments.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public void SetServiceArgs(string service, string target, string args)
        {
            Logger.LogDebug($"setting service '{service}' args for target '{target} to '{args}'");
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target);
            }

            if (!Services.ContainsKey(service))
            {
                throw new ItemDoesNotExistException(service);
            }

            Services[service].DeployArgs[target] = args;
            NotifyChanged();
        }

        /// <summary>
        /// Gets the named application service's arguments.
        /// </summary>
        /// <param name="service">Application service name.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public string GetServiceArgs(string service)
        {
            try
            {
                return Services[service].Args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(service);
            }
        }

        /// <summary>
        /// Gets the named application service's deployment arguments.
        /// </summary>
        /// <param name="service">Application service name.</param>
        /// <param name="target">Deployment target.</param>
        /// <exception cref="ItemDoesNotExistException">Thrown if the named application service does not exist in this configuration.</exception>
        public string GetServiceArgs(string service, string target)
        {
            if (!Registry.Targets.Contains(target))
            {
                throw new ItemDoesNotExistException(target);
            }

            try
            {
                Services[service].DeployArgs.TryGetValue(target, out var args);
                return args;
            }
            catch (KeyNotFoundException)
            {
                throw new ItemDoesNotExistException(service);
            }
        }

        /// <summary>
        /// Adds a listener for configuration changes,
        /// </summary>
        /// <param name="listener">Listener to be notified when configuration changes.</param>
        public void AddListener(IConfigurationListener listener)
        {
            _listeners.Add(listener);
        }

        /// <summary>
        /// Notify all listeners that this configuration has changed.
        /// </summary>
        public void NotifyChanged()
        {
            Logger.LogDebug("configuration changed");
            foreach (var listener in _listeners)
            {
                listener.ConfigurationChangeEvent();
            }
        }

        /// <summary>
        /// Represents an application in this configuration.
        /// </summary>
        public class App
        {
            /// <summary>
            /// Application target framework.
            /// </summary>
            [YamlMember(Alias = "targetFramework")]
            public string TargetFramework { get; set; }

            /// <summary>
            /// Application target runtime.
            /// </summary>
            [YamlMember(Alias = "targetRuntime")]
            public string TargetRuntime { get; set; }

            /// <summary>
            /// Application arguments.
            /// </summary>
            [YamlMember(Alias = "args")]
            public string Args { get; set; }

            /// <summary>
            /// Application deployment target arguments.
            /// </summary>
            [YamlMember(Alias = "deployArgs")]
            public SortedDictionary<string, string> DeployArgs { get; set; } = new SortedDictionary<string, string>();
        }

        /// <summary>
        /// Represents an application service in this configuration.
        /// </summary>
        public class Service : App
        {
            /// <summary>
            /// Application service type.
            /// </summary>
            [YamlMember(Alias = "type")]
            public string ServiceTypeName { get; set; }
        }
    }
}
