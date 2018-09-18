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
    [Label("list")]
    public class ListFeature : ListFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void ListHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_help"),
                when => the_developer_runs_cli_command("list --help"),
                and => the_cli_should_output(
                    "List services, service types, or deployment environments. If run with no args, list everything."),
                and => the_cli_should_output("scope One of: services, types, environments")
            );
        }

        [Scenario]
        public void ListNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_not_enough_args"),
                when => the_developer_runs_cli_command("list"),
                and => the_cli_should_error(ErrorCode.Argument, "List scope not specified")
            );
        }

        [Scenario]
        public void ListTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_too_many_args"),
                when => the_developer_runs_cli_command("list arg1 arg2"),
                and => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void ListUnknownScope()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_unknown_scope"),
                when => the_developer_runs_cli_command("list unknown-scope"),
                and => the_cli_should_error(ErrorCode.Tooling, "Unknown list scope 'unknown-scope")
            );
        }

        [Scenario]
        public void ListEnvironments()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_environments"),
                when => the_developer_runs_cli_command("list environments"),
                and => the_cli_should_list_available_environments()
            );
        }

        [Scenario]
        public void ListTypes()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_types"),
                when => the_developer_runs_cli_command("list types"),
                and => the_cli_should_list_available_service_types()
            );
        }

        [Scenario]
        public void ListServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_services"),
                and => the_developer_runs_cli_command("add z-service dummy-svc"),
                when => the_developer_runs_cli_command("add a-service dummy-svc"),
                and => the_developer_runs_cli_command("add 9-service dummy-svc"),
                and => the_developer_runs_cli_command("list services"),
                then => the_cli_should_list_services(new[]{"a-service", "z-service", "9-service"})
            );
        }
    }
}
