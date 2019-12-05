using System.IO;
using Shouldly;
using Steeltoe.Tooling.Drivers.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.Kubernetes
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
            dockerfile.AppPath.ShouldBe("/myapppath");
            dockerfile.BuildPath.ShouldBe("build/path");
            dockerfile.Environment.ShouldBe("myenv");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var dockerFile = new KubernetesDotnetAppDockerfileFile(_dockerFile);
            dockerFile.KubernetesDotnetAppDockerfile.BaseImage = "mytag";
            dockerFile.KubernetesDotnetAppDockerfile.App = "MyApp";
            dockerFile.KubernetesDotnetAppDockerfile.AppPath = "/myapppath";
            dockerFile.KubernetesDotnetAppDockerfile.BuildPath = "build/path";
            dockerFile.KubernetesDotnetAppDockerfile.Environment = "myenv";
            dockerFile.Store();
            File.ReadAllText(_dockerFile).ShouldBe(SampleDockerfile);
        }

        private const string SampleDockerfile = @"FROM mytag
COPY build/path /myapppath
ENV ASPNETCORE_ENVIRONMENT myenv
WORKDIR /myapppath
CMD dotnet MyApp.dll
";
    }
}
