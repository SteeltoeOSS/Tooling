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
using Steeltoe.Tooling.Executors;
using Xunit;

namespace Steeltoe.Tooling.Test.Executors
{
    public class UndeployExecutorTest : ToolingTest
    {
        [Fact]
        public void TestUndeploy()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddService("my-other-service", "dummy-svc");
            new DeployExecutor().Execute(Context);
            Context.Driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Online);
            Context.Driver.GetServiceStatus("my-other-service").ShouldBe(Lifecycle.Status.Online);
            Context.Driver.GetAppStatus("my-app").ShouldBe(Lifecycle.Status.Online);
            ClearConsole();
            new UndeployExecutor().Execute(Context);
            Console.ToString().ShouldContain("Undeploying app 'my-app'");
            Console.ToString().ShouldContain("Undeploying service 'my-service'");
            Console.ToString().ShouldContain("Undeploying service 'my-other-service'");
            Context.Driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Offline);
            Context.Driver.GetServiceStatus("my-other-service").ShouldBe(Lifecycle.Status.Offline);
            Context.Driver.GetAppStatus("my-app").ShouldBe(Lifecycle.Status.Offline);
        }

        [Fact]
        public void TestUndeployNothing()
        {
            new UndeployExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestUndeployNoTarget()
        {
            Context.Configuration.Target = null;
            Context.Configuration.AddService("a-existing-service", "dummy-svc");
            var e = Assert.Throws<ToolingException>(
                () => new UndeployExecutor().Execute(Context)
            );
            e.Message.ShouldBe("Target not set");
        }
    }
}
