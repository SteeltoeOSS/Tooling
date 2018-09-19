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
        public void TestListSeveral()
        {
            Context.Configuration.Services.Add("a-service", new Configuration.Service {ServiceTypeName = "a-service-type"});
            Context.Configuration.Services.Add("another-service",
                new Configuration.Service {ServiceTypeName = "another-service-type"});
            new ListServicesExecutor().Execute(Context);
            Console.ToString().ShouldContain("a-service (a-service-type)");
            Console.ToString().ShouldContain("another-service (another-service-type)");
        }
    }
}
