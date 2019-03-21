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

using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Cli.Test
{
    [Label("enable")]
    public class EnableFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void EnableHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("enable_help"),
                when => the_developer_runs_cli_command("enable --help"),
                then => the_cli_should_output("Enable a service."),
                and => the_cli_should_output("service Service name")
            );
        }

        [Scenario]
        public void EnableNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("enable_not_enough_args"),
                when => the_developer_runs_cli_command("enable"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void EnableTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("enable_too_many_args"),
                when => the_developer_runs_cli_command("enable arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void EnableService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("enable_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("disable my-service"),
                and => the_developer_runs_cli_command("enable my-service"),
                then => the_cli_should_output("Enabled service 'my-service'"),
                and => the_configuration_service_should_be_enabled("my-service")
            );
        }

        [Scenario]
        public void EnableNonExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("enable_non_existing_service"),
                when => the_developer_runs_cli_command("enable unknown-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'unknown-service' not found")
            );
        }
    }
}
