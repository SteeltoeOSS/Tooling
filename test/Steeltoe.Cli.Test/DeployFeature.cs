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
    public class DeployFeature : FeatureSpecs
    {
        [Scenario]
        public void DeployHelp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_help"),
                when => the_developer_runs_cli_command("deploy --help"),
                then => the_cli_should_output(new[]
                {
                    "Deploy apps and services to the target",
                    $"Usage: {Program.Name} deploy [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void DeployTooManyArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_too_many_args"),
                when => the_developer_runs_cli_command("deploy arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void DeployUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("deploy_uninitialized"),
                when => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void Deploy()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_services"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-a"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-b"),
                and => the_developer_runs_cli_command("-S deploy"),
                then => the_cli_should_output(new[]
                {
                    "Deploying service 'my-service-a'",
                    "Deploying service 'my-service-b'",
                    "Waiting for service 'my-service-a' to come online",
                    "Waiting for service 'my-service-b' to come online",
                    "Deploying app 'my-app'",
                })
            );
        }

        [Scenario]
        public void DeployNothing()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_nothing"),
                when => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_output_nothing()
            );
        }

        [Scenario]
        public void DeployNoTarget()
        {
            Runner.RunScenario(
                given => a_dotnet_project("deploy_no_target"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("add dummy-svc a-server"),
                and => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_error(ErrorCode.Tooling, "Target not set")
            );
        }
    }
}
