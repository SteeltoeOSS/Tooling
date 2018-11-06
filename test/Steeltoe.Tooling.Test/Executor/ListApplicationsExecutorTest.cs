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

using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class ListApplicationsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestListNone()
        {
//            new ListApplicationsExecutor().Execute(Context);
//            Console.ToString().ShouldBeEmpty();
        }

        [Fact]
        public void TestListApplications()
        {
//            Context.ApplicationManager.AddApplication("a-app");
//            Context.ApplicationManager.AddApplication("c-app");
//            Context.ApplicationManager.AddApplication("b-app");
//            ClearConsole();
//            new ListApplicationsExecutor().Execute(Context);
//            var reader = new StringReader(Console.ToString());
//            reader.ReadLine().ShouldBe("a-app");
//            reader.ReadLine().ShouldBe("b-app");
//            reader.ReadLine().ShouldBe("c-app");
//            reader.ReadLine().ShouldBeNull();
        }

        [Fact]
        public void TestListApplicationsVerbose()
        {
//            Context.ApplicationManager.AddApplication("a-app");
//            Context.ApplicationManager.AddApplication("c-app");
//            Context.ApplicationManager.AddApplication("b-app");
//            ClearConsole();
//            new ListApplicationsExecutor(true).Execute(Context);
//            var reader = new StringReader(Console.ToString());
//            reader.ReadLine().ShouldBe("a-app         application");
//            reader.ReadLine().ShouldBe("b-app         application");
//            reader.ReadLine().ShouldBe("c-app         application");
//            reader.ReadLine().ShouldBeNull();
        }
    }
}
