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

namespace Steeltoe.Tooling.DotnetCli.Service.Feature
{
    [Label("service")]
    public class AddServiceFeature : DotnetCliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void AddServiceHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_help"),
                when => the_developer_runs_steeltoe_("add-service --help"),
                then => the_command_succeeds(),
                and => the_developer_sees_("Add a service to the target environment."),
                and => the_developer_sees_(@"name\s+The service name"),
                and => the_developer_sees_(@"-s|--service-type\s+The service type")
            );
        }

        [Scenario]
        public void AddServiceNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_not_enough_args"),
                when => the_developer_runs_steeltoe_("add-service"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("Service name not specified")
            );
        }

        [Scenario]
        public void AddServiceTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_too_many_args"),
                when => the_developer_runs_steeltoe_("add-service arg1 arg2"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("Unrecognized command or argument 'arg2'")
            );
        }

        [Scenario]
        public void AddServiceMissingType()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_service_missing_type"),
                when => the_developer_runs_steeltoe_("add-service foo"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("Service type not specified")
            );
        }

        [Scenario]
        public void AddUnknownService()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_unknown_service"),
                when => the_developer_runs_steeltoe_("add-service foo -s no-such-type"),
                then => the_command_fails(),
                and => the_developer_sees_the_error_("Unknown service type 'no-such-type'")
            );
        }

        [Scenario]
        public void AddCloudFoundryConfigServerService()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_cloud_foundry_config_server_service"),
                when => the_developer_runs_steeltoe_("add-service MyConfigServer -s cloud-foundry-config-server"),
                then => the_command_succeeds(),
                and => the_developer_sees_("Added cloud-foundry-config-server service 'MyConfigServer'"),
                and => the_tooling_config_defines_the_service_("MyConfigServer", "cloud-foundry-config-server")
            );
        }
    }
}
