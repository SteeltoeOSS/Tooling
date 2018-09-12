// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Cli.Feature
{
    [Label("program")]
    public class ProgramFeature : CliFeatureSpecs
    {
        [Scenario]
        public void ProgramNoArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_no_args"),
                when => the_developer_runs_steeltoe_command(""),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see(@"Usage: steeltoe \[options\] \[command\]")
            );
        }

        [Scenario]
        [Label("help")]
        public void ProgramHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_help"),
                when => the_developer_runs_steeltoe_command("--help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"Steeltoe Developer Tools"),
                and => the_developer_should_see(@"-V\|--version\s+Show version information"),
                and => the_developer_should_see(@"-\?\|-h\|--help\s+Show help information"),
                and => the_developer_should_see(@"target\s+Target the deployment environment\."),
                and => the_developer_should_see(@"add\s+Add a service\."),
                and => the_developer_should_see(@"remove\s+Remove a service\."),
                and => the_developer_should_see(@"enable\s+Enable a service\."),
                and => the_developer_should_see(@"disable\s+Disable a service\."),
                and => the_developer_should_see(@"deploy\s+Start enabled services in the targeted deployment environment\."),
                and => the_developer_should_see(@"undeploy\s+Stop running services in the targeted deployment environment\."),
                and => the_developer_should_see(@"status\s+Show the status of a service in the targeted deployment environment\.  If run with no args, show the status of all services\."),
                and => the_developer_should_see(@"list\s+List services, service types, or deployment environments\.  If run with no args, list everything\.")
            );
        }

        [Scenario]
        [Label("version")]
        public void ProgramVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_version"),
                when => the_developer_runs_steeltoe_command("--version"),
                then => the_command_should_succeed(),
                and => the_developer_should_see("1.0.0")
            );
        }
    }
}
