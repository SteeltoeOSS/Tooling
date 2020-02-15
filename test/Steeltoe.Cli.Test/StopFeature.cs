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
    public class StopFeature : FeatureSpecs
    {
        [Scenario]
        public void StopHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("stop_help"),
                when => the_developer_runs_cli_command("stop --help"),
                then => the_cli_should_output(new[]
                {
                    "Stops the project running in the local Docker environment",
                    $"Usage: {Program.Name} stop [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Stops the project application and its dependencies in the local Docker environment.",
                    "Examples:",
                    "Stop the running project:",
                    "$ st stop",
                    "See Also:",
                    "run",
                })
            );
        }

        [Scenario]
        public void StopTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("stop_too_many_args"),
                when => the_developer_runs_cli_command("stop arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
