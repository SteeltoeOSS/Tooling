using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Base
{
    public class ToolingConfiguration
    {
        public const string DefaultFileName = ".steeltoe.tooling.yml";

        public string target { get; set; }

        public Dictionary<string, Service> services { get; set; } = new Dictionary<string, Service>();

        public static ToolingConfiguration Load(string path)
        {
            var realPath = Directory.Exists(path) ? Path.Combine(path, DefaultFileName) : path;
            using (var reader = new StreamReader(realPath))
            {
                return Load(reader);
            }
        }

        public static ToolingConfiguration Load(TextReader reader)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<ToolingConfiguration>(reader);
        }

        public void Store(string path)
        {
            var realPath = Directory.Exists(path) ? Path.Combine(path, DefaultFileName) : path;
            using (var writer = new StreamWriter(realPath))
            {
                Store(writer);
            }
        }

        public void Store(TextWriter writer)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(this);
            writer.Write(yaml);
            writer.Flush();
        }

        public class Service
        {
            public string type { get; set; }
        }
    }
}
