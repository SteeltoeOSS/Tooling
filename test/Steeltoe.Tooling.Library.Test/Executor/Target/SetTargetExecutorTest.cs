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

using System;
using Shouldly;
using Steeltoe.Tooling.Executor.Target;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor.Target
{
    public class SetTargetExecutorTest : ExecutorTest
    {
        [Fact]
        public void TestSetUnknown()
        {
            var svc = new SetTargetExecutor("unknown-environment");
            var e = Assert.Throws<ArgumentException>(
                () => svc.Execute(Config, Shell, Output)
                );
            e.Message.ShouldBe("Unknown environment 'unknown-environment'");
        }

        [Fact]
        public void TestSetCloudFoundry()
        {
            var svc = new SetTargetExecutor("cloud-foundry");
            svc.Execute(Config, Shell, Output);
            Output.ToString().ShouldContain("Target environment set to 'cloud-foundry'.");
            Config.environment.ShouldBe("cloud-foundry");
        }

        [Fact]
        public void TestSetDocker()
        {
            var svc = new SetTargetExecutor("docker");
            svc.Execute(Config, Shell, Output);
            Output.ToString().ShouldContain("Target environment set to 'docker'.");
            Config.environment.ShouldBe("docker");
        }
    }
}
