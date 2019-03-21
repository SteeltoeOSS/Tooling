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
using Steeltoe.Tooling.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test.CloudFoundry
{
    public class CloudFoundryServiceManagerTest
    {
        private readonly CloudFoundryServiceBackend _mgr;

        private readonly MockShell _shell;

        public CloudFoundryServiceManagerTest()
        {
            _shell = new MockShell(null);
            _mgr = new CloudFoundryServiceBackend(_shell);
        }

        [Fact]
        public void TestStartConfigServer()
        {
            _mgr.DeployService("my-service", "config-server");
            _shell.LastCommand.ShouldBe("cf create-service p-config-server standard my-service");
        }

        [Fact]
        public void TestStartRegistry()
        {
            _mgr.DeployService("my-service", "registry");
            _shell.LastCommand.ShouldBe("cf create-service p-service-registry standard my-service");
        }

        [Fact]
        public void TestStopService()
        {
            _mgr.UndeployService("my-service");
            _shell.LastCommand.ShouldBe("cf delete-service my-service -f");
        }

        [Fact]
        public void TestCheckService()
        {
            _mgr.GetServiceLifecleState("my-service");
            _shell.LastCommand.ShouldBe("cf service my-service");
        }
    }
}
