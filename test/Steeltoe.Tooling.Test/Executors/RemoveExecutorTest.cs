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
    public class RemoveExecutorTest : ToolingTest
    {
        [Fact]
        public void TestRemoveService()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddService("my-other-service", "dummy-svc");
            new RemoveExecutor("my-service").Execute(Context);
            Console.ToString().Trim().ShouldBe("Removed dummy-svc service 'my-service'");
            Context.Configuration.GetServices().ShouldNotContain("my-service");
            Context.Configuration.GetServices().ShouldContain("my-other-service");
        }

        [Fact]
        public void TestRemoveUnknownItem()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new RemoveExecutor("no-such-item").Execute(Context)
            );
            e.Name.ShouldBe("no-such-item");
            e.Description.ShouldBe("app or service");
        }
    }
}
