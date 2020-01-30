using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Drivers.CloudFoundry
{
    internal class CloudFoundryManifestFile
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<CloudFoundryManifestFile>();

        internal const string DefaultFileName = "manifest-steeltoe.yaml";

        public CloudFoundryManifest CloudFoundryManifest { get; } = new CloudFoundryManifest();

        public string File { get; }

        internal CloudFoundryManifestFile(string file)
        {
            File = file;
        }

        internal void Store()
        {
            Logger.LogDebug($"storing cloud foundry manifest to {File}");
            var template = TemplateManager.GetTemplate("cloud-foundry-manifest.yml.st");
            template.Bind("manifest", new CloudFoundryManifestAdapter(CloudFoundryManifest));
            System.IO.File.WriteAllText(File, template.Render());
        }

        internal bool Exists()
        {
            return System.IO.File.Exists(File);
        }

        internal class CloudFoundryManifestAdapter
        {
            private readonly CloudFoundryManifest _manifest;

            public CloudFoundryManifest.Application App { get; }

            public bool EnvironmentExists => App.Environment.Count > 0;

            internal CloudFoundryManifestAdapter(CloudFoundryManifest manifest)
            {
                _manifest = manifest;
                if (_manifest.Applications.Count > 0)
                {
                    App = _manifest.Applications[0];
                }
            }
        }
    }
}
