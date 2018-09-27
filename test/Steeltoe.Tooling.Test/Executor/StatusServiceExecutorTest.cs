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
    public class StatusServiceExecutorTest : ToolingTest
    {
        [Fact]
        public void TestStatusServices()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.EnableService("a-service");
            Context.ServiceManager.AddService("defunct-service", "dummy-svc");
            Context.ServiceManager.DisableService("defunct-service");
            ClearConsole();
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service offline");
            Console.ToString().ShouldContain("defunct-service disabled");

            new DeployServicesExecutor().Execute(Context);
            ClearConsole();
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service starting");
            Console.ToString().ShouldContain("defunct-service disabled");

            ClearConsole();
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service online");
            Console.ToString().ShouldContain("defunct-service disabled");

            new UndeployServicesExecutor().Execute(Context);
            ClearConsole();
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service stopping");
            Console.ToString().ShouldContain("defunct-service disabled");

            ClearConsole();
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service offline");
            Console.ToString().ShouldContain("defunct-service disabled");
        }

        [Fact]
        public void TestStatusNoServices()
        {
            new StatusServicesExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestStatusNoTarget()
        {
            Context.ToolingConfiguration.EnvironmentName = null;
            Context.ServiceManager.AddService("a-existing-service", "dummy-svc");
            Context.ServiceManager.EnableService("a-existing-service");
            var e = Assert.Throws<ToolingException>(
                () => new StatusServicesExecutor().Execute(Context)
            );
            e.Message.ShouldBe("Target deployment environment not set");
        }
    }
}
