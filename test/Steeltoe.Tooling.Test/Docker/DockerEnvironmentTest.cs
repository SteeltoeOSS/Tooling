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
    public class DockerEnvironmentTest : ToolingTest
    {
        private readonly DockerEnvironment _env;

        public DockerEnvironmentTest()
        {
            _env = new DockerEnvironment();
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
        public void TestGetServiceManager()
        {
            _env.GetServiceBackend(Context).ShouldBeOfType<DockerServiceBackend>();
        }

        [Fact]
        public void TestIsSane()
        {
            _env.IsHealthy(Context.Shell);
            Shell.LastCommand.ShouldBe("docker info");
        }
    }
}
