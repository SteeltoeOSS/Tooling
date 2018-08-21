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
using Steeltoe.Tooling.Cli.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Cli.Test.CloudFoundry
{
    public class CloudFoundryServiceManagerTest
    {
        [Fact]
        public void TestStartCloudFoundryConfigServiceService()
        {
            var mgr = new CloudFoundryServiceManager();
            var shell = new MockShell();
            mgr.StartService(shell, "my-service", "cloud-foundry-config-server");
            shell.LastCommand.ShouldBe("cf create-service p-config-server standard my-service");
        }

        [Fact]
        public void TestStopService()
        {
            var mgr = new CloudFoundryServiceManager();
            var shell = new MockShell();
            mgr.StopService(shell, "my-service");
            shell.LastCommand.ShouldBe("cf delete-service my-service -f");
        }

        [Fact]
        public void TestCheckService()
        {
            var mgr = new CloudFoundryServiceManager();
            var shell = new MockShell();
            mgr.CheckService(shell, "my-service");
            shell.LastCommand.ShouldBe("cf service my-service");
        }

        // TODO: convert following to unit test
//        [Scenario]
//        public void CheckOnlineService()
//        {
//            Runner.RunScenario(
//                given => a_project(),
//                and => a_cloud_foundry_service("my-service"),
//                when => check_service_is_run("my-service"),
//                then => the_output_should_include("online")
//            );
//        }
    }
}
