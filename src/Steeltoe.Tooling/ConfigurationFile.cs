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

using System.IO;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents a Steeltoe Tooling project configuration file.
    /// </summary>
    public class ConfigurationFile : IConfigurationListener
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<ConfigurationFile>();

        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "steeltoe.yml";

        /// <summary>
        /// The configuration stored in this configuration file.
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Configuration file name.
        /// </summary>
        public string File { get; }

        /// <summary>
        /// Creates a new ConfigurationFile at the specified path.
        /// </summary>
        /// <param name="path">Configuration file path.</param>
        public ConfigurationFile(string path)
        {
            File = Directory.Exists(path) ? Path.Combine(path, DefaultFileName) : path;
            if (Exists())
            {
                Load();
            }
            else
            {
                Configuration = new Configuration();
            }
        }

        /// <summary>
        /// Loads this configuration file from the file system.
        /// </summary>
        public void Load()
        {
            Logger.LogDebug($"loading configuration from {File}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(File))
            {
                Configuration = deserializer.Deserialize<Configuration>(reader);
            }

            Configuration.AddListener(this);
        }

        /// <summary>
        /// Loads this configuration file to the file system.
        /// </summary>
        public void Store()
        {
            Logger.LogDebug($"storing configuration to {File}");
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(Configuration);
            using (var writer = new StreamWriter(File))
            {
                writer.Write(yaml);
            }
        }

        /// <summary>
        /// Tests if this configuration file exists on the file system.
        /// </summary>
        /// <returns>True is this configuration file exists.</returns>
        public bool Exists()
        {
            return System.IO.File.Exists(File);
        }

        /// <summary>
        /// Called when this configuration file's configuration has been changed.
        /// </summary>
        public void ConfigurationChangeEvent()
        {
            Store();
        }
    }
}
