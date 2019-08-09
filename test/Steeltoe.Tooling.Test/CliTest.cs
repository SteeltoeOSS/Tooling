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
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class CliTest
    {
        [Fact]
        public void TestRun()
        {
            var shell = new MockShell();
            shell.AddResponse("output of mycommand");
            var cli = new Cli("mycommand", shell);
            var output = cli.Run("arg1 arg2", null);
            shell.LastCommand.ShouldBe("mycommand arg1 arg2");
            output.ShouldBe("output of mycommand");
            shell.AddResponse("some error message", 3);
            var e = Assert.Throws<CliException>(
                () => cli.Run("bad args", null)
            );
            e.ReturnCode.ShouldBe(3);
            e.Command.ShouldBe("mycommand bad args");
            e.Error.ShouldBe("some error message");
            e.Message.ShouldBe("some error message [rc=3,cmd='mycommand bad args']");
        }
    }
}
