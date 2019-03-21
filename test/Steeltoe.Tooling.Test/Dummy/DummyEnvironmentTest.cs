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
using Steeltoe.Tooling.Dummy;
using Xunit;

namespace Steeltoe.Tooling.Test.Dummy
{
    public class DummyEnvironmentTest
    {
        private readonly DummyEnvironment _env = new DummyEnvironment();

        [Fact]
        public void TestGetName()
        {
            _env.Name.ShouldBe("dummy-env");
        }

        [Fact]
        public void TestGetDescription()
        {
            _env.Description.ShouldBe("A dummy environment for testing Steeltoe Developer Tools");
        }
    }
}
