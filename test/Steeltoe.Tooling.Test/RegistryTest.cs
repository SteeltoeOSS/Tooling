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

using System.Collections.Generic;
using Shouldly;
using Steeltoe.Tooling.CloudFoundry;
using Steeltoe.Tooling.Docker;
using Steeltoe.Tooling.Dummy;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class RegistryTest : ToolingTest
    {
        [Fact]
        public void TestServiceNames()
        {
            var expected = new List<string>()
            {
                "dummy-svc",
                "config-server",
                "eureka-server",
                "hystrix-dashboard",
                "mssql",
                "redis",
                "zipkin",
            };
            foreach (var name in Registry.ServiceTypeNames)
            {
                expected.ShouldContain(name);
                expected.Remove(name);
            }

            expected.ShouldBeEmpty();
        }

        [Fact]
        public void TestEnvironmentNames()
        {
            var expected = new List<string>()
            {
                "dummy-env",
                "cloud-foundry",
                "docker"
            };
            foreach (var name in Registry.EnvironmentNames)
            {
                expected.ShouldContain(name);
                expected.Remove(name);
            }

            expected.ShouldBeEmpty();
        }

        [Fact]
        public void TestDummyEnvironment()
        {
            Registry.GetEnvironment("dummy-env").ShouldBeOfType<DummyEnvironment>();
        }

        [Fact]
        public void TestDockerEnvironment()
        {
            Registry.GetEnvironment("docker").ShouldBeOfType<DockerEnvironment>();
        }

        [Fact]
        public void TestCloudFoundryEnvironment()
        {
            Registry.GetEnvironment("cloud-foundry").ShouldBeOfType<CloudFoundryEnvironment>();
        }
    }
}
