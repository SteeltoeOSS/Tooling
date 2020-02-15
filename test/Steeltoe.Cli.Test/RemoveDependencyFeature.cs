// Copyright 2020 the original author or authors.
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
    public class RemoveDependencyFeature : FeatureSpecs
    {
        [Scenario]
        public void RemoveDependencyHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_dependency_help"),
                when => the_developer_runs_cli_command("rem-dep --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Removes a dependency that was added using the add-dep command",
                    $"Usage: {Program.Name} rem-dep [arguments] [options]",
                    "Arguments:",
                    "depname The name of the dependency to be removed",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Remove the named dependency from the project.",
                    "Examples:",
                    "Remove the dependency named MyRedis:",
                    "$ st rem-dep MyRedis",
                    "See Also:",
                    "add-dep",
                    "list-deps",
                })
            );
        }

        [Scenario]
        public void RemoveDependencyNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_dependency_not_enough_args"),
                when => the_developer_runs_cli_command("rem-dep"),
                then => the_cli_should_error(ErrorCode.Argument, "Dependency name not specified")
            );
        }

        [Scenario]
        public void RemoveDependencyTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_dependency_too_many_args"),
                when => the_developer_runs_cli_command("rem-dep arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }
    }
}
