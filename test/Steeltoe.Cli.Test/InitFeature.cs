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

using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Cli.Test
{
    [Label("init")]
    public class InitFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void InitHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("init_help"),
                when => the_developer_runs_cli_command("init --help"),
                then => the_cli_should_output(new[]
                {
                    "Initialize a project for Steeltoe Developer Tools.",
                    $"Usage: {Program.Name} init [options]",
                    "Options:",
                    "-F|--force Initialize the project even if already initialized",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void InitTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("init_too_many_args"),
                when => the_developer_runs_cli_command("init arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void InitProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("init_project"),
                when => the_developer_runs_cli_command("init"),
                then => the_cli_should_output("Project initialized for Steeltoe Developer Tools")
            );
        }

        [Scenario]
        public void InitProjectForce()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("init_project_already_initialized"),
                when => the_developer_runs_cli_command("init"),
                then => the_cli_should_error(ErrorCode.Tooling, "Project already initialized"),
                when => the_developer_runs_cli_command("init --force"),
                then => the_cli_should_output("Project initialized for Steeltoe Developer Tools")
            );
        }
    }
}
