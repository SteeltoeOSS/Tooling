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
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.CloudFoundry;
using Steeltoe.Tooling.Docker;
using Steeltoe.Tooling.Dummy;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    public class Registry
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Registry>();

        private static RegistryConfiguration _configuration = new RegistryConfiguration();

        static Registry()
        {
            // testing service type
            if (Settings.DummiesEnabled)
            {
                Logger.LogDebug("loading dummy registry");
                var dummyCfg = new RegistryConfiguration();
                var dummySvcType = new ServiceType {Port = 0, Description = "A Dummy Service"};
                dummyCfg.ServiceTypes.Add("dummy-svc", dummySvcType);
                var dummyEnvCfg = new EnvironmentConfiguration();
                dummyEnvCfg.Description = "A Dummy Environment";
                dummyCfg.EnvironmentConfigurations.Add("dummy-env", dummyEnvCfg);
                AddRegistryConfiguration(dummyCfg);
            }

            // default service types
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "steeltoe.rc",
                "registry.yml");
            Logger.LogDebug($"loading registry from {path}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(path))
            {
                var defaultCfg = deserializer.Deserialize<RegistryConfiguration>(reader);
                AddRegistryConfiguration(defaultCfg);
            }
        }

        public static List<string> ServiceTypeNames => _configuration.ServiceTypes.Keys.ToList();

        public static ServiceType GetServiceType(string serviceTypeName)
        {
            ServiceType svcType;
            if (_configuration.ServiceTypes.TryGetValue(serviceTypeName, out svcType))
            {
                svcType.Name = serviceTypeName;
                return svcType;
            }

            return null;
        }

        public static List<string> EnvironmentNames => _configuration.EnvironmentConfigurations.Keys.ToList();

        public static Environment GetEnvironment(string environmentName)
        {
            EnvironmentConfiguration envCfg;
            if (_configuration.EnvironmentConfigurations.TryGetValue(environmentName, out envCfg))
            {
                envCfg.Name = environmentName;
                switch (envCfg.Name)
                {
                    case "cloud-foundry":
                        return new CloudFoundryEnvironment(envCfg);
                    case "docker":
                        return new DockerEnvironment(envCfg);
                    case "dummy-env":
                        return new DummyEnvironment(envCfg);
                }
            }

            throw new ToolingException($"Unknown deployment environment '{environmentName}'");
        }

        private static void AddRegistryConfiguration(RegistryConfiguration configuration)
        {
            foreach (var svcTypeEntry in configuration.ServiceTypes)
            {
                svcTypeEntry.Value.Name = null;
                _configuration.ServiceTypes[svcTypeEntry.Key] = svcTypeEntry.Value;
            }

            foreach (var envCfgEntry in configuration.EnvironmentConfigurations)
            {
                envCfgEntry.Value.Name = null;
                _configuration.EnvironmentConfigurations[envCfgEntry.Key] = envCfgEntry.Value;
            }
        }
    }
}
