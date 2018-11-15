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
        public void TestGetArgsUnknownAppOrService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("no-such-app-or-svc").Execute(Context)
            );
            e.Name.ShouldBe("no-such-app-or-svc");
            e.Description.ShouldBe("app or service");
        }

        [Fact]
        public void TestGetTargetArgsUnknownAppOrService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("no-such-app-or-svc", "dummy-target").Execute(Context)
            );
            e.Name.ShouldBe("no-such-app-or-svc");
            e.Description.ShouldBe("app or service");
        }

        [Fact]
        public void TestGetAppArgs()
        {
            new AddExecutor("my-app").Execute(Context);
            new SetArgsExecutor("my-app", "arg1 arg2").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-app").Execute(Context);
            Console.ToString().Trim().ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetAppTargetArgs()
        {
            new AddExecutor("my-app").Execute(Context);
            new SetArgsExecutor("my-app", "dummy-target", "arg1 arg2").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-app", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetAppNoArgs()
        {
            new AddExecutor("my-app").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-app").Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestGetAppTargetNoArgs()
        {
            new AddExecutor("my-app").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-app", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestGetAppArgsUnknownTarget()
        {
            new AddExecutor("my-app").Execute(Context);
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("my-app", "no-such-target").Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestGetServiceArgs()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            new SetArgsExecutor("my-service", "arg1 arg2").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-service").Execute(Context);
            Console.ToString().Trim().ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceTargetArgs()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-service", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceNoArgs()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-service").Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestGetServiceTargetNoArgs()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new GetArgsExecutor("my-service", "dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestGetServiceArgsUnknownTarget()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new GetArgsExecutor("my-service", "no-such-target").Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }
    }
}
