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
using Steeltoe.Tooling.Executor;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class UndeployServiceExecutorTest : ToolingTest
    {
        [Fact]
        public void TestUndeployService()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.EnableService("a-service");
            Context.ServiceManager.AddService("defunct-service", "dummy-svc");
            Context.ServiceManager.DisableService("defunct-service");
            ClearConsole();
            new UndeployServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("Undeploying service 'a-service'");
            Console.ToString().ShouldContain("Ignoring disabled service 'defunct-service'");
            Context.ServiceManager.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Offline);
            Context.ServiceManager.GetServiceState("defunct-service").ShouldBe(ServiceLifecycle.State.Disabled);

            new DeployServicesExecutor().Execute(Context);
            new StatusServicesExecutor().Execute(Context);
            ClearConsole();
            new UndeployServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("Undeploying service 'a-service");
            Console.ToString().ShouldContain("Ignoring disabled service 'defunct-service'");
            Context.ServiceManager.GetServiceState("a-service").ShouldBe(ServiceLifecycle.State.Stopping);
            Context.ServiceManager.GetServiceState("defunct-service").ShouldBe(ServiceLifecycle.State.Disabled);
        }

        [Fact]
        public void TestUndeployNoServices()
        {
            new UndeployServicesExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }
    }
}
