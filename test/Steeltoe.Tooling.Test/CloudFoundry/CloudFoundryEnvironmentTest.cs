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
using Steeltoe.Tooling.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test.CloudFoundry
{
    public class CloudFoundryEnvironmentTest : ToolingTest
    {
        private readonly Environment _env;

        public CloudFoundryEnvironmentTest()
        {
            _env = Registry.GetEnvironment("cloud-foundry");
            _env.ShouldBeOfType<CloudFoundryEnvironment>();
        }

        [Fact]
        public void TestGetName()
        {
            _env.Name.ShouldBe("cloud-foundry");
        }

        [Fact]
        public void TestGetDescription()
        {
            _env.Description.ShouldBe("Cloud Foundry");
        }

        [Fact]
        public void TestGetServiceManager()
        {
            _env.GetServiceBackend(Context).ShouldBeOfType<CloudFoundryServiceBackend>();
        }

        [Fact]
        public void TestIsHealthy()
        {
            Shell.AddResponse("cf version SOME VERSION");
            var healthy = _env.IsHealthy(Context);
            healthy.ShouldBeTrue();
            var expected = new[]
            {
                "cf --version",
                "cf target",
            };
            Shell.Commands.Count.ShouldBe(expected.Length);
            for (int i = 0; i < expected.Length; ++i)
            {
                Shell.Commands[i].ShouldBe(expected[i]);
            }
            Console.ToString().ShouldContain("Cloud Foundry ... cf version SOME VERSION");
            Console.ToString().ShouldContain("logged into Cloud Foundry ... yes");
        }

        [Fact]
        public void TestIsHealthyNotLoggedIn()
        {
            Shell.AddResponse("");
            Shell.AddResponse("", 1);
            var healthy = _env.IsHealthy(Context);
            healthy.ShouldBeFalse();
            Console.ToString().ShouldContain("logged into Cloud Foundry ... !!! no");
        }
    }
}
