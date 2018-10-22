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
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    public class RegistryConfiguration
    {
        [YamlMember(Alias = "serviceTypes")]
        public SortedDictionary<string, ServiceType> ServiceTypes { get; set; } =
            new SortedDictionary<string, ServiceType>();

        [YamlMember(Alias = "environments")]
        public SortedDictionary<string, EnvironmentConfiguration> EnvironmentConfigurations { get; set; } =
            new SortedDictionary<string, EnvironmentConfiguration>();

        public void DefineServiceType(string name, int port, string description)
        {
            ServiceTypes[name] = new ServiceType {Port = port, Description = description};
        }

        public void DefineEnvironment(string environmentName, string description)
        {
            var envCfg = new EnvironmentConfiguration {Description = description};
            EnvironmentConfigurations[environmentName] = envCfg;
        }

        public void DefineEnvironmentServiceTypeProperty(
            string environmentName,
            string serviceTypeName,
            string propertyName,
            string propertyValue)
        {
            if (!EnvironmentConfigurations.ContainsKey(environmentName))
            {
                EnvironmentConfigurations[environmentName] = new EnvironmentConfiguration();
            }

            var envCfg = EnvironmentConfigurations[environmentName];
            if (!envCfg.ServiceTypeProperties.ContainsKey(serviceTypeName))
            {
                envCfg.ServiceTypeProperties[serviceTypeName] = new Dictionary<string, string>();
            }

            envCfg.ServiceTypeProperties[serviceTypeName][propertyName] = propertyValue;
            EnvironmentConfigurations[environmentName] = envCfg;
        }
    }
}
