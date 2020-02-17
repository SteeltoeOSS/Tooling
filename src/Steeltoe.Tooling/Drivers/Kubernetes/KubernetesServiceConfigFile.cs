using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.Templaters;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    internal class KubernetesServiceConfigFile
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<KubernetesServiceConfigFile>();

        internal KubernetesServiceConfig KubernetesServiceConfig { get; } = new KubernetesServiceConfig();

        internal string File { get; }

        internal KubernetesServiceConfigFile(string file)
        {
            File = file;
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes service config to {File}");
            var template = TemplateManager.GetTemplate("kubernetes-service.yml.st");
            template.Bind("config", KubernetesServiceConfig);
            System.IO.File.WriteAllText(File, template.Render());
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }
    }
}
