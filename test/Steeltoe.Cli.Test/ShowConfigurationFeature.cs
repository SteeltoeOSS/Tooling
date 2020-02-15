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
    public class ShowConfigurationFeature : FeatureSpecs
    {
        [Scenario]
        public void ShowConfigurationHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("show_configuration_help"),
                when => the_developer_runs_cli_command("show-cfg --help"),
                then => the_cli_should_output(new[]
                {
                    "Displays configuration details",
                    $"Usage: {Program.Name} show-cfg [options]",
                    "Options:",
                    "-n|--name <name> Sets the name of the configuration to be displayed",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "*** under construction ***",
                    "Examples:",
                    "Display the details of the default configuration:",
                    "$ st show-cfg",
                    "Display the details of a specific configuration:",
                    "$ st show-cfg --name MyCustomDockerConfig",
                })
            );
        }

        [Scenario]
        public void ShowConfigurationTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("show_configuration_too_many_args"),
                when => the_developer_runs_cli_command("show-cfg arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
