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
    [Label("disable")]
    public class DisableFeature : ServiceFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void DisableHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("disable_help"),
                when => the_developer_runs_cli_command("disable --help"),
                then => the_cli_should_output("Disable a service."),
                and => the_cli_should_output("name Service name")
            );
        }

        [Scenario]
        public void DisableNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("disable_not_enough_args"),
                when => the_developer_runs_cli_command("disable"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void DisableTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("disable_too_many_args"),
                when => the_developer_runs_cli_command("disable arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void DisableService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("disable_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                and => the_developer_runs_cli_command("disable my-service"),
                then => the_cli_should_output("disabled service 'my-service'"),
                and => the_configuration_service_should_not_be_enabled("my-service")
            );
        }

        [Scenario]
        public void DisableNonExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("disable_non_existing_service"),
                when => the_developer_runs_cli_command("disable unknown-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'unknown-service' not found")
            );
        }
    }
}
