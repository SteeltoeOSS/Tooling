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

namespace Steeltoe.Tooling.Cli.Feature.Service
{
    [Label("service")]
    public class RemoveServiceFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void RemoveServiceHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_service_help"),
                when => the_developer_runs_steeltoe_command("remove-service --help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"^Remove a service\.\n"),
                and => the_developer_should_see(@"\n\s*name\s+The service name\n")
            );
        }

        [Scenario]
        public void RemoveServiceNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_service_not_enough_args"),
                when => the_developer_runs_steeltoe_command("remove-service"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("Service name not specified")
            );
        }

        [Scenario]
        public void RemoveServiceTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_service_too_many_args"),
                when => the_developer_runs_steeltoe_command("remove-service arg1 arg2"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void RemoveServiceWithoutToolingConfiguration()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_service_without_tooling_configuration"),
                when => the_developer_runs_steeltoe_command("remove-service unknown-service"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("No such service 'unknown-service'")
            );
        }

        [Scenario]
        public void RemoveUnknownService()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_unknown_service"),
                and => a_service("known-service", "known-service-type"),
                when => the_developer_runs_steeltoe_command("remove-service unknown-service"),
                then => the_command_should_fail(),
                and => the_developer_should_see_the_error("No such service 'unknown-service'")
            );
        }

        [Scenario]
        public void RemoveKnownService()
        {
            Runner.RunScenario(
                given => a_dotnet_project("remove_known_service"),
                and => a_target("keep-this-target"),
                and => a_service("known-service", "known-service-type"),
                when => the_developer_runs_steeltoe_command("remove-service known-service"),
                then => the_command_should_succeed(),
                and => the_developer_should_see("Removed service 'known-service'"),
                and => the_service_config_should_not_exist("known-service"),
                and => the_target_config_should_exist("keep-this-target")
            );
        }
    }
}
