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
    public class ListServicesExecutorTest : ToolingTest
    {
        [Fact]
        public void TestListNone()
        {
            var svc = new ListServicesExecutor();
            svc.Execute(Context);
            Console.ToString().ShouldBeEmpty();
        }

        [Fact]
        public void TestListServices()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.AddService("another-service", "dummy-svc");
            ClearConsole();
            new ListServicesExecutor().Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("a-service");
            reader.ReadLine().ShouldBe("another-service");
            reader.ReadLine().ShouldBeNull();
        }

        [Fact]
        public void TestListServicesVerbose()
        {
            Context.ServiceManager.AddService("a-service", "dummy-svc");
            Context.ServiceManager.AddService("another-service", "dummy-svc");
            Context.ServiceManager.EnableService("another-service");
            ClearConsole();
            new ListServicesExecutor(true).Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("a-service            0  dummy-svc");
            reader.ReadLine().ShouldBe("another-service      0  dummy-svc");
            reader.ReadLine().ShouldBeNull();
        }
    }
}
