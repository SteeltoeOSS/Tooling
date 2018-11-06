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

using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Cli.Test
{
    public class StatusFeature : FeatureSpecs
    {
        [Scenario]
        public void StatusHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_help"),
                when => the_developer_runs_cli_command("status --help"),
                then => the_cli_should_output(new[]
                {
                    "Show app and service statuses",
                    $"Usage: {Program.Name} status [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void StatusTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_too_many_args"),
                when => the_developer_runs_cli_command("status arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void StatusUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_uninitialized"),
                when => the_developer_runs_cli_command("status"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void StatusServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("status_services"),
                when => the_developer_runs_cli_command("add dummy-svc my-service-a"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-b"),
                and => the_developer_runs_cli_command("-S status"),
                then => the_cli_should_output(new[]
                {
                    "my-service-a offline",
                    "my-service-b offline",
                })
            );
        }

        [Scenario]
        public void StatusNoServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("status_no_services"),
                when => the_developer_runs_cli_command("status"),
                then => the_cli_should_output_nothing()
            );
        }

        [Scenario]
        public void StatusNoTarget()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_no_target"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("add dummy-svc a-server"),
                and => the_developer_runs_cli_command("status"),
                then => the_cli_should_error(ErrorCode.Tooling, "Target not set")
            );
        }
    }
}
