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

namespace Steeltoe.Tooling.Cli.Feature
{
    [Label("program")]
    public class ProgramFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void ProgramHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_help"),
                when => the_developer_runs_steeltoe_command("--help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"\n\s*Steeltoe Developer Tools\n"),
                and => the_developer_should_see(@"\n\s*-V\|--version\s+Show version information\n"),
                and => the_developer_should_see(@"\n\s*-\?\|-h\|--help\s+Show help information\n"),
                and => the_developer_should_see(@"\n\s*add-service\s+Add a service\.\n"),
                and => the_developer_should_see(@"\n\s*check-service\s+Check a service in the current target\.\n"),
                and => the_developer_should_see(@"\n\s*list-service-types\s+List available service types\.\n"),
                and => the_developer_should_see(@"\n\s*list-services\s+List available services\.\n"),
                and => the_developer_should_see(@"\n\s*list-targets\s+List available target environments\.\n"),
                and => the_developer_should_see(@"\n\s*remove-service\s+Remove a service\.\n"),
                and => the_developer_should_see(@"\n\s*set-target\s+Set the target environment\.\n"),
                and => the_developer_should_see(@"\n\s*start-service\s+Start a service in the current target\.\n"),
                and => the_developer_should_see(@"\n\s*stop-service\s+Stop a service in the current target\.\n")
            );
        }

        [Scenario]
        public void ProgramNoArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_no_args"),
                when => the_developer_runs_steeltoe_command(""),
                then => the_command_should_fail(),
                and => the_developer_should_see(@"Usage: steeltoe \[options\] \[command\]")
            );
        }

        [Scenario]
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
