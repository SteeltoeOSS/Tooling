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
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents a Steeltoe Tooling projects available application service types and deployment targets.
    /// </summary>
    public class Registry
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Registry>();

        /// <summary>
        /// Represents the Registry's configuration.
        /// </summary>
        public class Configuration
        {
            /// <summary>
            /// Available application service types.
            /// </summary>
            [YamlMember(Alias = "serviceTypes")]
            public Dictionary<string, ServiceTypeInfo> ServiceTypes { get; set; } =
                new Dictionary<string, ServiceTypeInfo>();

            /// <summary>
            /// Available deployment targets.
            /// </summary>
            [YamlMember(Alias = "targets")]
            public Dictionary<string, TargetConfiguration> TargetConfigurations { get; set; } =
                new Dictionary<string, TargetConfiguration>();

            /// <summary>
            /// Defines a new application service type.
            /// </summary>
            /// <param name="name">Application service type name.</param>
            /// <param name="port">Application service port.</param>
            /// <param name="description">Application service description.</param>
            public void DefineServiceType(string name, int port, string description)
            {
                ServiceTypes[name] = new ServiceTypeInfo {Port = port, Description = description};
            }

            /// <summary>
            /// Defines a new deployment target.
            /// </summary>
            /// <param name="target">Deployment target name.</param>
            /// <param name="description">Deployment target description.</param>
            public void DefineTarget(string target, string description)
            {
                TargetConfigurations[target] = new TargetConfiguration {Description = description};
            }

            /// <summary>
            /// Defines a new application service type property for a deployment target.
            /// </summary>
            /// <param name="target">Deployment target name.</param>
            /// <param name="serviceType">Application service type name.</param>
            /// <param name="propertyName">Application service type property name.</param>
            /// <param name="propertyValue">Application service type property value.</param>
            public void DefineTargetServiceTypeProperty(
                string target,
                string serviceType,
                string propertyName,
                string propertyValue)
            {
                if (!TargetConfigurations.ContainsKey(target))
                {
                    TargetConfigurations[target] = new TargetConfiguration();
                }

                var targetCfg = TargetConfigurations[target];
                if (!targetCfg.ServiceTypeProperties.ContainsKey(serviceType))
                {
                    targetCfg.ServiceTypeProperties[serviceType] = new Dictionary<string, string>();
                }

                targetCfg.ServiceTypeProperties[serviceType][propertyName] = propertyValue;
                TargetConfigurations[target] = targetCfg;
            }
        }

        private static Configuration _configuration = new Configuration();

        /// <summary>
        /// Returns available deployment targets.
        /// </summary>
        public static List<string> Targets => _configuration.TargetConfigurations.Keys.ToList();

        static Registry()
        {
            // testing service type
            if (Settings.DummiesEnabled)
            {
                Logger.LogDebug("loading dummy registry");
                var dummyCfg = new Configuration();
                var dummySvcType = new ServiceTypeInfo {Port = 0, Description = "A Dummy Service"};
                dummyCfg.ServiceTypes.Add("dummy-svc", dummySvcType);
                var dummyTarget = new TargetConfiguration();
                dummyTarget.Description = "A Dummy Target";
                dummyCfg.TargetConfigurations.Add("dummy-target", dummyTarget);
                AddRegistryConfiguration(dummyCfg);
            }

            // default service types
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "steeltoe.rc",
                "registry.yaml");
            Logger.LogDebug($"loading registry from {path}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(path))
            {
                var defaultCfg = deserializer.Deserialize<Configuration>(reader);
                AddRegistryConfiguration(defaultCfg);
            }
        }

        /// <summary>
        /// Returns available application service types.
        /// </summary>
        /// <returns>Available application service types.</returns>
        public static List<string> GetServiceTypes()
        {
            return _configuration.ServiceTypes.Keys.ToList();
        }

        /// <summary>
        /// Returns the application service type information for the named type.
        /// </summary>
        /// <param name="serviceType">Application service type name.</param>
        /// <returns></returns>
        public static ServiceTypeInfo GetServiceTypeInfo(string serviceType)
        {
            ServiceTypeInfo svcTypeInfo;
            if (_configuration.ServiceTypes.TryGetValue(serviceType, out svcTypeInfo))
            {
                svcTypeInfo.Name = serviceType;
                return svcTypeInfo;
            }

            return null;
        }

        private static void AddRegistryConfiguration(Configuration configuration)
        {
            foreach (var svcTypeEntry in configuration.ServiceTypes)
            {
                svcTypeEntry.Value.Name = null;
                _configuration.ServiceTypes[svcTypeEntry.Key] = svcTypeEntry.Value;
            }

            foreach (var targetCfgEntry in configuration.TargetConfigurations)
            {
                targetCfgEntry.Value.Name = null;
                _configuration.TargetConfigurations[targetCfgEntry.Key] = targetCfgEntry.Value;
            }
        }
    }
}
