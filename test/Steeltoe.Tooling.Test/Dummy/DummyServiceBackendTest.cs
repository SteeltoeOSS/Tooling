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

using System.IO;
using Shouldly;
using Steeltoe.Tooling.Dummy;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace Steeltoe.Tooling.Test.Dummy
{
    public class DummyServiceBackendTest : ToolingTest
    {
        public DummyServiceBackendTest()
        {
            Directory.CreateDirectory(Context.ProjectDirectory);
        }

        [Fact]
        public void TestLifecyle()
        {
            string dbFile = Path.Combine(Context.ProjectDirectory, "dummy.db");
            new DummyServiceBackend(dbFile);
            File.Exists(dbFile).ShouldBeTrue("dummy.db");

            // start state -> offline
            new DummyServiceBackend(dbFile).GetServiceLifecleState("a-service")
                .ShouldBe(ServiceLifecycle.State.Offline);

            // offline -> deploy -> starting -> online
            new DummyServiceBackend(dbFile).DeployService("a-service", "dummy-svc");
            new DummyServiceBackend(dbFile).GetServiceLifecleState("a-service")
                .ShouldBe(ServiceLifecycle.State.Starting);
            new DummyServiceBackend(dbFile).GetServiceLifecleState("a-service").ShouldBe(ServiceLifecycle.State.Online);

            // online -> undeploy -> stopping -> offline
            new DummyServiceBackend(dbFile).UndeployService("a-service");
            new DummyServiceBackend(dbFile).GetServiceLifecleState("a-service")
                .ShouldBe(ServiceLifecycle.State.Stopping);
            new DummyServiceBackend(dbFile).GetServiceLifecleState("a-service")
                .ShouldBe(ServiceLifecycle.State.Offline);
        }
    }
}
