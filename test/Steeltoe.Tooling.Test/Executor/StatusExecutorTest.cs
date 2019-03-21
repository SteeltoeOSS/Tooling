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
using Steeltoe.Tooling.Executor;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class StatusExecutorTest : ToolingTest
    {
        [Fact]
        public void TestStatusServices()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddService("another-service", "dummy-svc");
            ClearConsole();
            new StatusExecutor().Execute(Context);
            Console.ToString().ShouldContain("my-service offline");
            Console.ToString().ShouldContain("another-service offline");

            new DeployExecutor().Execute(Context);
            ClearConsole();
            new StatusExecutor().Execute(Context);
            Console.ToString().ShouldContain("my-service starting");
            Console.ToString().ShouldContain("another-service starting");

            ClearConsole();
            new StatusExecutor().Execute(Context);
            Console.ToString().ShouldContain("my-service online");
            Console.ToString().ShouldContain("another-service online");

            new UndeployExecutor().Execute(Context);
            ClearConsole();
            new StatusExecutor().Execute(Context);
            Console.ToString().ShouldContain("my-service stopping");
            Console.ToString().ShouldContain("another-service stopping");

            ClearConsole();
            new StatusExecutor().Execute(Context);
            Console.ToString().ShouldContain("my-service offline");
            Console.ToString().ShouldContain("another-service offline");
        }

        [Fact]
        public void TestStatusNoServices()
        {
            new StatusExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestStatusNoTarget()
        {
            Context.Configuration.Target = null;
            Context.Configuration.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ToolingException>(
                () => new StatusExecutor().Execute(Context)
            );
            e.Message.ShouldBe("Target not set");
        }
    }
}
