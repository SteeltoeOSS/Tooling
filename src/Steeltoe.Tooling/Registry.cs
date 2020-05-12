// Copyright 2020 the original author or authors.
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
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.Models;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Tooling registry.
    /// </summary>
    public class Registry
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Registry>();

        /// <summary>
        /// Docker images for dotnet SDKs.
        /// </summary>
        public Dictionary<string, string> DotnetImages { get; private set; }

        /// <summary>
        /// Services.
        /// </summary>
        public List<ServiceSpecification> ServiceSpecifications { get; private set; }

        /// <summary>
        /// Load the registry from the specified directory.
        /// </summary>
        /// <param name="directory"></param>
        public void Load(string directory)
        {
            Logger.LogDebug($"loading registry: {directory}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(Path.Join(directory, "dotnet.yml")))
            {
                DotnetImages = deserializer.Deserialize<Dictionary<string, string>>(reader);
            }

            using (var reader = new StreamReader(Path.Join(directory, "dependencies.yml")))
            {
                ServiceSpecifications = deserializer.Deserialize<List<ServiceSpecification>>(reader);
            }
        }
    }
}
