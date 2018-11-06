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
    public class ConfigurationFile : IConfigurationListener
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<ConfigurationFile>();

        public const string DefaultFileName = "steeltoe.yml";

        public Configuration Configuration { get; private set; }

        public string File { get; }

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

        public bool Exists()
        {
            return System.IO.File.Exists(File);
        }

        public void ConfigurationChangeEvent()
        {
            Store();
        }
    }
}
