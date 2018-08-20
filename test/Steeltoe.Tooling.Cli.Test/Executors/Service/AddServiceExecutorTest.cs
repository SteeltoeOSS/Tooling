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

using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Tooling.Cli.Test.Executors.Service
{
    public partial class AddServiceExecutorTest
    {
        [Scenario]
        public void AddUnknownServiceType()
        {
            Runner.RunScenario(
                given => a_project(),
                when => add_service_is_run("MyService", "unknown-service-type"),
                then => an_exception_should_be_thrown<CommandException>("Unknown service type 'unknown-service-type'")
            );
        }

        [Scenario]
        public void RunAddExistingService()
        {
            Runner.RunScenario(
                given => a_project(),
                and => a_service("existing-service", "existing-service-type"),
                when => add_service_is_run("existing-service", "existing-service-type"),
                then => an_exception_should_be_thrown<CommandException>("Service 'existing-service' already exists")
            );
        }

        [Scenario]
        public void RunAddCloudFoundryConfigServerService()
        {
            Runner.RunScenario(
                given => a_project(),
                when => add_service_is_run("cfcs-service", "cloud-foundry-config-server"),
                then => the_output_should_match("Added cloud-foundry-config-server service 'cfcs-service'"),
                and => the_services_should_include("cfcs-service", "cloud-foundry-config-server")
            );
        }
    }
}
