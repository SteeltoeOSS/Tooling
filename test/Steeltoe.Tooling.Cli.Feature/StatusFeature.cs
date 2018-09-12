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
    [Label("status")]
    public class StatusFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void StatusHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_help"),
                when => the_developer_runs_steeltoe_command("status --help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"Show the status of a service in the targeted deployment environment\.  If run with no args, show the status of all services\."),
                and => the_developer_should_see(@"\s+name\s+Service name")
            );
        }

        [Scenario]
        public void StatusTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("status_too_many_args"),
                when => the_developer_runs_steeltoe_command("status arg1 arg2"),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see_the_error("Unrecognized command or argument 'arg2'")
            );
        }
    }
}
