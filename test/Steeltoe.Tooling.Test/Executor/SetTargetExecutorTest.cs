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
    public class SetTargetExecutorTest : ToolingTest
    {
        [Fact]
        public void TestSetTarget()
        {
            Context.Configuration.Target = null;
            new SetTargetExecutor("dummy-target").Execute(Context);
            Console.ToString().Trim().ShouldContain("Target set to 'dummy-target'");
            Context.Configuration.Target.ShouldBe("dummy-target");
        }

        [Fact]
        public void TestSetUnknownTarget()
        {
            var e = Assert.Throws<ToolingException>(
                () => new SetTargetExecutor("no-such-target").Execute(Context)
            );
            e.Message.ShouldBe("Unknown target 'no-such-target'");
        }
    }
}
