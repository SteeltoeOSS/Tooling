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
    [Label("undeploy")]
    public class UndeployFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void UndeployHelp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_help"),
                when => the_developer_runs_cli_command("undeploy --help"),
                and => the_cli_should_output("Stop an enabled service in the targeted deployment environment."),
                and => the_cli_should_output("name Service name")
            );
        }

        [Scenario]
        public void UndeployNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undeploy_not_enough_args"),
                when => the_developer_runs_cli_command("undeploy"),
                and => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void UndeployTooManyArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_too_many_args"),
                when => the_developer_runs_cli_command("undeploy arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void UndeployService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("undeploy_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("deploy my-service"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("starting"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("online"),
                when => the_developer_runs_cli_command("undeploy my-service"),
                then => the_cli_should_output("Undeployed service 'my-service'"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("stopping"),
                when => the_developer_runs_cli_command("status my-service"),
                then => the_cli_should_output("offline")
            );
        }
    }
}
