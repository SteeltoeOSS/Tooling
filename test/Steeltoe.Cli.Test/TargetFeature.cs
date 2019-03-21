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
    public class TargetFeature : FeatureSpecs
    {
        [Scenario]
        public void TargetHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_help"),
                when => the_developer_runs_cli_command("target --help"),
                then => the_cli_should_output(new[]
                {
                    "Set or get the deployment target",
                    $"Usage: {Program.Name} target [arguments] [options]",
                    "Arguments:",
                    "target Deployment target name",
                    "Options:",
                    "-F|--force Set the deployment target even if checks fail",
                    "-?|-h|--help Show help information",
                    "If run with no args, show the current deployment target.",
                })
            );
        }

        [Scenario]
        public void TargetTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_too_many_args"),
                when => the_developer_runs_cli_command("target arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void TargetUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_uninitialized"),
                when => the_developer_runs_cli_command("target"),
                then => the_cli_should_error(ErrorCode.Tooling, "Steeltoe Developer Tools has not been initialized")
            );
        }

        [Scenario]
        public void TargetSet()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_set"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("target dummy-target"),
                then => the_cli_should_output(new[]
                {
                    "dummy tool version ... 0.0.0",
                    "Target set to 'dummy-target'",
                }),
                and => the_configuration_should_target("dummy-target")
            );
        }

        [Scenario]
        public void TargetSetUnknown()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("target_set_unknown"),
                when => the_developer_runs_cli_command("target no-such-target"),
                then => the_cli_should_error(ErrorCode.Tooling, "Unknown target 'no-such-target'")
            );
        }

        [Scenario]
        public void ShowTargetEnvironment()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("show_target_environment"),
                when => the_developer_runs_cli_command("target"),
                then => the_cli_should_output("dummy-target")
            );
        }
    }
}
