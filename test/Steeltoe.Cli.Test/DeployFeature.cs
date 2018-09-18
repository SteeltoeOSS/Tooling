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
    [Label("deploy")]
    public class DeployFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void DeployHelp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_help"),
                when => the_developer_runs_cli_command("deploy --help"),
                and => the_cli_should_output("Start an enabled service in the targeted deployment environment."),
                and => the_cli_should_output("name Service name")
            );
        }

        [Scenario]
        public void DeployNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("deploy_not_enough_args"),
                when => the_developer_runs_cli_command("deploy"),
                and => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void DeployTooManyArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_too_many_args"),
                when => the_developer_runs_cli_command("deploy arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void DeployService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("deploy my-service"),
                then => the_cli_should_output("Deployed service 'my-service'"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("starting"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("online")
            );
        }

        [Scenario]
        public void DeployNonExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_non_existing_service"),
                when => the_developer_runs_cli_command("deploy unknown-service"),
                and => the_cli_should_error(ErrorCode.Tooling, "Service 'unknown-service' not found")
            );
        }

        [Scenario]
        public void DeployDisabledService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_disabled_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("disable my-service"),
                and => the_developer_runs_cli_command("deploy my-service"),
                and => the_cli_should_error(ErrorCode.Tooling, "Invalid service status 'disabled'")
            );
        }
    }
}
