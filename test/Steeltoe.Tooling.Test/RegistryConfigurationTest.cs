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
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class RegistryConfigurationTest
    {
        private Registry.Configuration _registryCfg = new Registry.Configuration();

        [Fact]
        public void TestDefineServiceType()
        {
            _registryCfg.DefineServiceType("my-service", 1234, "my service description");
            _registryCfg.DefineServiceType("my-other-service", 4321, "my other service description");
            _registryCfg.ServiceTypes.Count.ShouldBe(2);
            _registryCfg.ServiceTypes["my-service"].ShouldNotBeNull();
            _registryCfg.ServiceTypes["my-service"].Port.ShouldBe(1234);
            _registryCfg.ServiceTypes["my-service"].Description.ShouldBe("my service description");
            _registryCfg.ServiceTypes["my-other-service"].ShouldNotBeNull();
            _registryCfg.ServiceTypes["my-other-service"].Port.ShouldBe(4321);
            _registryCfg.ServiceTypes["my-other-service"].Description.ShouldBe("my other service description");
        }

        [Fact]
        public void TestDefineEnvironment()
        {
            _registryCfg.DefineTarget("my-env", "my environment");
            _registryCfg.DefineTarget("my-other-env", "my other environment");
            _registryCfg.TargetConfigurations.Count.ShouldBe(2);
            _registryCfg.TargetConfigurations["my-env"].ShouldNotBeNull();
            _registryCfg.TargetConfigurations["my-env"].Description.ShouldBe("my environment");
            _registryCfg.TargetConfigurations["my-other-env"].ShouldNotBeNull();
            _registryCfg.TargetConfigurations["my-other-env"].Description.ShouldBe("my other environment");
        }

        [Fact]
        public void TestDefineEnvironmentServiceTypeProperty()
        {
            _registryCfg.DefineTarget("my-env", "my environment");
            _registryCfg.DefineTargetServiceTypeProperty("my-env", "my-service", "my-prop-a", "my value a");
            _registryCfg.DefineTargetServiceTypeProperty("my-env", "my-service", "my-prop-b", "my value b");
            _registryCfg.DefineTargetServiceTypeProperty("my-env", "my-other-service", "my-other-prop",
                "my other value");
            _registryCfg.DefineTargetServiceTypeProperty("my-other-env", "my-other-env-service", "my-other-env-prop",
                "my other env value");
            _registryCfg.TargetConfigurations.Count.ShouldBe(2);
            _registryCfg.TargetConfigurations["my-env"].ShouldNotBeNull();
            _registryCfg.TargetConfigurations["my-env"].ServiceTypeProperties["my-service"].ShouldNotBeNull();
            _registryCfg.TargetConfigurations["my-env"].ServiceTypeProperties["my-service"]["my-prop-a"]
                .ShouldBe("my value a");
            _registryCfg.TargetConfigurations["my-env"].ServiceTypeProperties["my-service"]["my-prop-b"]
                .ShouldBe("my value b");
            _registryCfg.TargetConfigurations["my-env"].ServiceTypeProperties["my-other-service"]["my-other-prop"]
                .ShouldBe("my other value");
            _registryCfg.TargetConfigurations["my-other-env"].ShouldNotBeNull();
            _registryCfg.TargetConfigurations["my-other-env"].Description.ShouldBeNull();
            _registryCfg.TargetConfigurations["my-other-env"].ServiceTypeProperties["my-other-env-service"][
                    "my-other-env-prop"]
                .ShouldBe("my other env value");
        }
    }
}
