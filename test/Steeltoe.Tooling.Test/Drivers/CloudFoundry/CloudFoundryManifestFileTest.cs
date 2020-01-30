using System.Collections.Generic;
using System.IO;
using Shouldly;
using Steeltoe.Tooling.Drivers.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.CloudFoundry
{
    public class CloudFoundryManifestFileTest : ToolingTest
    {
        private readonly string _configFile;

        public CloudFoundryManifestFileTest()
        {
            _configFile = Path.Join(Context.ProjectDirectory, "config-file");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfgFile = new CloudFoundryManifestFile(_configFile);
            cfgFile.CloudFoundryManifest.Applications.Add(new CloudFoundryManifest.Application()
            {
                Name = "myapp",
                Command = "my command",
                BuildPacks = new List<string> {"my_build_pack"},
                Stack = "my stack",
                Memory = "my mem",
                Environment = new Dictionary<string, string> {{"myenv", "my var"}},
                ServiceNames = new List<string> {"my-service", "my-other-service"},
            });
            cfgFile.Store();
            File.ReadAllText(_configFile).ShouldBe(SampleConfig);
        }

        [Fact]
        public void TestDefaultFileName()
        {
            CloudFoundryManifestFile.DefaultFileName.ShouldBe("manifest-steeltoe.yaml");
        }

        private const string SampleConfig = @"applications:
- name: myapp
  command: my command
  buildpacks:
  - my_build_pack
  stack: my stack
  memory: my mem
  env:
    myenv: my var
  services:
  - my-service
  - my-other-service
";
    }
}
