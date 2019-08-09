using System.IO;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Kubernetes
{
    internal class KubernetesDeploymentConfigFile
    {
        private static readonly ILogger Logger =
            Logging.LoggerFactory.CreateLogger<KubernetesDeploymentConfigFile>();

        internal KubernetesDeploymentConfig KubernetesDeploymentConfig { get; private set; }

        internal string File { get; }

        internal KubernetesDeploymentConfigFile(string file)
        {
            File = file;
            if (Exists())
            {
                Load();
            }
            else
            {
                KubernetesDeploymentConfig = new KubernetesDeploymentConfig();
            }
        }

        private void Load()
        {
            Logger.LogDebug($"loading kubernetes deployment config from {File}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(File))
            {
                KubernetesDeploymentConfig = deserializer.Deserialize<KubernetesDeploymentConfig>(reader);
            }
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes deployment config to {File}");
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(KubernetesDeploymentConfig);
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
