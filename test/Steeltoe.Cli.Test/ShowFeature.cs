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
    public class ShowFeature : FeatureSpecs
    {
        [Scenario]
        public void ShowHelp()
        {
            Runner.RunScenario(
                given => an_empty_directory("show_help"),
                when => the_developer_runs_cli_command($"show --help"),
                then => the_cli_should_output(new[]
                {
                    "Displays project details",
                    $"Usage: {Program.Name} show [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "*** under construction ***",
                    "Examples:",
                    "Show the details of the project in the current directory:",
                    "$ st show",
                })
            );
        }

        [Scenario]
        public void ShowTooManyArgs()
        {
            Runner.RunScenario(
                given => an_empty_directory("show_too_many_args"),
                when => the_developer_runs_cli_command("show arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void Show()
        {
            Runner.RunScenario(
                given => a_dotnet31_project("show"),
                when => the_developer_runs_cli_command("show"),
                then => the_cli_should_output(new[]
                {
                    "configuration: show",
                    "project:",
                    "name: show",
                    "file: show.csproj",
                    "framework: netcoreapp3.1",
                    "image: mcr.microsoft.com/dotnet/core/sdk:3.1",
                    "protocols:",
                    "- name: http",
                    "port: 5000",
                    "- name: https",
                    "port: 5001",
                })
            );
        }
    }
}
