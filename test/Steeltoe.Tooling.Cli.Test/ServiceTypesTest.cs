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
using Xunit;

namespace Steeltoe.Tooling.Cli.Test
{
    public class ServiceTypesTest
    {
        [Fact]
        public void TestNames()
        {
            var expected = new List<string>()
            {
                "config-server",
                "registry"
            };
            foreach (var name in ServiceTypes.GetNames())
            {
                expected.ShouldContain(name);
                expected.Remove(name);
            }

            expected.ShouldBeEmpty();
        }

        [Fact]
        public void TestDescriptions()
        {
            ServiceTypes.GetDescription("config-server").ShouldBe("Spring Cloud Config Server");
            ServiceTypes.GetDescription("registry").ShouldBe("Netflix Eureka Server");
        }

        [Fact]
        public void TestUnknown()
        {
            ServiceTypes.GetDescription("unknown-name").ShouldBeNull();
        }
    }
}
