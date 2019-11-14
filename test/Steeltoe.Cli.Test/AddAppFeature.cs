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
    public class AddAppFeature : FeatureSpecs
    {
        [Scenario]
        public void AddAppHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_app_help"),
                when => the_developer_runs_cli_command("add-app --help"),
                then => the_cli_should_output(new[]
                {
                    "Add an app",
                    $"Usage: {Program.Name} add-app [arguments] [options]",
                    "Arguments:",
                    "name App name",
                    "Options:",
                    "-f|--framework Target framework",
                    "-r|--runtime Target runtime",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void AddAppNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_app_not_enough"),
                when => the_developer_runs_cli_command("add-app"),
                then => the_cli_should_error(ErrorCode.Argument, "App name not specified")
            );
        }

        [Scenario]
        public void AddAppTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_app_too_many_args"),
                when => the_developer_runs_cli_command("add-app arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void AddAppUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_app_uninitialized"),
                when => the_developer_runs_cli_command("add-app my-app"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }


        [Scenario]
        public void AddApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_app"),
                when => the_developer_runs_cli_command("add-app my-app"),
                then => the_cli_should_output("Added app 'my-app' (netcoreapp2.1/win10-x64)"),
                and => the_configuration_should_contain_app("my-app")
            );
        }

        [Scenario]
        public void AddExistingApp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_app_existing_app"),
                when => the_developer_runs_cli_command("add-app existing-app"),
                and => the_developer_runs_cli_command("add-app existing-app"),
                then => the_cli_should_error(ErrorCode.Tooling, "App 'existing-app' already exists")
            );
        }

        [Scenario]
        public void AddAppFramework()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_app_framework"),
                when => the_developer_runs_cli_command("add-app my-app -f dummy-framework"),
                and => the_configuration_should_contain_app_framework("my-app", "dummy-framework")
            );
        }

        [Scenario]
        public void AddAppRuntime()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("add_app_runtime"),
                when => the_developer_runs_cli_command("add-app my-app -r dummy-runtime"),
                and => the_configuration_should_contain_app_runtime("my-app", "dummy-runtime")
            );
        }
    }
}
