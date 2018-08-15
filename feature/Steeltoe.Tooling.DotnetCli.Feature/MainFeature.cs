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
using Steeltoe.Tooling.DotnetCli.Base.Feature;

namespace Steeltoe.Tooling.DotnetCli.Feature
{
    [Label("main")]
    public class SetTargetFeature : DotnetCliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void MainHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_help"),
                when => the_developer_runs_steeltoe_("--help"),
                then => the_command_succeeds(),
                and => the_developer_sees_("Steeltoe Developer Tools"),
                and => the_developer_sees_(@"add-service\s+Add a service."),
                and => the_developer_sees_(@"check-service\s+Check a service in the current target."),
                and => the_developer_sees_(@"list-services\s+List available service types."),
                and => the_developer_sees_(@"list-targets\s+List available target environments."),
                and => the_developer_sees_(@"remove-service\s+Remove a service."),
                and => the_developer_sees_(@"set-target\s+Set the target environment."),
                and => the_developer_sees_(@"start-service\s+Start a service in the current target."),
                and => the_developer_sees_(@"stop-service\s+Stop a service in the current target.")
            );
        }

        [Scenario]
        public void MainNoArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("main_no_args"),
                when => the_developer_runs_steeltoe_(""),
                then => the_command_succeeds()
            );
        }
    }
}
