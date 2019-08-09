using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.Kubernetes
{
    public class KubernetesDotnetAppDockerfileFile
    {
        private static readonly ILogger Logger =
            Logging.LoggerFactory.CreateLogger<KubernetesDotnetAppDockerfileFile>();
        
        internal string File { get; }

        internal KubernetesDotnetAppDockerfile KubernetesDotnetAppDockerfile { get; private set; }

        internal KubernetesDotnetAppDockerfileFile(string file)
        {
            File = file;
            if (Exists())
            {
                Load();
            }
            else
            {
                KubernetesDotnetAppDockerfile = new KubernetesDotnetAppDockerfile();
            }
        }

        private void Load()
        {
            Logger.LogDebug($"loading kubernetes dotnet app dockerfile from {File}");
            string text = System.IO.File.ReadAllText(File);
            KubernetesDotnetAppDockerfile = new KubernetesDotnetAppDockerfile()
            {
                BaseImage = new Regex(@"^FROM\s+(.+)", RegexOptions.Multiline)
                    .Match(text).Groups[1].ToString(),
                App = new Regex(@"^CMD dotnet /app/(.+)\.dll$", RegexOptions.Multiline)
                    .Match(text).Groups[1].ToString(),
                BuildPath = new Regex(@"^COPY (.+) /app$", RegexOptions.Multiline)
                    .Match(text).Groups[1].ToString(),
            };
        }

        internal void Store()
        {
            Logger.LogDebug($"storing kubernetes dotnet app dockerfile to {File}");
            System.IO.File.WriteAllText(File, $@"FROM {KubernetesDotnetAppDockerfile.BaseImage}
COPY {KubernetesDotnetAppDockerfile.BuildPath} /app
CMD dotnet /app/{KubernetesDotnetAppDockerfile.App}.dll
");
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }
        
    }
}
