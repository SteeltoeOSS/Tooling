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
    public class DeployExecutorTest : ToolingTest
    {
        [Fact]
        public void TestDeploy()
        {
            Context.Configuration.AddApp("my-app");
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddService("another-service", "dummy-svc");
            ClearConsole();
            new DeployExecutor().Execute(Context);
            Console.ToString().ShouldContain("Deploying service 'my-service'");
            Console.ToString().ShouldContain("Deploying service 'another-service'");
            Console.ToString().ShouldContain("Deploying app 'my-app'");
            Context.Backend.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Online);
            Context.Backend.GetServiceStatus("another-service").ShouldBe(Lifecycle.Status.Online);
            Context.Backend.GetAppStatus("my-app").ShouldBe(Lifecycle.Status.Starting);
        }

        [Fact]
        public void TestDeployNothing()
        {
            new DeployExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestDeployNoTarget()
        {
            Context.Configuration.Target = null;
            Context.Configuration.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ToolingException>(
                () => new DeployExecutor().Execute(Context)
            );
            e.Message.ShouldBe("Target not set");
        }
    }
}
