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

using System.IO;
using Shouldly;
using Steeltoe.Tooling.Dummy;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace Steeltoe.Tooling.Test.Dummy
{
    public class DummyDriverTest : ToolingTest
    {
        private readonly string _dbFile;

        private readonly DummyDriver _driver;

        public DummyDriverTest()
        {
            Directory.CreateDirectory(Context.ProjectDirectory);
            _dbFile = Path.Combine(Context.ProjectDirectory, "dummy.db");
            _driver = new DummyDriver(_dbFile);
        }

        [Fact]
        public void TestLifecyle()
        {
            File.Exists(_dbFile).ShouldBeTrue();

            // start state -> offline
            _driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Offline);

            // offline -> deploy -> starting -> online
            _driver.DeployService("my-service");
            _driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Starting);
            _driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Online);

            // online -> undeploy -> stopping -> offline
            _driver.UndeployService("my-service");
            _driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Stopping);
            _driver.GetServiceStatus("my-service").ShouldBe(Lifecycle.Status.Offline);
        }
    }
}
