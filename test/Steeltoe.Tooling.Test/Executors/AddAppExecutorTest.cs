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
            new AddAppExecutor("my-app", "dummy-framework", "dummy-runtime").Execute(Context);
            Console.ToString().Trim().ShouldBe("Added app 'my-app' (dummy-framework/dummy-runtime)");
            Context.Configuration.GetApps().Count.ShouldBe(1);
            Context.Configuration.GetApps()[0].ShouldBe("my-app");
            var appInfo = Context.Configuration.GetAppInfo("my-app");
            appInfo.App.ShouldBe("my-app");
        }

        [Fact]
        public void TestAddExistingApp()
        {
            Context.Configuration.AddApp("existing-app", "dummy-framework", "dummy-runtime");
            var e = Assert.Throws<ItemExistsException>(
                () => new AddAppExecutor("existing-app", "dummy-framework", "dummy-runtime").Execute(Context)
            );
            e.Name.ShouldBe("existing-app");
            e.Description.ShouldBe("app");
        }
    }
}
