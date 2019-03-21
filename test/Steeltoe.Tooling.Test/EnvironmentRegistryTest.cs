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

using System.Collections.Generic;
using Shouldly;
using Steeltoe.Tooling.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class EnvironmentRegistryTest : ToolingTest
    {
        [Fact]
        public void TestEnvironmentNames()
        {
            var expected = new List<string>()
            {
                "dummy-env",
                "cloud-foundry",
                "docker"
            };
            foreach (var name in EnvironmentRegistry.Names)
            {
                expected.ShouldContain(name);
                expected.Remove(name);
            }

            expected.ShouldBeEmpty();
        }

        [Fact]
        public void TestUnknownEnvironment()
        {
            var e = Assert.Throws<ToolingException>(
                () => EnvironmentRegistry.ForName("unknown-env")
            );
            e.Message.ShouldBe("Unknown deployment environment 'unknown-env'");
        }

        [Fact]
        public void TestCloudFoundry()
        {
            EnvironmentRegistry.ForName("cloud-foundry").ShouldBeOfType<CloudFoundryEnvironment>();
        }
    }
}
