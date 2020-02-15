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
    public class ListConfigurations : FeatureSpecs
    {
        [Scenario]
        public void ListConfigurationsHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_configurations_help"),
                when => the_developer_runs_cli_command("list-cfgs --help"),
                then => the_cli_should_output(new[]
                {
                    "Displays a list of available configurations",
                    $"Usage: {Program.Name} list-cfgs [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "*** under construction ***",
                })
            );
        }

        [Scenario]
        public void ListConfigurationsTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("list_configurations_too_many_args"),
                when => the_developer_runs_cli_command("list-cfgs arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
