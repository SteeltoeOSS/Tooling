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
        public void TestLoadFromFile()
        {
            File.WriteAllText(_configFile, SampleConfig);
            var cfgFile = new CloudFoundryManifestFile(_configFile);
            cfgFile.CloudFoundryManifest.Applications.Count.ShouldBe(1);
            var app = cfgFile.CloudFoundryManifest.Applications[0];
            app.Name.ShouldBe("myapp");
            app.Command.ShouldBe("my command");
            app.BuildPacks.Count.ShouldBe(1);
            app.BuildPacks[0].ShouldBe("my_build_pack");
            app.Stack.ShouldBe("my stack");
            app.Memory.ShouldBe("my mem");
            app.Environment.Count.ShouldBe(1);
            app.Environment["myenv"].ShouldBe("my var");
            app.ServiceNames.Count.ShouldBe(2);
            app.ServiceNames.ShouldContain("my-service");
            app.ServiceNames.ShouldContain("my-other-service");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfgFile = new CloudFoundryManifestFile(_configFile);
            cfgFile.CloudFoundryManifest.Applications.Add(new CloudFoundryManifest.Application
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
            CloudFoundryManifestFile.DefaultFileName.ShouldBe("manifest-steeltoe.yml");
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
