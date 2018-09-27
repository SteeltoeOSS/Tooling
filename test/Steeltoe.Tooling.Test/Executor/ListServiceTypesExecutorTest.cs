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

using System.IO;
using Shouldly;
using Steeltoe.Tooling.Executor;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class ListServiceTypesExecutorTest : ToolingTest
    {
        [Fact]
        public void TestListServiceTypes()
        {
            new ListServiceTypesExecutor().Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("circuit-breaker-dashboard");
            reader.ReadLine().ShouldBe("config-server");
            reader.ReadLine().ShouldBe("dummy-svc");
            reader.ReadLine().ShouldBe("redis-server");
            reader.ReadLine().ShouldBe("service-registry");
            reader.ReadLine().ShouldBeNull();
        }

        [Fact]
        public void TestListServiceTypesVerbose()
        {
            new ListServiceTypesExecutor(true).Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("circuit-breaker-dashboard   7979  Netflix Hystrix Dashboard");
            reader.ReadLine().ShouldBe("config-server               8888  Cloud Foundry Config Server");
            reader.ReadLine().ShouldBe("dummy-svc                      0  A Dummy Service");
            reader.ReadLine().ShouldBe("redis-server                6379  Redis In-Memory Datastore");
            reader.ReadLine().ShouldBe("service-registry            8761  Netflix Eureka Server");
            reader.ReadLine().ShouldBeNull();
        }
    }
}
