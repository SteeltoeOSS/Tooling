// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
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
    public class DockerServiceManagerTest : ToolingTest
    {
        private readonly IServiceBackend _backend;

        public DockerServiceManagerTest()
        {
            _backend = new DockerEnvironment().GetServiceBackend(Context);
            _backend.ShouldBeOfType(typeof(DockerServiceBackend));
        }

        [Fact]
        public void TestDeployService()
        {
            _backend.DeployService("a-service", "dummy-svc");
            Shell.LastCommand.ShouldBe(
                "docker run --name a-service --publish -1:-1 --detach --rm steeltoeoss/dummyserver");
        }

        [Fact]
        public void TestDeployServiceWithArgs()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.SetServiceDeploymentArgs("docker", "a-service", "arg1 arg2");
            _backend.DeployService("a-service", "dummy-svc");
            Shell.LastCommand.ShouldBe(
                "docker run --name a-service --publish -1:-1 --detach --rm arg1 arg2 steeltoeoss/dummyserver");
        }

        [Fact]
        public void TestUndeployService()
        {
            _backend.UndeployService("a-service");
            Shell.LastCommand.ShouldBe("docker stop a-service");
        }

        [Fact]
        public void TestGetServiceLifecycleState()
        {
            _backend.GetServiceLifecleState("a-service");
            Shell.LastCommand.ShouldBe("docker ps --no-trunc --filter name=^/a-service$");
        }
    }
}
