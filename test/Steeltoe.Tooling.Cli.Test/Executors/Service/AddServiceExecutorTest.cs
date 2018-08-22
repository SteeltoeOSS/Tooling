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
using Steeltoe.Tooling.Cli.Executors.Service;
using Xunit;

namespace Steeltoe.Tooling.Cli.Test.Executors.Service
{
    public class AddServiceExecutorTest : ExecutorTest
    {
        [Fact]
        public void TestAddUnknownTypeError()
        {
            var svc = new AddServiceExecutor("unknown-service", "unknown-service-type");
            var e = Assert.Throws<ArgumentException>(
                () => svc.Execute(Config, Shell, Output)
                );
            e.Message.ShouldBe("Unknown service type 'unknown-service-type'");
        }

        [Fact]
        public void TestAddExistingError()
        {
            Config.services.Add("existing-service", new Configuration.Service("existing-service-type"));
            var svc = new AddServiceExecutor("existing-service", "existing-service-type");
            var e = Assert.Throws<ArgumentException>(
                () => svc.Execute(Config, Shell, Output)
                );
            e.Message.ShouldBe("Service 'existing-service' already exists");
        }

        [Fact]
        public void TestAddCloudFoundryConfigServer()
        {
            var svc = new AddServiceExecutor("my-service", "cloud-foundry-config-server");
            svc.Execute(Config, Shell, Output);
            Output.ToString().ShouldContain("Added cloud-foundry-config-server service 'my-service'");
            Config.services.ShouldContainKey("my-service");
            Config.services["my-service"].type.ShouldBe("cloud-foundry-config-server");
        }
    }
}
