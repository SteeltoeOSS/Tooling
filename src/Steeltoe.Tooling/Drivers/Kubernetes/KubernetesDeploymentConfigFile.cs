using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.Templaters;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    internal class KubernetesDeploymentConfigFile
    {
        private static readonly ILogger Logger =
            Logging.LoggerFactory.CreateLogger<KubernetesDeploymentConfigFile>();

        internal KubernetesDeploymentConfig KubernetesDeploymentConfig { get; } = new KubernetesDeploymentConfig();

        internal string File { get; }

        internal KubernetesDeploymentConfigFile(string file)
        {
            File = file;
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes deployment config to {File}");
            var template = TemplateManager.GetTemplate("kubernetes-deployment.yml.st");
            template.Bind("config", KubernetesDeploymentConfig);
            System.IO.File.WriteAllText(File, template.Render());
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }
    }
}
