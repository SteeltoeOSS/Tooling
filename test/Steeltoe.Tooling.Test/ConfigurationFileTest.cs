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

using System;
using System.IO;
using System.Linq;
using Shouldly;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class ConfigurationFileTest : IDisposable
    {
        private readonly string _testDir;

        private readonly string _testConfigPath;

        private readonly string _defaultConfigPath;

        public ConfigurationFileTest()
        {
            _testDir = new[] {Path.GetTempPath(), "Steeltoe.Tooling.Test", Guid.NewGuid().ToString()}.Aggregate(Path.Combine);
            Directory.CreateDirectory(_testDir);
            _testConfigPath = Path.Combine(_testDir, "config-file");
            _defaultConfigPath = Path.Combine(_testDir, ConfigurationFile.DefaultFileName);
        }

        public void Dispose()
        {
            Directory.Delete(_testDir, true);
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfg = new ConfigurationFile(_testConfigPath);
            cfg.EnvironmentName = "myEnvironment";
            cfg.Services.Add("myService", new Configuration.Service
            {
                ServiceTypeName = "myServiceType",
                Enabled = true
            });
            cfg.Store();
            File.ReadAllText(_testConfigPath).ShouldBe(SampleConfig);
        }

        [Fact]
        public void TestStoreToDirectory()
        {
            var cfg = new ConfigurationFile(_testDir);
            cfg.EnvironmentName = "myEnvironment";
            cfg.Services.Add("myService", new Configuration.Service
            {
                ServiceTypeName = "myServiceType",
                Enabled = true
            });
            cfg.Store();
            File.ReadAllText(_defaultConfigPath).ShouldBe(SampleConfig);
        }

        [Fact]
        public void TestLoadFromFile()
        {
            File.WriteAllText(_testConfigPath, SampleConfig);
            var cfg = new ConfigurationFile(_testConfigPath);
            cfg.EnvironmentName.ShouldBe("myEnvironment");
            cfg.Services.ShouldContainKey("myService");
            cfg.Services["myService"].ServiceTypeName.ShouldBe("myServiceType");
            cfg.Services["myService"].Enabled.ShouldBeTrue();
        }

        [Fact]
        public void TestLoadFromDirectory()
        {
            File.WriteAllText(_defaultConfigPath, SampleConfig);
            var cfg = new ConfigurationFile(_testDir);
            cfg.EnvironmentName.ShouldBe("myEnvironment");
            cfg.Services.ShouldContainKey("myService");
            cfg.Services["myService"].ServiceTypeName.ShouldBe("myServiceType");
            cfg.Services["myService"].Enabled.ShouldBeTrue();
        }

        private const string SampleConfig = @"environment: myEnvironment
services:
  myService:
    type: myServiceType
    enabled: true
    args: {}
";
    }
}
