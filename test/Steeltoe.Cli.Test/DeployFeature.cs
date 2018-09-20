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

namespace Steeltoe.Cli.Test
{
    [Label("deploy")]
    public class DeployFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void DeployHelp()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_help"),
                when => the_developer_runs_cli_command("deploy --help"),
                then => the_cli_should_output("Deploy enabled services to the targeted deployment environment.")
            );
        }

        [Scenario]
        public void DeployTooManyArgs()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_too_many_args"),
                when => the_developer_runs_cli_command("deploy arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void DeployServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_services"),
                when => the_developer_runs_cli_command("add a-service dummy-svc"),
                and => the_developer_runs_cli_command("add defunct-service dummy-svc"),
                and => the_developer_runs_cli_command("disable defunct-service"),
                and => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_output("Deploying service 'a-service'"),
                and => the_cli_should_output("Ignoring disabled service 'defunct-service'"),
                when => the_developer_runs_cli_command("status"),
                then => the_cli_should_output("a-service starting"),
                and => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_output("Deploying service 'a-service'"),
                and => the_cli_should_output("Ignoring disabled service 'defunct-service'")
            );
        }

        [Scenario]
        public void DeployNoServices()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("deploy_no_services"),
                when => the_developer_runs_cli_command("deploy"),
                then => the_cli_should_output("No services have been added")
            );
        }
    }
}
