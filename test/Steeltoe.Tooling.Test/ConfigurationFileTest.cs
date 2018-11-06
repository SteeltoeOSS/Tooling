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

using System.IO;
using Shouldly;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class ConfigurationFileTest : ToolingTest
    {
        [Fact]
        public void TestLoadFromFile()
        {
            var file = Path.Combine(Context.ProjectDirectory, "config-file");
            File.WriteAllText(file, SampleConfig);
            var cfgFile = new ConfigurationFile(file);
            cfgFile.Configuration.Target.ShouldBe("dummy-target");
            cfgFile.Configuration.GetServices().ShouldContain("my-service");
            cfgFile.Configuration.GetServiceInfo("my-service").ServiceType.ShouldBe("dummy-svc");
        }

        [Fact]
        public void TestLoadFromDirectory()
        {
            var defaultFile = Path.Combine(Context.ProjectDirectory, ConfigurationFile.DefaultFileName);
            File.WriteAllText(defaultFile, SampleConfig);
            var cfgFile = new ConfigurationFile(Context.ProjectDirectory);
            cfgFile.Configuration.Target.ShouldBe("dummy-target");
            cfgFile.Configuration.GetServices().ShouldContain("my-service");
            cfgFile.Configuration.GetServiceInfo("my-service").ServiceType.ShouldBe("dummy-svc");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var file = Path.Combine(Context.ProjectDirectory, "config-file");
            var cfgFile = new ConfigurationFile(file);
            cfgFile.Configuration.Target = "dummy-target";
            cfgFile.Configuration.AddApp("my-app");
            cfgFile.Configuration.AddService("my-service", "dummy-svc");
            cfgFile.Store();
            File.ReadAllText(file).ShouldBe(SampleConfig);
        }

        [Fact]
        public void TestStoreToDirectory()
        {
            var cfgFile = new ConfigurationFile(Context.ProjectDirectory);
            cfgFile.Configuration.Target = "dummy-target";
            cfgFile.Configuration.AddApp("my-app");
            cfgFile.Configuration.AddService("my-service", "dummy-svc");
            cfgFile.Store();
            var defaultFile = Path.Combine(Context.ProjectDirectory, ConfigurationFile.DefaultFileName);
            File.ReadAllText(defaultFile).ShouldBe(SampleConfig);
        }

        private const string SampleConfig = @"target: dummy-target
apps:
  my-app: {}
services:
  my-service:
    type: dummy-svc
    args: {}
";
    }
}
