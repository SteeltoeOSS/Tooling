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
    public class AddFeature : FeatureSpecs
    {
        [Scenario]
        public void AddHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_help"),
                when => the_developer_runs_cli_command("add --help"),
                then => the_cli_should_output(new[]
                {
                    "Add an app or service",
                    $"Usage: {Program.Name} add [arguments] [options]",
                    "Arguments:",
                    "type 'app' or service type",
                    "name App or service name",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void AddNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough"),
                when => the_developer_runs_cli_command("add"),
                then => the_cli_should_error(ErrorCode.Argument, "'app' or service type not specified")
            );
            Console.Clear();
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough_args1"),
                when => the_developer_runs_cli_command("add arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "App or service name not specified")
            );
        }

        [Scenario]
        public void AddTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_too_many_args"),
                when => the_developer_runs_cli_command("add arg1 arg2 arg3"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg3'")
            );
        }

        [Scenario]
        public void AddUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_uninitialized"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void AddUnknownServiceType()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_unknown_type"),
                when => the_developer_runs_cli_command("add no-such-service-type foo"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service type 'no-such-service-type' does not exist")
            );
        }

        [Scenario]
        public void AddApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_app"),
                when => the_developer_runs_cli_command("add app my-app"),
                then => the_cli_should_output("Added app 'my-app'"),
                and => the_configuration_should_contain_app("my-app")
            );
        }

        [Scenario]
        public void AddExistingApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_existing_app"),
                when => the_developer_runs_cli_command("add app existing-app"),
                and => the_developer_runs_cli_command("add app existing-app"),
                then => the_cli_should_error(ErrorCode.Tooling, "App 'existing-app' already exists")
            );
        }

        [Scenario]
        public void AddService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_service"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                then => the_cli_should_output("Added dummy-svc service 'my-service'"),
                and => the_configuration_should_contain_service("my-service", "dummy-svc")
            );
        }

        [Scenario]
        public void AddExistingService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_existing_service"),
                when => the_developer_runs_cli_command("add dummy-svc existing-service"),
                and => the_developer_runs_cli_command("add dummy-svc existing-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'existing-service' already exists")
            );
        }
    }
}
