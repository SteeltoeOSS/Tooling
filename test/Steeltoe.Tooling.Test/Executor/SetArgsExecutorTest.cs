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
    public class SetArgsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestSetServiceArgs()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            Console.ToString().Trim().ShouldBe("Service 'my-service' args for target 'dummy-target' set to 'arg1 arg2'");
        }
        [Fact]

        public void TestSetServiceArgsForce()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context)
            );
            e.Message.ShouldBe("Service 'my-service' args for target 'dummy-target' already set to 'arg1 arg2'");
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg3", true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Service 'my-service' args for target 'dummy-target' set to 'arg3'");
        }

        [Fact]
        public void TestSetServiceArgsUnknownService()
        {
            Assert.Throws<NotFoundException>(
                () => new SetArgsExecutor("no-such-svc", "dummy-target", null).Execute(Context)
            );
        }

        [Fact]
        public void TestSetServiceArgsUnknownTarget()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<NotFoundException>(
                () => new SetArgsExecutor("my-service", "no-such-target", null).Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }
    }
}
