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
    public class AddExecutorTest : ToolingTest
    {
        [Fact]
        public void TestAdd()
        {
            new AddExecutor("my-app").Execute(Context);
            Console.ToString().Trim().ShouldBe("Added app 'my-app'");
            Context.Configuration.GetApps().Count.ShouldBe(1);
            Context.Configuration.GetApps()[0].ShouldBe("my-app");
            var appInfo = Context.Configuration.GetAppInfo("my-app");
            appInfo.App.ShouldBe("my-app");
        }

        [Fact]
        public void TestAddExistingApp()
        {
            Context.Configuration.AddApp("existing-app");
            var e = Assert.Throws<ItemExistsException>(
                () => new AddExecutor("existing-app").Execute(Context)
            );
            e.Name.ShouldBe("existing-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestAddService()
        {
            new AddExecutor("my-service", "dummy-svc").Execute(Context);
            Console.ToString().Trim().ShouldBe("Added dummy-svc service 'my-service'");
            Context.Configuration.GetServices().Count.ShouldBe(1);
            var svcName = Context.Configuration.GetServices()[0];
            svcName.ShouldBe("my-service");
            var svcInfo = Context.Configuration.GetServiceInfo(svcName);
            svcInfo.Service.ShouldBe("my-service");
            svcInfo.ServiceType.ShouldBe("dummy-svc");
        }

        [Fact]
        public void TestAddExistingService()
        {
            Context.Configuration.AddService("existing-service", "dummy-svc");
            var e = Assert.Throws<ItemExistsException>(
                () => new AddExecutor("existing-service", "dummy-svc").Execute(Context)
            );
            e.Name.ShouldBe("existing-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestAddUnknownServiceType()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => new AddExecutor("unknown-service", "unknown-service-type").Execute(Context)
            );
            e.Name.ShouldBe("unknown-service-type");
            e.Description.ShouldBe("service type");
        }
    }
}
