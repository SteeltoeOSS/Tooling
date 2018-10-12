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
    public class SetServiceDeploymentArgsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestSetServiceDeploymentArgs()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            ClearConsole();
            new SetServiceDeploymentArgsExecutor("dummy-env", "a-service", "arg1 arg2").Execute(Context);
            Console.ToString().Trim().ShouldBe("Set the 'dummy-env' deployment environment arguments for service 'a-service' to 'arg1 arg2'");
        }
        [Fact]

        public void TestSetServiceDeploymentArgsForce()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            ClearConsole();
            new SetServiceDeploymentArgsExecutor("dummy-env", "a-service", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetServiceDeploymentArgsExecutor("dummy-env", "a-service", "arg1 arg2").Execute(Context)
            );
            e.Message.ShouldBe("'dummy-env' deployment environment arguments for service 'a-service' already set.");
            ClearConsole();
            new SetServiceDeploymentArgsExecutor("dummy-env", "a-service", "arg1 arg2", true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Set the 'dummy-env' deployment environment arguments for service 'a-service' to 'arg1 arg2'");
        }


        [Fact]
        public void TestSetNonExistentServiceDeploymentArgs()
        {
            Assert.Throws<ServiceNotFoundException>(
                () => new SetServiceDeploymentArgsExecutor("dummy-env", "non-existent-service", null).Execute(Context)
            );
        }
    }
}
