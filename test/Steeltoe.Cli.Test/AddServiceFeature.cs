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
    public class AddServiceFeature : FeatureSpecs
    {
        [Scenario]
        public void AddServiceHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_help"),
                when => the_developer_runs_cli_command("add-service --help"),
                then => the_cli_should_output(new[]
                {
                    "Add a service",
                    $"Usage: {Program.Name} add-service [arguments] [options]",
                    "Arguments:",
                    "type Service type",
                    "name Service name",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void AddServiceNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_not_enough"),
                when => the_developer_runs_cli_command("add-service"),
                then => the_cli_should_error(ErrorCode.Argument, "Service type not specified")
            );
            Console.Clear();
            Runner.RunScenario(
                given => a_dotnet_project("add_service_not_enough_args1"),
                when => the_developer_runs_cli_command("add-service arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void AddServiceTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_too_many_args"),
                when => the_developer_runs_cli_command("add-service arg1 arg2 arg3"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg3'")
            );
        }

        [Scenario]
        public void AddServiceUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_uninitialized"),
                when => the_developer_runs_cli_command("add-service dummy-svc my-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void AddUnknownServiceType()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_service_unknown_type"),
                when => the_developer_runs_cli_command("add-service no-such-service-type foo"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service type 'no-such-service-type' does not exist")
            );
        }

        [Scenario]
        public void AddService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_service"),
                when => the_developer_runs_cli_command("add-service dummy-svc my-service"),
                then => the_cli_should_output("Added dummy-svc service 'my-service'"),
                and => the_configuration_should_contain_service("my-service", "dummy-svc")
            );
        }

        [Scenario]
        public void AddExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_service_existing_service"),
                when => the_developer_runs_cli_command("add-service dummy-svc existing-service"),
                and => the_developer_runs_cli_command("add-service dummy-svc existing-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'existing-service' already exists")
            );
        }
    }
}
