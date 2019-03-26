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
    public class RemoveFeature : FeatureSpecs
    {
        [Scenario]
        public void RemoveHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_help"),
                when => the_developer_runs_cli_command("remove --help"),
                then => the_cli_should_output(new[]
                {
                    "Remove an app or service",
                    $"Usage: {Program.Name} remove [arguments] [options]",
                    "Arguments:",
                    "name App or service name",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void RemoveNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_not_enough_args"),
                when => the_developer_runs_cli_command("remove"),
                then => the_cli_should_error(ErrorCode.Argument, "App or service name not specified")
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
        public void RemoveUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_uninitialized"),
                when => the_developer_runs_cli_command("remove my-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void RemoveApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("remove_app"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("remove my-app"),
                then => the_cli_should_output("Removed app 'my-app'"),
                and => the_configuration_should_not_contain_app("my-app")
            );
        }

        [Scenario]
        public void RemoveService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("remove_service"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("remove my-service"),
                then => the_cli_should_output("Removed dummy-svc service 'my-service'"),
                and => the_configuration_should_not_contain_service("my-service")
            );
        }

        [Scenario]
        public void RemoveUnknownItem()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("remove_unknown_item"),
                when => the_developer_runs_cli_command("remove no-such-item"),
                then => the_cli_should_error(ErrorCode.Tooling, "App or service 'no-such-item' does not exist")
            );
        }
    }
}
