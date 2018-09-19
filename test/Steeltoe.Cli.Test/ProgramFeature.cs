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
    [Label("program")]
    public class ProgramFeature : FeatureSpecs
    {
//        [Scenario]
//        public void ProgramNoArgs()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("program_no_args"),
//                when => the_developer_runs_cli_command(""),
//                and => the_cli_should_error(1, "Usage: steeltoe [options] [command]")
//            );
//        }

        [Scenario]
        [Label("help")]
        public void ProgramHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_help"),
                when => the_developer_runs_cli_command("--help"),
                then => the_cli_should_output("Steeltoe Developer Tools"),
                and => the_cli_should_output("-D|--debug Enable debug output"),
                and => the_cli_should_output("-V|--version Show version information"),
                and => the_cli_should_output("-?|-h|--help Show help information"),
                and => the_cli_should_output("init Initialize a project for Steeltoe Developer Tools."),
                and => the_cli_should_output("target Target the deployment environment."),
                and => the_cli_should_output("add Add a service."),
                and => the_cli_should_output("remove Remove a service."),
                and => the_cli_should_output("enable Enable a service."),
                and => the_cli_should_output("disable Disable a service."),
                and => the_cli_should_output("deploy Start an enabled service in the targeted deployment environment."),
                and => the_cli_should_output(
                    "undeploy Stop an enabled service in the targeted deployment environment."),
                and => the_cli_should_output(
                    "status Show the status of a service in the targeted deployment environment. If run with no args, show the status of all services"),
                and => the_cli_should_output(
                    "list List services, service types, or deployment environments.")
            );
        }

        [Scenario]
        [Label("version")]
        public void ProgramVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_version"),
                when => the_developer_runs_cli_command("--version"),
                then => the_cli_should_output("1.0.0")
            );
        }
    }
}
