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
    [Label("remove")]
    public class RemoveFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void RemoveHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_help"),
                when => the_developer_runs_cli_command("remove --help"),
                then => the_cli_should_output("Remove a service."),
                and => the_cli_should_output("service Service name")
            );
        }

        [Scenario]
        public void RemoveNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_not_enough_args"),
                when => the_developer_runs_cli_command("remove"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void RemoveTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_too_many_args"),
                when => the_developer_runs_cli_command("remove arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void RemoveUninitializedProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_uninitialized_project"),
                when => the_developer_runs_cli_command("remove my-service"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "Project has not been initialized for Steeltoe Developer Tools")
            );
        }

        [Scenario]
        public void RemoveService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("remove_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("remove my-service"),
                then => the_cli_should_output("Removed service 'my-service'"),
                and => the_configuration_should_not_contain_service("my-service")
            );
        }

        [Scenario]
        public void RemoveNonExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("remove_non_existing_service"),
                when => the_developer_runs_cli_command("remove unknown-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'unknown-service' not found")
            );
        }
    }
}
