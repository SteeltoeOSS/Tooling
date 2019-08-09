using System.IO;
using Shouldly;
using Steeltoe.Tooling.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Kubernetes
{
    public class KubernetesDotnetAppDockerfileTest : ToolingTest
    {
        private readonly string _dockerFile;

        public KubernetesDotnetAppDockerfileTest()
        {
            _dockerFile = Path.Join(Context.ProjectDirectory, "kubernetes-dotnet-app-dockerfile");
        }

        [Fact]
        public void TestLoadFromFile()
        {
            File.WriteAllText(_dockerFile, SampleDockerfile);
            var dockerfile = new KubernetesDotnetAppDockerfileFile(_dockerFile).KubernetesDotnetAppDockerfile;
            dockerfile.BaseImage.ShouldBe("mytag");
            dockerfile.App.ShouldBe("MyApp");
            dockerfile.BuildPath.ShouldBe("build/path");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var dockerFile = new KubernetesDotnetAppDockerfileFile(_dockerFile);
            dockerFile.KubernetesDotnetAppDockerfile.BaseImage = "mytag";
            dockerFile.KubernetesDotnetAppDockerfile.App = "MyApp";
            dockerFile.KubernetesDotnetAppDockerfile.BuildPath = "build/path";
            dockerFile.Store();
            File.ReadAllText(_dockerFile).ShouldBe(SampleDockerfile);
        }

        private const string SampleDockerfile = @"FROM mytag
COPY build/path /app
CMD dotnet /app/MyApp.dll
";
    }
}
