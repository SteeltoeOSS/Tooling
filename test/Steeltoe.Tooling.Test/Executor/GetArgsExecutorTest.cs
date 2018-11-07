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
    public class GetArgsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestGetServiceArgs()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-service", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceNoArgs()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            ClearConsole();
            new GetArgsExecutor("my-service", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestGetServiceArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("no-such-service", "dummy-target").Execute(Context)
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestGetServiceArgsUnknownTarget()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("my-service", "no-such-target").Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }
    }
}
