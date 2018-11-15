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
                    "Set or get the arguments for an app or service",
                    $"Usage: {Program.Name} args [arguments] [options] [[--] <arg>...]",
                    "Arguments:",
                    "name App or service name",
                    "args App or service arguments",
                    "Options:",
                    "-t|--target Apply the args to the deployment on the specified target",
                    "-F|--force Overwrite existing arguments",
                    "-?|-h|--help Show help information",
                    "If run with no arguments, show the current arguments for the app or service.",
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
        }

        [Scenario]
        public void ArgsUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("args_uninitialized"),
                when => the_developer_runs_cli_command("args foo arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized"),
                when => the_developer_runs_cli_command("args foo"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void ArgsUnknownAppOrService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_unknown_app_or_service"),
                when => the_developer_runs_cli_command("args no-such-thing arg1"),
                then => the_cli_should_error(ErrorCode.Tooling, "App or service 'no-such-thing' does not exist"),
                when => the_developer_runs_cli_command("args no-such-thing"),
                then => the_cli_should_error(ErrorCode.Tooling, "App or service 'no-such-thing' does not exist")
            );
        }

        [Scenario]
        public void ArgsApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_app"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("args my-app"),
                then => the_cli_should_output_nothing(),
                when => the_developer_runs_cli_command("args my-app arg1 arg2"),
                then => the_cli_should_output("Set 'my-app' app args to 'arg1 arg2'"),
                and => the_configuration_should_contain_app_args("my-app", "arg1 arg2"),
                when => the_developer_runs_cli_command("args my-app arg3"),
                then => the_cli_should_error(ErrorCode.Tooling, "'my-app' app args already set to 'arg1 arg2'"),
                when => the_developer_runs_cli_command("args my-app arg3 --force"),
                then => the_cli_should_output("Set 'my-app' app args to 'arg3'"),
                and => the_configuration_should_contain_app_args("my-app", "arg3"),
                when => the_developer_runs_cli_command("args my-app"),
                then => the_cli_should_output("arg3")
            );
        }

        [Scenario]
        public void ArgsAppDeploy()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_app_deploy"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("args my-app -t dummy-target"),
                then => the_cli_should_output_nothing(),
                when => the_developer_runs_cli_command("args my-app -t dummy-target arg1 arg2"),
                then => the_cli_should_output("Set 'dummy-target' deploy args for 'my-app' app to 'arg1 arg2'"),
                and => the_configuration_should_contain_app_args("my-app", "dummy-target", "arg1 arg2"),
                when => the_developer_runs_cli_command("args my-app -t dummy-target arg3"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "'dummy-target' deploy args for 'my-app' app already set to 'arg1 arg2'"),
                when => the_developer_runs_cli_command("args my-app -t dummy-target arg3 --force"),
                then => the_cli_should_output("Set 'dummy-target' deploy args for 'my-app' app to 'arg3'"),
                and => the_configuration_should_contain_app_args("my-app", "dummy-target", "arg3"),
                when => the_developer_runs_cli_command("args my-app -t dummy-target"),
                then => the_cli_should_output("arg3")
            );
        }

        [Scenario]
        public void ArgsAppWithOpt()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_app_with_opt"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("args my-app -- arg1 -opt2"),
                then => the_cli_should_output("Set 'my-app' app args to 'arg1 -opt2'"),
                and => the_configuration_should_contain_app_args("my-app", "arg1 -opt2"),
                when => the_developer_runs_cli_command("args my-app"),
                then => the_cli_should_output("arg1 -opt2")
            );
        }

        [Scenario]
        public void ArgsAppDeployWithOpt()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_app_deploy_with_opt"),
                when => the_developer_runs_cli_command("add app my-app"),
                and => the_developer_runs_cli_command("args my-app -t dummy-target -- arg1 -opt2"),
                then => the_cli_should_output("Set 'dummy-target' deploy args for 'my-app' app to 'arg1 -opt2'"),
                and => the_configuration_should_contain_app_args("my-app", "dummy-target", "arg1 -opt2"),
                when => the_developer_runs_cli_command("args my-app -t dummy-target"),
                then => the_cli_should_output("arg1 -opt2")
            );
        }

        [Scenario]
        public void ArgsService()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_service"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service"),
                then => the_cli_should_output_nothing(),
                when => the_developer_runs_cli_command("args my-service arg1 arg2"),
                then => the_cli_should_output("Set 'my-service' dummy-svc service args to 'arg1 arg2'"),
                and => the_configuration_should_contain_service_args("my-service", "arg1 arg2"),
                when => the_developer_runs_cli_command("args my-service arg3"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "'my-service' dummy-svc service args already set to 'arg1 arg2'"),
                when => the_developer_runs_cli_command("args my-service arg3 --force"),
                then => the_cli_should_output("Set 'my-service' dummy-svc service args to 'arg3'"),
                and => the_configuration_should_contain_service_args("my-service", "arg3"),
                when => the_developer_runs_cli_command("args my-service"),
                then => the_cli_should_output("arg3")
            );
        }

        [Scenario]
        public void ArgsServiceDeploy()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_service"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service -t dummy-target"),
                then => the_cli_should_output_nothing(),
                when => the_developer_runs_cli_command("args my-service -t dummy-target arg1 arg2"),
                then => the_cli_should_output(
                    "Set 'dummy-target' deploy args for 'my-service' dummy-svc service to 'arg1 arg2'"),
                and => the_configuration_should_contain_service_args("my-service", "dummy-target", "arg1 arg2"),
                when => the_developer_runs_cli_command("args my-service -t dummy-target arg3"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "'dummy-target' deploy args for 'my-service' dummy-svc service already set to 'arg1 arg2'"),
                when => the_developer_runs_cli_command("args my-service -t dummy-target arg3 --force"),
                then => the_cli_should_output(
                    "Set 'dummy-target' deploy args for 'my-service' dummy-svc service to 'arg3'"),
                and => the_configuration_should_contain_service_args("my-service", "dummy-target", "arg3"),
                when => the_developer_runs_cli_command("args my-service -t dummy-target"),
                then => the_cli_should_output("arg3")
            );
        }

        [Scenario]
        public void ArgsServiceWithOpt()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_service_with_opt"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service -- arg1 -opt2"),
                then => the_cli_should_output("Set 'my-service' dummy-svc service args to 'arg1 -opt2'"),
                and => the_configuration_should_contain_service_args("my-service", "arg1 -opt2"),
                when => the_developer_runs_cli_command("args my-service"),
                then => the_cli_should_output("arg1 -opt2")
            );
        }

        [Scenario]
        public void ArgsServiceDeployWithOpt()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("args_service_deploy_with_opt"),
                when => the_developer_runs_cli_command("add dummy-svc my-service"),
                and => the_developer_runs_cli_command("args my-service -t dummy-target -- arg1 -opt2"),
                then => the_cli_should_output(
                    "Set 'dummy-target' deploy args for 'my-service' dummy-svc service to 'arg1 -opt2'"),
                and => the_configuration_should_contain_service_args("my-service", "dummy-target", "arg1 -opt2"),
                when => the_developer_runs_cli_command("args my-service -t dummy-target"),
                then => the_cli_should_output("arg1 -opt2")
            );
        }
    }
}
