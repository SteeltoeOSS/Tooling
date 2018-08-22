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

namespace Steeltoe.Tooling.Cli.Feature.Commands
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
                and => the_developer_should_see(@"add-service\s+Add a service\."),
                and => the_developer_should_see(@"check-service\s+Check the status of a service in the target environment\."),
                and => the_developer_should_see(@"list-service-types\s+List available service types\."),
                and => the_developer_should_see(@"list-services\s+List available services\."),
                and => the_developer_should_see(@"list-targets\s+List available target environments\."),
                and => the_developer_should_see(@"remove-service\s+Remove a service\."),
                and => the_developer_should_see(@"set-target\s+Set the target environment\."),
                and => the_developer_should_see(@"start-service\s+Start a service in the target environment\."),
                and => the_developer_should_see(@"stop-service\s+Stop a service in the target environment\.")
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
