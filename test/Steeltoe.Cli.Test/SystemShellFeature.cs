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
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
    public partial class SystemShellFeature
    {
        [Scenario]
        public void RunCommand()
        {
            Runner.RunScenario(
                when => the_command_is_run(ListCommand),
                then => the_command_should_succeed(),
                and => the_command_output_should_not_be_empty()
            );
        }

        [Scenario]
        public void RunCommandWithArgs()
        {
            string sandbox = Path.Combine("sandboxes", "run_command_with_args");
            Runner.RunScenario(
                given => a_directory(sandbox),
                and => a_file(Path.Combine(sandbox, "aFile")),
                when => the_command_is_run_with_args(ListCommand, sandbox),
                then => the_command_should_succeed(),
                and => the_command_output_should_contain("aFile")
            );
        }

        [Scenario]
        public void RunCommandWithWorkingDirectory()
        {
            string sandbox = Path.Combine("sandboxes", "run_command_with_working_directory");
            Runner.RunScenario(
                given => a_directory(sandbox),
                and => a_file(Path.Combine(sandbox, "aFile")),
                when => the_command_is_run_with_working_directory(ListCommand, sandbox),
                then => the_command_should_succeed(),
                and => the_command_output_should_contain("aFile")
            );
        }

        [Scenario]
        public void RunCommandThatErrors()
        {
            Runner.RunScenario(
                when => the_command_is_run(ErrorCommand),
                then => the_command_should_fail()
            );
        }

        [Scenario]
        public void RunNoSuchCommand()
        {
            Runner.RunScenario(
                when => the_command_is_run("no-such-command"),
                then => the_command_should_raise_exception<ShellException>()
            );
        }
    }
}
