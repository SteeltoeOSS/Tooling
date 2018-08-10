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

namespace Steeltoe.Tooling.DotnetCli.Target.Feature
{
    [Label("target")]
    public partial class SetTargetFeature
    {
        [Scenario]
        [Label("help")]
        public void SetTargetHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_help"),
                when => the_developer_runs_steeltoe_("set-target --help"),
                then => the_command_succeeds(),
                and => the_developer_sees_("Set the target environment."),
                and => the_developer_sees_(@"environment\s+The target environment")
            );
        }

        [Scenario]
        public void SetTargetNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_not_enough_args"),
                when => the_developer_runs_steeltoe_("set-target"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("environment not specified")
            );
        }

        [Scenario]
        public void SetTargetTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_too_many_args"),
                when => the_developer_runs_steeltoe_("set-target arg1 arg2"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void SetTargetToCloudFoundry()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_to_cloud_foundry"),
                when => the_developer_runs_steeltoe_("set-target cloud-foundry"),
                then => the_command_succeeds(),
                and => the_developer_sees_("Target set to 'cloud-foundry'."),
                and => the_target_environment_is_("cloud-foundry")
            );
        }
    }
}
