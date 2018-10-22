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
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class RegistryConfigurationTest
    {
        [Fact]
        public void TestDefineServiceType()
        {
            var cfg = new RegistryConfiguration();
            cfg.DefineServiceType("my-service", 1234, "my service description");
            cfg.DefineServiceType("my-other-service", 4321, "my other service description");
            cfg.ServiceTypes.Count.ShouldBe(2);
            cfg.ServiceTypes["my-service"].ShouldNotBeNull();
            cfg.ServiceTypes["my-service"].Port.ShouldBe(1234);
            cfg.ServiceTypes["my-service"].Description.ShouldBe("my service description");
            cfg.ServiceTypes["my-other-service"].ShouldNotBeNull();
            cfg.ServiceTypes["my-other-service"].Port.ShouldBe(4321);
            cfg.ServiceTypes["my-other-service"].Description.ShouldBe("my other service description");
        }

        [Fact]
        public void TestDefineEnvironment()
        {
            var cfg = new RegistryConfiguration();
            cfg.DefineEnvironment("my-env", "my environment");
            cfg.DefineEnvironment("my-other-env", "my other environment");
            cfg.EnvironmentConfigurations.Count.ShouldBe(2);
            cfg.EnvironmentConfigurations["my-env"].ShouldNotBeNull();
            cfg.EnvironmentConfigurations["my-env"].Description.ShouldBe("my environment");
            cfg.EnvironmentConfigurations["my-other-env"].ShouldNotBeNull();
            cfg.EnvironmentConfigurations["my-other-env"].Description.ShouldBe("my other environment");
        }

        [Fact]
        public void TestDefineEnvironmentServiceTypeProperty()
        {
            var cfg = new RegistryConfiguration();
            cfg.DefineEnvironment("my-env", "my environment");
            cfg.DefineEnvironmentServiceTypeProperty("my-env", "my-service", "my-prop-a", "my value a");
            cfg.DefineEnvironmentServiceTypeProperty("my-env", "my-service", "my-prop-b", "my value b");
            cfg.DefineEnvironmentServiceTypeProperty("my-env", "my-other-service", "my-other-prop", "my other value");
            cfg.DefineEnvironmentServiceTypeProperty("my-other-env", "my-other-env-service", "my-other-env-prop",
                "my other env value");
            cfg.EnvironmentConfigurations.Count.ShouldBe(2);
            cfg.EnvironmentConfigurations["my-env"].ShouldNotBeNull();
            cfg.EnvironmentConfigurations["my-env"].ServiceTypeProperties["my-service"].ShouldNotBeNull();
            cfg.EnvironmentConfigurations["my-env"].ServiceTypeProperties["my-service"]["my-prop-a"].ShouldBe("my value a");
            cfg.EnvironmentConfigurations["my-env"].ServiceTypeProperties["my-service"]["my-prop-b"].ShouldBe("my value b");
            cfg.EnvironmentConfigurations["my-env"].ServiceTypeProperties["my-other-service"]["my-other-prop"]
                .ShouldBe("my other value");
            cfg.EnvironmentConfigurations["my-other-env"].ShouldNotBeNull();
            cfg.EnvironmentConfigurations["my-other-env"].Description.ShouldBeNull();
            cfg.EnvironmentConfigurations["my-other-env"].ServiceTypeProperties["my-other-env-service"]["my-other-env-prop"]
                .ShouldBe("my other env value");
        }
    }
}
