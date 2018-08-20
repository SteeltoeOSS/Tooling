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
    public partial class ListServicesExecutorTest
    {
        [Scenario]
        public void RunListNoServices()
        {
            Runner.RunScenario(
                given => a_project(),
                when => list_services_is_run(),
                and => the_output_should_be_empty()
            );
        }

        [Scenario]
        public void RunListServices()
        {
            Runner.RunScenario(
                given => a_project(),
                and => a_service("service-a", "service-a-type"),
                and => a_service("service-z", "service-z-type"),
                when => list_services_is_run(),
                and => the_output_should_be("service-a (service-a-type)\nservice-z (service-z-type)")
            );
        }
    }
}
