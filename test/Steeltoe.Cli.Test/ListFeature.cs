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
    public class ListFeature : FeatureSpecs
    {
        [Scenario]
        public void ListHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_help"),
                when => the_developer_runs_cli_command("list --help"),
                then => the_cli_should_output(new[]
                {
                    "List apps and services",
                    $"Usage: {Program.Name} list [options]",
                    "Options:",
                    "-v|--verbose Verbose",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void ListTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_too_many_args"),
                when => the_developer_runs_cli_command("list arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void ListUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_uninitialized"),
                when => the_developer_runs_cli_command("list"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        /*
        [Scenario]
        public void ListEnvironments()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_environments"),
                when => the_developer_runs_cli_command("list -e"),
                then => the_cli_should_output(new[]
                {
                    "cloud-foundry",
                    "docker",
                    "dummy-target",
                })
            );
        }
        */

        /*
        [Scenario]
        public void ListEnvironmentsVerbose()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_environments_verbose"),
                when => the_developer_runs_cli_command("list -e -v"),
                then => the_cli_should_output(new[]
                {
                    "cloud-foundry Cloud Foundry",
                    "docker Docker",
                    "dummy-target A Dummy Target",
                })
            );
        }
        */

        /*
        [Scenario]
        public void ListServiceTypes()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_service_types"),
                when => the_developer_runs_cli_command("list -t"),
                then => the_cli_should_output(new[]
                {
                    "config-server",
                    "dummy-svc",
                    "eureka-server",
                    "hystrix-dashboard",
                    "mssql",
                    "mysql",
                    "redis",
                    "zipkin",
                })
            );
        }
        */

        /*
        [Scenario]
        public void ListServiceTypesVerbose()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_service_types_verbose"),
                when => the_developer_runs_cli_command("list -t -v"),
                then => the_cli_should_output(new[]
                {
                    "config-server 8888 Cloud Foundry Config Server",
                    "dummy-svc 0 A Dummy Service",
                    "eureka-server 8761 Netflix Eureka Server",
                    "hystrix-dashboard 7979 Netflix Hystrix Dashboard",
                    "mssql 1433 Microsoft SQL Server",
                    "mysql 3306 Microsoft SQL Server",
                    "redis 6379 Redis In-Memory Datastore",
                    "zipkin 9411 Zipkin Tracing Collector and UI",
                })
            );
        }
        */

        [Scenario]
        public void ListServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_services"),
                when => the_developer_runs_cli_command("add dummy-svc my-service-c"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-b"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-a"),
                and => the_developer_runs_cli_command("list"),
                then => the_cli_should_output(new[]
                {
                    "my-service-a",
                    "my-service-b",
                    "my-service-c",
                })
            );
        }

        [Scenario]
        public void ListServicesVerbose()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("list_services_verbose"),
                when => the_developer_runs_cli_command("add dummy-svc my-service-c"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-b"),
                and => the_developer_runs_cli_command("add dummy-svc my-service-a"),
                and => the_developer_runs_cli_command("list -v"),
                then => the_cli_should_output(new[]
                {
                    "my-service-a 0 dummy-svc",
                    "my-service-b 0 dummy-svc",
                    "my-service-c 0 dummy-svc",
                })
            );
        }
    }
}
