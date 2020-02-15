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
    public class AddDependencyFeature : FeatureSpecs
    {
        [Scenario]
        public void AddDependencyHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_dependency_help"),
                when => the_developer_runs_cli_command("add-dep --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Adds a dependency",
                    $"Usage: {Program.Name} add-dep [arguments] [options]",
                    "Arguments:",
                    "dep The dependency to be added",
                    "Options:",
                    "-n|--name Sets the dependency name; default is <dep>",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Explicitly add a dependency to the project. Useful when autodetection fails.",
                    "Examples:",
                    "Add a dependency on a Redis service:",
                    "$ st add-dep redis",
                    "Add a dependency on a Redis service and name it MyRedis:",
                    "$ st add-dep redis --name MyRedis",
                    "See Also:",
                    "rem-dep",
                    "list-deps",
                })
            );
        }

        [Scenario]
        public void AddDependencyNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_dependency_not_enough_args"),
                when => the_developer_runs_cli_command("add-dep"),
                then => the_cli_should_error(ErrorCode.Argument, "Dependency not specified")
            );
            Console.Clear();
        }

        [Scenario]
        public void AddDependencyTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_dependency_too_many_args"),
                when => the_developer_runs_cli_command("add-dep arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }
    }
}
