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
    public class ListServiceTypesExecutorTest : ToolingTest
    {
        [Fact]
        public void TestListServiceTypes()
        {
            var expected = new[] {"dummy-svc", "config-server", "registry"};
            new ListServiceTypesExecutor().Execute(Context);
            foreach (var env in expected)
            {
                Console.ToString().ShouldContain(env);
            }
        }
    }
}
