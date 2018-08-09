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
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryConfiguration
    {
        public Application[] applications { get; set; }

        public static CloudFoundryConfiguration load(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return load(reader);
            }
        }

        public static CloudFoundryConfiguration load(StreamReader reader)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<CloudFoundryConfiguration>(reader);
        }

        public void store(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                store(writer);
            }
        }

        public void store(StreamWriter writer)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(this);
            writer.Write(yaml);
        }
    }

    public class Application
    {
        public string name { get; set; }
        public string buildpack { get; set; } = "dotnet_core_buildpack";

        public Dictionary<string, string> env { get; set; } = new Dictionary<string, string>()
        {
            {"ASPNETCORE_ENVIRONMENT", "development"}
        };

        public string[] services { get; set; }
    }
}
