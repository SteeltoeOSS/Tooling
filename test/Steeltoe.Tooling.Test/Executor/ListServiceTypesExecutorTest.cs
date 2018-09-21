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
            reader.ReadLine().ShouldBe("config-server");
            reader.ReadLine().ShouldBe("dummy-svc");
            reader.ReadLine().ShouldBe("eureka");
            reader.ReadLine().ShouldBe("hystrix");
            reader.ReadLine().ShouldBe("mssql");
            reader.ReadLine().ShouldBe("redis");
            reader.ReadLine().ShouldBe("uaa");
            reader.ReadLine().ShouldBe("zipkin");
            reader.ReadLine().ShouldBeNull();
        }

        [Fact]
        public void TestListServiceTypesVerbose()
        {
            new ListServiceTypesExecutor(true).Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("config-server   8888  Cloud Foundry Config Server");
            reader.ReadLine().ShouldBe("dummy-svc          0  A Dummy Service");
            reader.ReadLine().ShouldBe("eureka          8761  Netflix Eureka Server");
            reader.ReadLine().ShouldBe("hystrix         7979  Netflix Hystrix Server");
            reader.ReadLine().ShouldBe("mssql           1433  Microsoft SQL Server");
            reader.ReadLine().ShouldBe("redis           6379  Redis Server");
            reader.ReadLine().ShouldBe("uaa             8080  Workshop User Account and Authentication Server");
            reader.ReadLine().ShouldBe("zipkin          9411  Zipkin Distributed Tracing System");
            reader.ReadLine().ShouldBeNull();
        }
    }
}
