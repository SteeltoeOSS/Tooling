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
using Steeltoe.Tooling.Environment;
using Steeltoe.Tooling.Environment.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test.Environment
{
    public class EnvironmentRegistryTest
    {
        [Fact]
        public void TestEnvironmentNames()
        {
            var expected = new List<string>()
            {
                "cloud-foundry",
                "docker"
            };
            foreach (var name in EnvironmentRegistry.GetNames())
            {
                expected.ShouldContain(name);
                expected.Remove(name);
            }

            expected.ShouldBeEmpty();
        }

        [Fact]
        public void TestUnknown()
        {
            EnvironmentRegistry.ForName("unknown-name").ShouldBeNull();
        }

        [Fact]
        public void TestCloudFoundry()
        {
            EnvironmentRegistry.ForName("cloud-foundry").ShouldBeOfType<CloudFoundryEnvironment>();
        }
    }
}
