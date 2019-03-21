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
    public class RemoveServiceExecutorTest : ToolingTest
    {
        [Fact]
        public void TestRemoveService()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.AddService("another-service", "dummy-svc");
            new RemoveServiceExecutor("a-service").Execute(Context);
            Console.ToString().Trim().ShouldBe("Removed service 'a-service'");
            Context.ServiceManager.GetServiceNames().ShouldNotContain("a-service");
            Context.ServiceManager.GetServiceNames().ShouldContain("another-service");
        }

        [Fact]
        public void TestRemoveNonExistentService()
        {
            var svc = new RemoveServiceExecutor("non-existent-service");
            Assert.Throws<ServiceNotFoundException>(
                () => svc.Execute(Context)
            );
        }
    }
}
