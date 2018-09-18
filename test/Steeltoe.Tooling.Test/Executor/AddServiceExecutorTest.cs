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
    public class AddServiceExecutorTest : ToolingTest
    {
        [Fact]
        public void TestAddService()
        {
            new AddServiceExecutor("a-service", "dummy-svc").Execute(Context);
            Console.ToString().Trim().ShouldBe("Added dummy-svc service 'a-service'");
            Context.ServiceManager.GetServiceNames().Count.ShouldBe(1);
            var svcName = Context.ServiceManager.GetServiceNames()[0];
            svcName.ShouldBe("a-service");
            var svc = Context.ServiceManager.GetService(svcName);
            svc.Type.ShouldBe("dummy-svc");
            Context.ServiceManager.GetServiceStatus("a-service").ShouldBe("offline");
        }

        [Fact]
        public void TestAddPreExistingService()
        {
            Context.ServiceManager.AddService("pre-existing-service", "dummy-svc");
            var executor = new AddServiceExecutor("pre-existing-service", "dummy-svc");
            var e = Assert.Throws<ToolingException>(
                () => executor.Execute(Context)
            );
            e.Message.ShouldBe("Service 'pre-existing-service' already exists");
        }

        [Fact]
        public void TestAddUnknownServiceType()
        {
            var executor = new AddServiceExecutor("unknown-service", "unknown-service-type");
            var e = Assert.Throws<ToolingException>(
                () => executor.Execute(Context)
            );
            e.Message.ShouldBe("Unknown service type 'unknown-service-type'");
        }
    }
}
