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
    public class ShowTopicFeature : FeatureSpecs
    {
        [Scenario]
        public void ShowTopicHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("show_topic_help"),
                when => the_developer_runs_cli_command("show-topic --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Displays documentation on a topic",
                    $"Usage: {Program.Name} show-topic [arguments] [options]",
                    "Arguments:",
                    "topic Topic",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Displays documentation for various topics. Run with no arguments for a list of available topics.",
                    "Examples:",
                    "Display documentation for autodetection:",
                    "$ st show-topic autodetection",
                })
            );
        }

        [Scenario]
        public void ShowTopicTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("show_topic_too_many_args"),
                when => the_developer_runs_cli_command("show-topic arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }
    }
}
