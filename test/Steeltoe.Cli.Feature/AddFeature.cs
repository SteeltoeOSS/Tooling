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
    [Label("add")]
    public class AddFeature : CliFeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void AddHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_help"),
                when => the_developer_runs_steeltoe_command("add --help"),
                then => the_command_should_succeed(),
                and => the_developer_should_see(@"Add a service\."),
                and => the_developer_should_see(@"\s+name\s+Service name"),
                and => the_developer_should_see(@"\s+type\s+Service type\s+\(run 'steeltoe list types' for available service types\)")
            );
        }

        [Scenario]
        public void AddNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough_args0"),
                when => the_developer_runs_steeltoe_command("add"),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see_the_error("Service name not specified")
            );
            Runner.RunScenario(
                given => a_dotnet_project("add_not_enough_args1"),
                when => the_developer_runs_steeltoe_command("add myservice"),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see_the_error("Service type not specified")
            );
        }

        [Scenario]
        public void AddTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("add_too_many_args"),
                when => the_developer_runs_steeltoe_command("add arg1 arg2 arg3"),
                then => the_command_should_fail_with(1),
                and => the_developer_should_see_the_error("Unrecognized command or argument 'arg3'")
            );
        }

//        [Scenario]
//        public void AddUnknownServiceType()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("add_unknown_service_type"),
//                when => the_developer_runs_steeltoe_command("add-service foo --type no-such-type"),
//                then => the_command_should_fail_with(1),
//                and => the_developer_should_see_the_error("Unknown service type 'no-such-type'")
//            );
//        }

//        [Scenario]
//        public void AddAlreadyExistingService()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("add_already_existing_service"),
//                and => a_service("existing-service", "existing-service-type"),
//                when => the_developer_runs_steeltoe_command("add-service existing-service --type cloud-foundry-config-server"),
//                then => the_command_should_fail_with(1),
//                and => the_developer_should_see_the_error("Service 'existing-service' already exists")
//            );
//        }

//        [Scenario]
//        public void AddConfigServer()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("add_config_server"),
//                when => the_developer_runs_steeltoe_command("add-service MyConfigServer --type config-server"),
//                then => the_command_should_succeed(),
//                and => the_developer_should_see("Added config-server service 'MyConfigServer'"),
//                and => the_service_config_should_exist("MyConfigServer", "config-server")
//            );
//        }

//        [Scenario]
//        public void AddRegistry()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("add_registry"),
//                when => the_developer_runs_steeltoe_command("add-service MyRegistryService --type registry"),
//                then => the_command_should_succeed(),
//                and => the_developer_should_see("Added registry service 'MyRegistryService'"),
//                and => the_service_config_should_exist("MyRegistryService", "registry")
//            );
//        }
    }
}
