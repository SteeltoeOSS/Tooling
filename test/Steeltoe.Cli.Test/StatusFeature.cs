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
    [Label("status")]
    public class StatusFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void StatusHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_help"),
                when => the_developer_runs_cli_command("status --help"),
                then => the_cli_should_output("Show service statuses.")
            );
        }

        [Scenario]
        public void StatusTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_too_many_args"),
                when => the_developer_runs_cli_command("status arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void StatusUninitializedProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_uninitialized_project"),
                when => the_developer_runs_cli_command("status"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "Project has not been initialized for Steeltoe Developer Tools")
            );
        }

        [Scenario]
        public void StatusServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("status_services"),
                when => the_developer_runs_cli_command("add a-service dummy-svc"),
                when => the_developer_runs_cli_command("add defunct-service dummy-svc"),
                and => the_developer_runs_cli_command("disable defunct-service"),
                and => the_developer_runs_cli_command("status"),
                then => the_cli_should_output("a-service offline"),
                and => the_cli_should_output("defunct-service disabled"),
                when => the_developer_runs_cli_command("deploy"),
                and => the_developer_runs_cli_command("status"),
                then => the_cli_should_output("a-service starting"),
                and => the_cli_should_output("defunct-service disabled"),
                when => the_developer_runs_cli_command("undeploy"),
                and => the_developer_runs_cli_command("status"),
                then => the_cli_should_output("a-service stopping"),
                and => the_cli_should_output("defunct-service disabled")
            );
        }

        [Scenario]
        public void StatusNoServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("status_no_services"),
                when => the_developer_runs_cli_command("status"),
                then => the_cli_should_output("No services have been added")
            );
        }
    }
}
