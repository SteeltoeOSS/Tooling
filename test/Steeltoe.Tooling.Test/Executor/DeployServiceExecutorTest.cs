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
using Steeltoe.Tooling.Executor.Service;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class DeployServiceExecutorTest : ToolingTest
    {
        [Fact]
        public void TestDeployService()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Assert.Throws<ServiceLifecycleException>(
                () => new DeployServiceExecutor("a-service").Execute(Context)
            );
            Context.ServiceManager.EnableService("a-service");
            new DeployServiceExecutor("a-service").Execute(Context);
            Console.ToString().Trim().ShouldBe("Deployed service 'a-service'");
            Context.ServiceManager.GetServiceStatus("a-service").ShouldBe("starting");
            Context.ServiceManager.GetServiceStatus("a-service").ShouldBe("online");
            new DeployServiceExecutor("a-service").Execute(Context);
            Context.ServiceManager.GetServiceStatus("a-service").ShouldBe("online");
        }

        [Fact]
        public void TestDeployDisabledService()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Assert.Throws<ServiceLifecycleException>(
                () => new DeployServiceExecutor("a-service").Execute(Context)
            );
        }

        [Fact]
        public void TestDeployNonExistentService()
        {
            Assert.Throws<ServiceNotFoundException>(
                () => new DeployServiceExecutor("non-existent-service").Execute(Context)
            );
        }
    }
}
