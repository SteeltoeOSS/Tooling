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
    public class UndefineDependencyFeature : FeatureSpecs
    {
        [Scenario]
        public void UndefineDependencyHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undefine_dependency_help"),
                when => the_developer_runs_cli_command("undef-dep --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Removes a custom dependency definition",
                    $"Usage: {Program.Name} undef-dep [arguments] [options]",
                    "Arguments:",
                    "dep Dependency",
                    "Options:",
                    "--scope <scope> Sets the dependency definition scope (one of: project, global); default is project",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "*** under construction ***",
                    "Examples:",
                    "Remove a dependency definition:",
                    "$ st undef-dep MyService",
                    "See Also:",
                    "def-dep",
                    "list-deps",
                })
            );
        }

        [Scenario]
        public void DefineDependencyNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undefine_dependency_not_enough_args"),
                when => the_developer_runs_cli_command("undef-dep"),
                then => the_cli_should_error(ErrorCode.Argument, "Dependency not specified")
            );
        }

        [Scenario]
        public void RemoveDependencyTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("undefine_dependency_too_many_args"),
                when => the_developer_runs_cli_command("undef-dep arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }
    }
}
