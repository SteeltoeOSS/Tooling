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

namespace Steeltoe.Tooling.Base.Test
{
    public class ToolingConfigurationTest
    {
        private static readonly string Sandbox;

        private static readonly string MyConfig;

        static ToolingConfigurationTest()
        {
            Sandbox = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "unit-tests"), "tooling-configuration");
            Directory.CreateDirectory(Sandbox);
            MyConfig = Path.Combine(Sandbox, ".steeltoe.tooling.yml");
            File.WriteAllText(MyConfig, @"target: myTarget
");
        }

        [Fact]
        public void TestStoreToStream()
        {
            var ostr = new StringWriter();
            var cfg = new ToolingConfiguration();
            cfg.target = "myTarget";
            var svc = new ToolingConfiguration.Service();
            svc.type = "myServiceType";
            cfg.services.Add("myService", svc);
            cfg.Store(ostr);
            ostr.ToString().ShouldMatch(@"\s*target: myTarget\s+services:\s+myService:\s+type:\s+myServiceType\s*");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfgFile = Path.Combine(Sandbox, "stored.yml");
            var cfg = new ToolingConfiguration();
            cfg.target = "StoredTarget";
            cfg.Store(cfgFile);
            File.Exists(cfgFile).ShouldBeTrue();
            File.ReadAllText(cfgFile).ShouldContain("StoredTarget");
        }

        [Fact]
        public void TestStoreToDirectory()
        {
            var customDir = Path.Combine(Sandbox, "custom-dir");
            Directory.CreateDirectory(customDir);
            var cfg = new ToolingConfiguration();
            cfg.target = "CustomDirTarget";
            cfg.Store(customDir);
            var expectedCfgFile = Path.Combine(customDir, ".steeltoe.tooling.yml");
            File.Exists(expectedCfgFile).ShouldBeTrue();
            File.ReadAllText(expectedCfgFile).ShouldContain("CustomDirTarget");
        }

        [Fact]
        public void TestLoadFromReader()
        {
            var istr = new StringReader(@"target: myTarget
services:
  myService:
    type: myServiceType");
            var cfg = ToolingConfiguration.Load(istr);
            cfg.target.ShouldBe("myTarget");
        }

        [Fact]
        public void TestLoadFromFile()
        {
            ToolingConfiguration.Load(MyConfig).target.ShouldBe("myTarget");
        }

        [Fact]
        public void TestLoadFromDirectory()
        {
            ToolingConfiguration.Load(Sandbox).target.ShouldBe("myTarget");
        }
    }
}
