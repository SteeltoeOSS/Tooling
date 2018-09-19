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
    [Label("add")]
    public class AddFeature : ServiceFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void AddHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_help"),
                when => the_developer_runs_cli_command("add --help"),
                then => the_cli_should_output("Add a service."),
                and => the_cli_should_output("name Service name"),
                and => the_cli_should_output(
                    "type Service type (run 'steeltoe list types' for available service types)")
            );
        }

        [Scenario]
        public void AddNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough_args0"),
                when => the_developer_runs_cli_command("add"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough_args1"),
                when => the_developer_runs_cli_command("add myservice"),
                then => the_cli_should_error(ErrorCode.Argument, "Service type not specified")
            );
        }

        [Scenario]
        public void AddTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_too_many_args"),
                when => the_developer_runs_cli_command("add arg1 arg2 arg3"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg3'")
            );
        }

        [Scenario]
        public void AddUnknownServiceType()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_unknown_service_type"),
                when => the_developer_runs_cli_command("add foo no-such-type"),
                then => the_cli_should_error(ErrorCode.Tooling, "Unknown service type 'no-such-type'")
            );
        }

        [Scenario]
        public void AddService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_service"),
                when => the_developer_runs_cli_command("add my-service dummy-svc"),
                then => the_cli_should_output("Added dummy-svc service 'my-service'"),
                and => the_configuration_should_contain_service("my-service"),
                and => the_configuration_service_should_be_enabled("my-service")
            );
        }

        [Scenario]
        public void AddPreExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_pre_existing_service"),
                when => the_developer_runs_cli_command("add existing-service dummy-svc"),
                and => the_developer_runs_cli_command("add existing-service dummy-svc"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'existing-service' already exists")
            );
        }
    }
}
