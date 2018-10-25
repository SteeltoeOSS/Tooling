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
    [Label("args")]
    public class ArgsFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void ArgsHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_help"),
                when => the_developer_runs_cli_command("args --help"),
                then => the_cli_should_output(new[]
                {
                    "Set or get the deployment environment arguments for a service.",
                    $"Usage: {Program.Name} args [arguments] [options] [[--] <arg>...]",
                    "Arguments:",
                    "environment Deployment environment",
                    "service Service name",
                    "args Deployment environment arguments",
                    "Options:",
                    "-F|--force Overwrite existing deployment environment arguments",
                    "-?|-h|--help Show help information",
                    "If run with no deployment environment arguments, show the service's current deployment environment arguments.",
                })
            );
        }

        [Scenario]
        public void ArgsNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_not_enough_args0"),
                when => the_developer_runs_cli_command("args"),
                then => the_cli_should_error(ErrorCode.Argument, "Deployment environment not specified")
            );
            Console.Clear();
            Runner.RunScenario(
                given => a_dotnet_project("args_not_enough_args1"),
                when => the_developer_runs_cli_command("args arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Service name not specified")
            );
        }

        [Scenario]
        public void ArgsUninitializedProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_uninitialized_project"),
                when => the_developer_runs_cli_command("args dummy-env a-service"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "Project has not been initialized for Steeltoe Developer Tools")
            );
        }

        [Scenario]
        public void ArgsUnknownEnvironment()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_unknown_environment"),
                when => the_developer_runs_cli_command("args no-such-env a-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Unknown deployment environment 'no-such-env'")
            );
        }

        [Scenario]
        public void ArgsNoSuchService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_nonexistent_service"),
                when => the_developer_runs_cli_command("args dummy-env no-such-svc"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'no-such-svc' not found")
            );
            Console.Clear();
            Runner.RunScenario(
                given => a_steeltoe_project("args_nonexistent_service"),
                when => the_developer_runs_cli_command("args dummy-env no-such-svc arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'no-such-svc' not found")
            );
        }

        [Scenario]
        public void ArgsSetDeploymentEnvironmentArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_deployment_environment_args"),
                when => the_developer_runs_cli_command("add dummy-svc a-service"),
                and => the_developer_runs_cli_command("args dummy-env a-service arg1 arg2"),
                then => the_cli_should_output(
                    "Set the 'dummy-env' deployment environment arguments for service 'a-service' to 'arg1 arg2'")
            );
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_deployment_environment_args_with_opt"),
                when => the_developer_runs_cli_command("add dummy-svc a-service"),
                and => the_developer_runs_cli_command("args dummy-env a-service -- arg1 -opt2"),
                then => the_cli_should_output(
                    "Set the 'dummy-env' deployment environment arguments for service 'a-service' to 'arg1 -opt2'")
            );
        }

        [Scenario]
        public void ArgsSetDeploymentEnvironmentArgsForce()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_deployment_environment_args_force"),
                when => the_developer_runs_cli_command("add dummy-svc a-service"),
                and => the_developer_runs_cli_command("args dummy-env a-service arg1 arg2"),
                and => the_developer_runs_cli_command("args dummy-env a-service arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "'dummy-env' deployment environment arguments for service 'a-service' already set."),
                when => the_developer_runs_cli_command("args dummy-env a-service arg1 arg2 --force"),
                then => the_cli_should_output(
                    "Set the 'dummy-env' deployment environment arguments for service 'a-service' to 'arg1 arg2'")
            );
        }

        [Scenario]
        public void ArgsGetDeploymentEnvironmentArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get_deployment_environment_args"),
                when => the_developer_runs_cli_command("add dummy-svc a-service"),
                and => the_developer_runs_cli_command("args dummy-env a-service arg1 arg2"),
                and => the_developer_runs_cli_command("args dummy-env a-service"),
                then => the_cli_should_output("arg1 arg2")
            );
        }

        [Scenario]
        public void ArgsGetDeploymentEnvironmentNoArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get_deployment_environment_no_args"),
                when => the_developer_runs_cli_command("add dummy-svc a-service"),
                when => the_developer_runs_cli_command("args dummy-env a-service"),
                then => the_cli_should_output_nothing()
            );
        }
    }
}
