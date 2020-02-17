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
    public class HelpFeature : FeatureSpecs
    {
        [Scenario]
        public void HelpHelp()
        {
            Runner.RunScenario(
                given => an_empty_directory("help_help"),
                when => the_developer_runs_cli_command("help --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Displays documentation on a topic",
                    $"Usage: {Program.Name} help [arguments] [options]",
                    "Arguments:",
                    "topic Topic",
                    "Options:",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Displays documentation for various topics. Run with no arguments for a list of available topics.",
                    "Examples:",
                    "Display documentation for autodetection:",
                    "$ st help autodetection",
                })
            );
        }

        [Scenario]
        public void HelpTooManyArgs()
        {
            Runner.RunScenario(
                given => an_empty_directory("help_too_many_args"),
                when => the_developer_runs_cli_command("help arg1 arg2"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void HelpTopicList()
        {
            Runner.RunScenario(
                given => an_empty_directory("help_topic_list"),
                when => the_developer_runs_cli_command("help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "autodetection Application and Service Autodetection",
                })
            );
        }

        [Scenario]
        public void HelpNotFound()
        {
            Runner.RunScenario(
                given => an_empty_directory("help_topic_not_found"),
                when => the_developer_runs_cli_command("help no-such-topic"),
                then => the_cli_should_error(ErrorCode.Tooling, "'no-such-topic' does not exist")
            );
        }

        [Scenario]
        public void HelpAutodetection()
        {
            Runner.RunScenario(
                given => an_empty_directory("help_topic_autodetection"),
                when => the_developer_runs_cli_command("help autodetection"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Application and Service Autodetection",
                    "Application Autodetection",
                    "***",
                })
            );
        }
    }
}
