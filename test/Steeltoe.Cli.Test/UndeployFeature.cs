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

using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Cli.Test
{
    public class UndeployFeature : FeatureSpecs
    {
        [Scenario]
        public void UndeployHelp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_help"),
                when => the_developer_runs_cli_command("undeploy --help"),
                then => the_cli_should_output(new[]
                {
                    "Undeploy apps and services from the target",
                    $"Usage: {Program.Name} undeploy [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void UndeployTooManyArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_too_many_args"),
                when => the_developer_runs_cli_command("undeploy arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void UndeployUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undeploy_uninitialized"),
                when => the_developer_runs_cli_command("undeploy"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void Undeploy()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_services"),
                when => the_developer_runs_cli_command("add-app my-app"),
                and => the_developer_runs_cli_command("add-service dummy-svc my-service-a"),
                and => the_developer_runs_cli_command("add-service dummy-svc my-service-b"),
                and => the_developer_runs_cli_command("deploy"),
                and => the_developer_runs_cli_command("status"),
                and => the_developer_runs_cli_command("undeploy"),
                then => the_cli_should_output(new[]
                {
                    "Undeploying app 'my-app'",
                    "Undeploying service 'my-service-a'",
                    "Undeploying service 'my-service-b'",
                    "Waiting for 'my-service-a' to transition to offline (1)",
                    "Waiting for 'my-service-b' to transition to offline (1)",
                })
            );
        }

        [Scenario]
        public void DeployNothing()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_nothing"),
                when => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_output_nothing()
            );
        }

        [Scenario]
        public void UndeployNoTarget()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undeploy_no_target"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("add dummy-svc a-server"),
                and => the_developer_runs_cli_command("undeploy"),
                then => the_cli_should_error(ErrorCode.Tooling, "Target not set")
            );
        }
    }
}
