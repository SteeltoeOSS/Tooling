using System.IO;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    internal class KubernetesServiceConfigFile
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<KubernetesServiceConfigFile>();

        internal KubernetesServiceConfig KubernetesServiceConfig { get; private set; }

        internal string File { get; }

        internal KubernetesServiceConfigFile(string file)
        {
            File = file;
            if (Exists())
            {
                Load();
            }
            else
            {
                KubernetesServiceConfig = new KubernetesServiceConfig();
            }
        }

        private void Load()
        {
            Logger.LogDebug($"loading kubernetes service config from {File}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(File))
            {
                KubernetesServiceConfig = deserializer.Deserialize<KubernetesServiceConfig>(reader);
            }
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes service config to {File}");
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(KubernetesServiceConfig);
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
