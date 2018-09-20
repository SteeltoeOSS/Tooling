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
    public class ListFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void ListHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_help"),
                when => the_developer_runs_cli_command("list --help"),
                then => the_cli_should_output("List services, service types, or deployment environments."),
                and => the_cli_should_output("If run with no options, list services."),
                and => the_cli_should_output("-e|--environments List deployment environments"),
                and => the_cli_should_output("-s|--services List services"),
                and => the_cli_should_output("-t|--service-types List service types")
            );
        }

        [Scenario]
        public void ListTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_too_many_args"),
                when => the_developer_runs_cli_command("list arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void ListMutallyExclusiveOptions()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_mutually_exclusive_options"),
                when => the_developer_runs_cli_command("list -e -t"),
                then => the_cli_should_error(ErrorCode.Argument,
                    "Specify at most one of: -e|--environments, -s|--services, -t|--service-types")
            );
        }

        [Scenario]
        public void ListServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_services"),
                when => the_developer_runs_cli_command("add z-service dummy-svc"),
                and => the_developer_runs_cli_command("add a-service dummy-svc"),
                and => the_developer_runs_cli_command("add 9-service dummy-svc"),
                and => the_developer_runs_cli_command("list"),
                then => the_cli_should_list(new[] {"a-service", "z-service", "9-service"})
            );
        }

        [Scenario]
        public void ListEnvironments()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_environments"),
                when => the_developer_runs_cli_command("list -e"),
                then => the_cli_should_list(new[]
                {
                    "dummy-env (A dummy environment for testing Steeltoe Developer Tools)",
                    "cloud-foundry (Cloud Foundry)",
                    "docker (Docker)",
                })
            );
        }

        [Scenario]
        public void ListServiceTypes()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_service_types"),
                when => the_developer_runs_cli_command("list -t"),
                then => the_cli_should_list(new[]
                {
                    "dummy-svc (A dummy service for testing Steeltoe Developer Tools)",
                    "config-server (Cloud Foundry Config Server)",
                    "eureka (Netflix Eureka Server)"
                })
            );
        }
    }
}
