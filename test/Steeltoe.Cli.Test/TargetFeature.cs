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
    [Label("target")]
    public class TargetFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void TargetHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_help"),
                when => the_developer_runs_cli_command("target --help"),
                then => the_cli_should_output("Set or get the targeted deployment environment."),
                then => the_cli_should_output("If run with no args, show the currently targeted deployment environment."),
                and => the_cli_should_output("environment Deployment environment"),
                and => the_cli_should_output("-F|--force Target the deployment environment even if checks fail")
            );
        }

        [Scenario]
        public void TargetTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_too_many_args"),
                when => the_developer_runs_cli_command("target arg1 arg2"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void TargetEnvironment()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_environment"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("target dummy-env"),
                then => the_cli_should_output("Target deployment environment set to 'dummy-env'."),
                and => the_configuration_should_target("dummy-env")
            );
        }

        [Scenario]
        public void TargetUnknownEnvironment()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("target_unknown_environment"),
                when => the_developer_runs_cli_command("target no-such-environment"),
                then => the_cli_should_error(ErrorCode.Tooling, "Unknown deployment environment 'no-such-environment'")
            );
        }

        [Scenario]
        public void TargetUninitializedProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_uninitialized_project"),
                when => the_developer_runs_cli_command("target"),
                then => the_cli_should_error(ErrorCode.Tooling,
                    "Project has not been initialized for Steeltoe Developer Tools")
            );
        }

        [Scenario]
        public void ShowTargetEnvironment()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("show_target_environment"),
                when => the_developer_runs_cli_command("target"),
                then => the_cli_should_output("Target deployment environment set to 'dummy-env'.")
            );
        }
    }
}
