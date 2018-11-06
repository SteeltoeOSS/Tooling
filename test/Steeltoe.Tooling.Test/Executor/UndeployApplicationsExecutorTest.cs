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
    public class UndeployApplicationsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestUndeployApplications()
        {
//            Context.ApplicationManager.AddApplication("an-app");
//            ClearConsole();
//            new UndeployApplicationsExecutor().Execute(Context);
//            Console.ToString().ShouldContain("Undeploying application 'an-app'");
//            Context.ApplicationManager.GetApplicationState("an-app").ShouldBe(Lifecycle.State.Offline);
        }

        [Fact]
        public void TestUndeployNoApplications()
        {
//            new UndeployApplicationsExecutor().Execute(Context);
//            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestUndeployNoTarget()
        {
//            Context.Configuration.EnvironmentName = null;
//            Context.Configuration.AddService("a-existing-service", "dummy-svc");
//            var e = Assert.Throws<ToolingException>(
//                () => new UndeployApplicationsExecutor().Execute(Context)
//            );
//            e.Message.ShouldBe("Target deployment environment not set");
        }
    }
}
