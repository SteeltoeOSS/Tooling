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
using Steeltoe.Tooling.Executor.Target;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor.Target
{
    public class ListTargetsExecutorTest : ExecutorTest
    {
        [Fact]
        public void TestList()
        {
            var svc = new ListTargetsExecutor();
            svc.Execute(Config, null, Output);
            Output.ToString().ShouldContain("cloud-foundry");
        }
    }
}
