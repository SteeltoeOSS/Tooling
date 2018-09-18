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

using Shouldly;

namespace Steeltoe.Cli.Test
{
    public class ListFeatureSpecs : FeatureSpecs
    {
        protected void the_cli_should_list_available_environments()
        {
            the_cli_command_should_succeed();
            string[] expected =
            {
                "dummy-env (A dummy environment for testing Steeltoe Developer Tools",
                "cloud-foundry (Cloud Foundry)",
                "docker (Docker)"
            };
            foreach (string env in expected)
            {
                _shellOut.ShouldContain(env);
            }
        }

        protected void the_cli_should_list_available_service_types()
        {
            the_cli_command_should_succeed();
            string[] expected =
            {
                "dummy-svc (A dummy service for testing Steeltoe Developer Tools)",
                "config-server (Cloud Foundry Config Server)",
                "registry (Netflix Eureka Server)"
            };
            foreach (string type in expected)
            {
                _shellOut.ShouldContain(type);
            }
        }

        protected void the_cli_should_list_services(string[] expected)
        {
            the_cli_command_should_succeed();
            foreach (string service in expected)
            {
                _shellOut.ShouldContain(service);
            }
        }
    }
}
