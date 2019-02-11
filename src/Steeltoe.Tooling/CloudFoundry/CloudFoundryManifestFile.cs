using System.IO;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.CloudFoundry
{
    internal class CloudFoundryManifestFile
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<CloudFoundryManifestFile>();

        internal const string DefaultFileName = "manifest-steeltoe.yml";

        internal CloudFoundryManifest CloudFoundryManifest { get; private set; }

        internal string File { get; }

        internal CloudFoundryManifestFile(string file)
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

        private void Load()
        {
            Logger.LogDebug($"loading cloud foundry manifest from {File}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(File))
            {
                CloudFoundryManifest = deserializer.Deserialize<CloudFoundryManifest>(reader);
            }
        }

        internal void Store()
        {
            Logger.LogDebug($"storing cloud foundry manifest to {File}");
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(CloudFoundryManifest);
            using (var writer = new StreamWriter(File))
            {
                writer.Write(yaml);
            }
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }
    }
}
