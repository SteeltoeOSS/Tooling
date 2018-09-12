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
    public class ConfigurationTest
    {
        [Fact]
        public void TestStoreToStream()
        {
            var cfg = new Configuration();
            cfg.environment = "myEnvironment";
            cfg.services.Add("myService", new Configuration.Service("myServiceType"));
            var ostr = new StringWriter();
            cfg.Store(ostr);
            ostr.ToString().ShouldBe(SampleConfig);

        }

        [Fact]
        public void TestLoadFromStream()
        {
            var istr = new StringReader(SampleConfig);
            var cfg = Configuration.Load(istr);
            cfg.environment.ShouldBe("myEnvironment");
            cfg.services.ShouldContainKey("myService");
            cfg.services["myService"].type.ShouldBe("myServiceType");
        }

        private const string SampleConfig = @"environment: myEnvironment
services:
  myService:
    type: myServiceType
";
    }
}
