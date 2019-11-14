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
using Steeltoe.Tooling.Executors;
using Xunit;

namespace Steeltoe.Tooling.Test.Executors
{
    public class SetArgsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestSetArgsUnknownAppOrService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new SetArgsExecutor("no-such-app-or-svc", "arg1").Execute(Context)
            );
            e.Name.ShouldBe("no-such-app-or-svc");
            e.Description.ShouldBe("app or service");
            Context.Configuration.GetApps().ShouldNotContain("no-such-app-or-svc");
            Context.Configuration.GetServices().ShouldNotContain("no-such-app-or-svc");
        }

        [Fact]
        public void TestSetArgsTargetUnknownAppOrService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new SetArgsExecutor("no-such-app-or-svc", "dummy-target", "arg1").Execute(Context)
            );
            e.Name.ShouldBe("no-such-app-or-svc");
            e.Description.ShouldBe("app or service");
            Context.Configuration.Apps.ShouldNotContainKey("no-such-app-or-svc");
            Context.Configuration.Services.ShouldNotContainKey("no-such-app-or-svc");
        }

        [Fact]
        public void TestSetAppArgs()
        {
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-app", "arg1 arg2").Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'my-app' app args to 'arg1 arg2'");
            Context.Configuration.Apps["my-app"].Args.ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestSetAppTargetArgs()
        {
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-app", "dummy-target", "arg1 arg2").Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'dummy-target' deploy args for 'my-app' app to 'arg1 arg2'");
            Context.Configuration.Apps["my-app"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestSetAppArgsForce()
        {
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-app", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetArgsExecutor("my-app", "arg3").Execute(Context)
            );
            e.Message.ShouldBe("'my-app' app args already set to 'arg1 arg2'");
            Context.Configuration.Apps["my-app"].Args.ShouldBe("arg1 arg2");
            ClearConsole();
            new SetArgsExecutor("my-app", "arg3", true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'my-app' app args to 'arg3'");
            Context.Configuration.Apps["my-app"].Args.ShouldBe("arg3");
        }

        [Fact]
        public void TestSetAppTargetArgsForce()
        {
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-app", "dummy-target", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetArgsExecutor("my-app", "dummy-target", "arg3").Execute(Context)
            );
            e.Message.ShouldBe("'dummy-target' deploy args for 'my-app' app already set to 'arg1 arg2'");
            Context.Configuration.Apps["my-app"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
            ClearConsole();
            new SetArgsExecutor("my-app", "dummy-target", "arg3", true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'dummy-target' deploy args for 'my-app' app to 'arg3'");
            Context.Configuration.Apps["my-app"].DeployArgs["dummy-target"].ShouldBe("arg3");
        }

        [Fact]
        public void TestSetAppTargetArgsUnknownTarget()
        {
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new SetArgsExecutor("my-app", "no-such-target", "arg1").Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
            Context.Configuration.Apps["my-app"].DeployArgs.ShouldNotContainKey("no-such-target");
        }

        [Fact]
        public void TestSetServiceArgs()
        {
            new AddServiceExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-service", "arg1 arg2").Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'my-service' dummy-svc service args to 'arg1 arg2'");
            Context.Configuration.Services["my-service"].Args.ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestSetServiceTargetArgs()
        {
            new AddServiceExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            Console.ToString().Trim()
                .ShouldBe("Set 'dummy-target' deploy args for 'my-service' dummy-svc service to 'arg1 arg2'");
            Context.Configuration.Services["my-service"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestSetServiceArgsForce()
        {
            new AddServiceExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-service", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetArgsExecutor("my-service", "arg3").Execute(Context)
            );
            e.Message.ShouldBe("'my-service' dummy-svc service args already set to 'arg1 arg2'");
            Context.Configuration.Services["my-service"].Args.ShouldBe("arg1 arg2");
            ClearConsole();
            new SetArgsExecutor("my-service", "arg3", true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Set 'my-service' dummy-svc service args to 'arg3'");
            Context.Configuration.Services["my-service"].Args.ShouldBe("arg3");
        }

        [Fact]
        public void TestSetServiceTargetArgsForce()
        {
            new AddServiceExecutor("my-service", "dummy-svc").Execute(Context);
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg1 arg2").Execute(Context);
            var e = Assert.Throws<ToolingException>(
                () => new SetArgsExecutor("my-service", "dummy-target", "arg3").Execute(Context)
            );
            e.Message.ShouldBe(
                "'dummy-target' deploy args for 'my-service' dummy-svc service already set to 'arg1 arg2'");
            Context.Configuration.Services["my-service"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
            ClearConsole();
            new SetArgsExecutor("my-service", "dummy-target", "arg3", true).Execute(Context);
            Console.ToString().Trim()
                .ShouldBe("Set 'dummy-target' deploy args for 'my-service' dummy-svc service to 'arg3'");
            Context.Configuration.Services["my-service"].DeployArgs["dummy-target"].ShouldBe("arg3");
        }

        [Fact]
        public void TestSetServiceTargetArgsUnknownTarget()
        {
            new AddServiceExecutor("my-service", "dummy-svc").Execute(Context);
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new SetArgsExecutor("my-service", "no-such-target", "arg1").Execute(Context)
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
            Context.Configuration.Services["my-service"].DeployArgs.ShouldNotContainKey("no-such-target");
        }
    }
}
