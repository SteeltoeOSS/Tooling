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
    public class DockerServiceManagerTest
    {
        private readonly IServiceBackend _mgr;

        private readonly MockShell _shell;

        public DockerServiceManagerTest()
        {
            _shell = new MockShell(null);
            _mgr = new DockerEnvironment().GetServiceBackend(new Context(null, null, _shell));
            _mgr.ShouldBeOfType(typeof(DockerServiceBackend));
        }

        [Fact]
        public void TestStartConfigServer()
        {
            _mgr.DeployService("my-service", "config-server");
            _shell.LastCommand.ShouldBe(
                "docker run --name my-service --publish 8888:8888 --detach --rm steeltoeoss/configserver");
        }

        [Fact]
        public void TestStartRegistry()
        {
            _mgr.DeployService("my-service", "eureka");
            _shell.LastCommand.ShouldBe(
                "docker run --name my-service --publish 8761:8761 --detach --rm steeltoeoss/eurekaserver");
        }

        [Fact]
        public void TestStopService()
        {
            _mgr.UndeployService("my-service");
            _shell.LastCommand.ShouldBe("docker stop my-service");
        }

        [Fact]
        public void TestCheckService()
        {
            _mgr.GetServiceLifecleState("my-service");
            _shell.LastCommand.ShouldBe("docker ps --no-trunc --filter name=^/my-service$");
        }
    }
}
