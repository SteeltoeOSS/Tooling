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
    public class ArgsFeature : FeatureSpecs
    {
        [Scenario]
        public void ArgsHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_help"),
                when => the_developer_runs_cli_command("args --help"),
                then => the_cli_should_output(new[]
                {
                    "Set or get the deployment arguments for an app or service",
                    $"Usage: {Program.Name} args [arguments] [options] [[--] <arg>...]",
                    "Arguments:",
                    "name App or service name",
                    "target Deployment target name",
                    "args Deployment arguments",
                    "Options:",
                    "-F|--force Overwrite existing deployment environment arguments",
                    "-?|-h|--help Show help information",
                    "If run with no arguments, show the current deployment arguments for the app or service.",
                })
            );
        }

        [Scenario]
        public void ArgsNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_not_enough"),
                when => the_developer_runs_cli_command("args"),
                then => the_cli_should_error(ErrorCode.Argument, "App or service name not specified")
            );
            Console.Clear();
            Runner.RunScenario(
                given => a_dotnet_project("args_not_enough_args1"),
                when => the_developer_runs_cli_command("args arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Deployment target name not specified")
            );
        }

        [Scenario]
        public void ArgsGetUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_get_uninitialized"),
                when => the_developer_runs_cli_command("args dummy-env my-service"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void ArgsSetUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_set_uninitialized"),
                when => the_developer_runs_cli_command("args dummy-env my-service arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void ArgsGetUnknownTarget()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get_unknown_target"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service no-such-target"),
                then => the_cli_should_error(ErrorCode.Tooling, "Target 'no-such-target' does not exist")
            );
        }

        [Scenario]
        public void ArgsSetUnknownTarget()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_unknown_target"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service no-such-target arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "Target 'no-such-target' does not exist")
            );
        }

        [Scenario]
        public void ArgsGetUnknownService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get_unknown_service"),
                when => the_developer_runs_cli_command("args no-such-svc dummy-target"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'no-such-svc' does not exist")
            );
        }

        [Scenario]
        public void ArgsSetUnknownService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_unknown_service"),
                when => the_developer_runs_cli_command("args no-such-svc dummy-target arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "Service 'no-such-svc' does not exist")
            );
        }

        [Scenario]
        public void ArgsSet()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service dummy-target arg1 arg2"),
                then => the_cli_should_output("Service 'my-service' args for target 'dummy-target' set to 'arg1 arg2'")
            );
        }

        [Scenario]
        public void ArgsSetWithOpt()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_with_opt"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service dummy-target -- arg1 -opt2"),
                then => the_cli_should_output("Service 'my-service' args for target 'dummy-target' set to 'arg1 -opt2'")
            );
        }

        [Scenario]
        public void ArgsSetForce()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_set_force"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service dummy-target arg1 arg2"),
                and => the_developer_runs_cli_command("args my-service dummy-target arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "Service 'my-service' args for target 'dummy-target' already set to 'arg1 arg2'"),
                when => the_developer_runs_cli_command("args my-service dummy-target arg3 --force"),
                then => the_cli_should_output("Service 'my-service' args for target 'dummy-target' set to 'arg3'")
            );
        }

        [Scenario]
        public void ArgsGet()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service dummy-target arg1 arg2"),
                and => the_developer_runs_cli_command("args my-service dummy-target"),
                then => the_cli_should_output("arg1 arg2")
            );
        }

        [Scenario]
        public void ArgsGetNoArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_get_no_args"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                when => the_developer_runs_cli_command("args my-service dummy-target"),
                then => the_cli_should_output_nothing()
            );
        }
    }
}
