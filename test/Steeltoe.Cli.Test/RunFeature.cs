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
    public class RunFeature : FeatureSpecs
    {
        [Scenario]
        public void RunHelp()
        {
            Runner.RunScenario(
                given => an_empty_directory("run_help"),
                when => the_developer_runs_cli_command("run --help"),
                then => the_cli_should_output(new[]
                {
                    "Runs project in the local Docker environment",
                    $"Usage: {Program.Name} run [options]",
                    "Options:",
                    "-n|--name <name> Sets the name of the configuration to be run (must be a Docker configuration)",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Starts the project application and its dependencies in the local Docker environment.",
                    "Examples:",
                    "Run the default configuration:",
                    "$ st run",
                    "Run a specific configuration:",
                    "$ st run -n MyDockerConfig",
                    "See Also:",
                    "stop",
                    "list-cfgs",
                })
            );
        }

        [Scenario]
        public void RunTooManyArgs()
        {
            Runner.RunScenario(
                given => an_empty_directory("run_too_many_args"),
                when => the_developer_runs_cli_command("run arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
