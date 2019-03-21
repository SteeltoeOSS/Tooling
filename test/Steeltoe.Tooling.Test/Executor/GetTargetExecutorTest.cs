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
    public class GetTargetExecutorTest : ToolingTest
    {
        [Fact]
        public void TestShowTarget()
        {
            Context.Configuration.EnvironmentName = "my-environment";
            new GetTargetExecutor().Execute(Context);
            Console.ToString().Trim().ShouldBe("Target deployment environment set to 'my-environment'.");
        }

        [Fact]
        public void TestShowNoTarget()
        {
            Context.Configuration.EnvironmentName = null;
            var svc = new GetTargetExecutor();
            var e = Assert.Throws<ToolingException>(
                () => svc.Execute(Context)
            );
            e.Message.ShouldBe("Target deployment environment not set");
        }
    }
}
