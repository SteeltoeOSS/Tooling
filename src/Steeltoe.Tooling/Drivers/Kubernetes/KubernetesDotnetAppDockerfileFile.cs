using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    internal class KubernetesDotnetAppDockerfileFile
    {
        private static readonly ILogger Logger =
            Logging.LoggerFactory.CreateLogger<KubernetesDotnetAppDockerfileFile>();

        internal string File { get; }

        internal KubernetesDotnetAppDockerfile KubernetesDotnetAppDockerfile { get; } =
            new KubernetesDotnetAppDockerfile();

        internal KubernetesDotnetAppDockerfileFile(string file)
        {
            File = file;
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes dotnet app dockerfile to {File}");
            var template = TemplateManager.GetTemplate("kubernetes-dockerfile.st");
            template.Bind("dockerfile", KubernetesDotnetAppDockerfile);
            System.IO.File.WriteAllText(File, template.Render());
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }
    }
}
