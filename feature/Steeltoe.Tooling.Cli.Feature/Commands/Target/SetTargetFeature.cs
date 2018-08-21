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

namespace Steeltoe.Tooling.Cli.Feature.Commands.Target
{
    [Label("target")]
    public class SetTargetFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void SetTargetHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_help"),
                when => the_developer_runs_steeltoe_command("set-target --help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"Set the target environment\."),
                and => the_developer_should_see(@"environment\s+The environment")
            );
        }

        [Scenario]
        public void SetTargetMissingType()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_not_enough_args"),
                when => the_developer_runs_steeltoe_command("set-target"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("Environment not specified")
            );
        }

        [Scenario]
        public void SetTargetTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_target_too_many_args"),
                when => the_developer_runs_steeltoe_command("set-target arg1 arg2"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void SetUnknownTargetType()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_unknown_environment"),
                when => the_developer_runs_steeltoe_command("set-target no-such-environment"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("Unknown environment 'no-such-environment'")
            );
        }

        [Scenario]
        public void SetCloudFoundryTargetType()
        {
            Runner.RunScenario(
                given => a_dotnet_project("set_cloud_foundry_target"),
                when => the_developer_runs_steeltoe_command("set-target cloud-foundry"),
                then => the_command_should_succeed(),
                and => the_developer_should_see("Target environment set to 'cloud-foundry'."),
                and => the_target_config_should_exist("cloud-foundry")
            );
        }
    }
}
