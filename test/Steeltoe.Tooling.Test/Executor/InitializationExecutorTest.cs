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
using Steeltoe.Tooling.Executor;
using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class InitializationExecutorTest : ToolingTest
    {
        [Fact]
        public void TestInitialization()
        {
            new InitializationExecutor().Execute(Context);
            File.Exists(Path.Join(Context.ProjectDirectory, ConfigurationFile.DefaultFileName)).ShouldBeTrue();
            Console.ToString().Trim().ShouldBe("Initialized Steeltoe Developer Tools");
        }

        [Fact]
        public void TestInitializationForce()
        {
            var file = "config-file";
            new ConfigurationFile(Path.Combine(Context.ProjectDirectory, file)).Store();
            var e = Assert.Throws<ToolingException>(
                () => new InitializationExecutor(file).Execute(Context)
            );
            e.Message.ShouldBe("Steeltoe Developer Tools already initialized");
            new InitializationExecutor(file, true).Execute(Context);
            Console.ToString().Trim().ShouldBe("Initialized Steeltoe Developer Tools");
        }
    }
}
