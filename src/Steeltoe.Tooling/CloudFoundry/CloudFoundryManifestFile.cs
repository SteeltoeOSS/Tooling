using System.IO;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryManifestFile
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<CloudFoundryManifestFile>();

        public CloudFoundryManifest CloudFoundryManifest { get; private set; }

        public string File { get; }

        public CloudFoundryManifestFile(string file)
        {
            File = file;
            if (Exists())
            {
                Load();
            }
            else
            {
                CloudFoundryManifest = new CloudFoundryManifest();
            }
        }

        public void Load()
        {
            Logger.LogDebug($"loading cloud foundry manifest from {File}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(File))
            {
                CloudFoundryManifest = deserializer.Deserialize<CloudFoundryManifest>(reader);
            }
        }

        public void Store()
        {
            Logger.LogDebug($"storing cloud foundry manifest to {File}");
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(CloudFoundryManifest);
            using (var writer = new StreamWriter(File))
            {
                writer.Write(yaml);
            }
        }

        public bool Exists()
        {
            return System.IO.File.Exists(File);
        }
    }
}
