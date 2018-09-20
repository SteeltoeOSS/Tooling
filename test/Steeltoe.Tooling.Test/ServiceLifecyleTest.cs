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
    public class ServiceLifecyleTest : ToolingTest
    {
        [Fact]
        public void TestStateMachine()
        {
            var svcMgr = Context.ServiceManager;
            svcMgr.AddService("a-service", "dummy-svc");
            ServiceLifecycle lifecycle = new ServiceLifecycle(Context, "a-service");

            // start state -> disabled
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Disabled);

            // disabled -> deploy -> ERROR
            lifecycle.Disable();
            Assert.Throws<ServiceLifecycleException>(
                () => lifecycle.Deploy()
            );
            // disabled -> undeploy -> ERROR
            Assert.Throws<ServiceLifecycleException>(
                () => lifecycle.Undeploy()
            );
            // disabled -> disable -> disabled
            lifecycle.Disable();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Disabled);
            // disabled -> enable -> enabled
            lifecycle.Enable();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Offline);

            // offline -> undeploy -> offline
            lifecycle.Undeploy();
            Context.ServiceManager.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Offline);
            // offline -> enable -> offline
            lifecycle.Enable();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Offline);
            // offline -> disable -> disabled
            lifecycle.Disable();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Disabled);
            // offline -> deploy -> starting -> online
            lifecycle.Enable();
            lifecycle.Deploy();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Starting);
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Online);

            // online -> enable -> ERROR
            Assert.Throws<ServiceLifecycleException>(
                () => lifecycle.Enable()
            );
            // online -> disable -> ERROR
            Assert.Throws<ServiceLifecycleException>(
                () => lifecycle.Disable()
            );
            // online -> deploy -> online
            lifecycle.Deploy();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Online);
            // online -> undeploy -> stopping -> offline
            lifecycle.Undeploy();
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Stopping);
            svcMgr.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Offline);
        }
    }
}
