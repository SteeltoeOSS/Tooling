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
    public class LifecyleTest : ToolingTest
    {
        [Fact]
        public void TestAppStateMachine()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            var lifecycle = new Lifecycle(Context, "my-app");

            // start state -> offline
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);

            // offline -> undeploy -> offline
            lifecycle.Undeploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);

            // offline -> deploy -> online
            lifecycle.Deploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Online);

            // online -> deploy -> online
            lifecycle.Deploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Online);

            // online -> undeploy -> offline
            lifecycle.Undeploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);
        }

        [Fact]
        public void TestServiceStateMachine()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            var lifecycle = new Lifecycle(Context, "my-service");

            // start state -> offline
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);

            // offline -> undeploy -> offline
            lifecycle.Undeploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);

            // offline -> deploy -> starting -> online
            lifecycle.Deploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Starting);
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Online);

            // online -> deploy -> online
            lifecycle.Deploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Online);

            // online -> undeploy -> stopping -> offline
            lifecycle.Undeploy();
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Stopping);
            lifecycle.GetStatus().ShouldBe(Lifecycle.Status.Offline);
        }

    }
}
