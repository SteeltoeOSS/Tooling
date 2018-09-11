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
    [Label("target")]
    public class TargetFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void TargetHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_help"),
                when => the_developer_runs_steeltoe_command("target --help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"Target the deployment environment\.  If run with no args, show the targeted deployment environment\."),
                and => the_developer_should_see(@"\s+environment\s+Deployment environment\s+\(run 'steeltoe list targets' for available deployment environments\)"),
                and => the_developer_should_see(@"\s+-F\|--force\s+Target the deployment environment even if checks fail")
            );
        }

        [Scenario]
        public void TargetTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("target_too_many_args"),
                when => the_developer_runs_steeltoe_command("target arg1 arg2"),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see_the_error("Unrecognized command or argument 'arg2'")
            );
        }

//        [Scenario]
//        public void SetUnknownTargetType()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("set_unknown_environment"),
//                when => the_developer_runs_steeltoe_command("set-target no-such-environment"),
//                then => the_command_should_fail_with(1),
//                and => the_developer_should_see_the_error("Unknown environment 'no-such-environment'")
//            );
//        }

//        [Scenario]
//        public void SetCloudFoundryTarget()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("set_cloud_foundry_target"),
//                when => the_developer_runs_steeltoe_command("set-target cloud-foundry"),
//                then => the_command_should_succeed(),
//                and => the_developer_should_see("Target environment set to 'cloud-foundry'."),
//                and => the_target_config_should_exist("cloud-foundry")
//            );
//        }

//        [Scenario]
//        public void SetDockerTarget()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("set_docker_target"),
//                when => the_developer_runs_steeltoe_command("set-target docker"),
//                then => the_command_should_succeed(),
//                and => the_developer_should_see("Target environment set to 'docker'"),
//                and => the_target_config_should_exist("docker")
//            );
//        }
    }
}
