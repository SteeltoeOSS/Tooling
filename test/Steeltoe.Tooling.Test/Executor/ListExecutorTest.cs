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
    public class ListExecutorTest : ToolingTest
    {
        [Fact]
        public void TestListNone()
        {
            new ListExecutor().Execute(Context);
            Console.ToString().ShouldBeEmpty();
        }

        [Fact]
        public void TestList()
        {
            Context.Configuration.AddApp("my-app");
            Context.Configuration.AddService("my-service-c", "dummy-svc");
            Context.Configuration.AddService("my-service-a", "dummy-svc");
            Context.Configuration.AddService("my-service-b", "dummy-svc");
            ClearConsole();
            new ListExecutor().Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("my-service-a");
            reader.ReadLine().ShouldBe("my-service-b");
            reader.ReadLine().ShouldBe("my-service-c");
            reader.ReadLine().ShouldBe("my-app");
            reader.ReadLine().ShouldBeNull();
        }

        [Fact]
        public void TestListVerbose()
        {
            Context.Configuration.AddApp("my-app");
            Context.Configuration.AddService("my-service-c", "dummy-svc");
            Context.Configuration.AddService("my-service-a", "dummy-svc");
            Context.Configuration.AddService("my-service-b", "dummy-svc");
            ClearConsole();
            new ListExecutor(true).Execute(Context);
            var reader = new StringReader(Console.ToString());
            reader.ReadLine().ShouldBe("my-service-a      0  dummy-svc");
            reader.ReadLine().ShouldBe("my-service-b      0  dummy-svc");
            reader.ReadLine().ShouldBe("my-service-c      0  dummy-svc");
            reader.ReadLine().ShouldBe("my-app               app");
            reader.ReadLine().ShouldBeNull();
        }
    }
}
