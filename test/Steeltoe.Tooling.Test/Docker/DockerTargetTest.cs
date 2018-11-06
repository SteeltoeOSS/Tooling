// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Shouldly;
using Steeltoe.Tooling.Docker;
using Xunit;

namespace Steeltoe.Tooling.Test.Docker
{
    public class DockerTargetTest : ToolingTest
    {
        private readonly DockerTarget _env;

        public DockerTargetTest()
        {
            _env = Registry.GetTarget("docker") as DockerTarget;
        }

        [Fact]
        public void TestGetName()
        {
            _env.Name.ShouldBe("docker");
        }

        [Fact]
        public void TestGetDescription()
        {
            _env.Description.ShouldBe("Docker");
        }

        [Fact]
        public void TestGetBackend()
        {
            _env.GetBackend(Context).ShouldBeOfType<DockerBackend>();
        }

        [Fact]
        public void TestIsHealthy()
        {
            Shell.AddResponse("Docker version SOME VERSION");
            Shell.AddResponse(@"
Operating System: SOME HOST OS
OSType: SOME CONTAINER OS
");
            var healthy = _env.IsHealthy(Context);
            healthy.ShouldBeTrue();
            var expected = new[]
            {
                "docker --version",
                "docker info",
            };
            Shell.Commands.Count.ShouldBe(expected.Length);
            for (int i = 0; i < expected.Length; ++i)
            {
                Shell.Commands[i].ShouldBe(expected[i]);
            }

            Console.ToString().ShouldContain("Docker ... Docker version SOME VERSION");
            Console.ToString().ShouldContain("Docker host OS ... SOME HOST OS");
            Console.ToString().ShouldContain("Docker container OS ... SOME CONTAINER OS");
        }

        [Fact]
        public void TestIsHealthyDockerNotRunning()
        {
            Shell.AddResponse("");
            Shell.AddResponse("", 1);
            var healthy = _env.IsHealthy(Context);
            healthy.ShouldBeFalse();
        }
    }
}
