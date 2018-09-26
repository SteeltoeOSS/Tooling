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
    public class CloudFoundryServiceManagerTest : ToolingTest
    {
        private readonly IServiceBackend _backend;

        public CloudFoundryServiceManagerTest()
        {
            Context.ProjectConfiguration.EnvironmentName = "cloud-foundry";
            _backend = Context.Environment.GetServiceBackend(Context);
            _backend.ShouldBeOfType(typeof(CloudFoundryServiceBackend));

        }

        [Fact]
        public void TestDeployConfigServer()
        {
            _backend.DeployService("my-service", "config-server");
            Shell.LastCommand.ShouldBe("cf create-service p-config-server standard my-service");
        }

        [Fact]
        public void TestDeployServiceRegistry()
        {
            _backend.DeployService("my-service", "service-registry");
            Shell.LastCommand.ShouldBe("cf create-service p-service-registry standard my-service");
        }

        [Fact]
        public void TestDeployCircuitBreakerDashboard()
        {
            _backend.DeployService("my-service", "circuit-breaker-dashboard");
            Shell.LastCommand.ShouldBe("cf create-service p-circuit-breaker-dashboard standard my-service");
        }

        [Fact]
        public void TestStopService()
        {
            _backend.UndeployService("my-service");
            Shell.LastCommand.ShouldBe("cf delete-service my-service -f");
        }

        [Fact]
        public void TestCheckService()
        {
            _backend.GetServiceLifecleState("my-service");
            Shell.LastCommand.ShouldBe("cf service my-service");
        }

        [Fact]
        public void TestServiceStarting()
        {
            Shell.NextResponse = "status:    create in progress";
            var state = _backend.GetServiceLifecleState("my-service");
            state.ShouldBe(ServiceLifecycle.State.Starting);
        }

        [Fact]
        public void TestServiceOnline()
        {
            Shell.NextResponse = "status:    create succeeded";
            var state = _backend.GetServiceLifecleState("my-service");
            state.ShouldBe(ServiceLifecycle.State.Online);
        }

        [Fact]
        public void TestServiceOffline()
        {
            Shell.NextExitCode = 1;
            Shell.NextResponse = "Service instance my-service not found";
            var state = _backend.GetServiceLifecleState("my-service");
            state.ShouldBe(ServiceLifecycle.State.Offline);
        }
    }
}
