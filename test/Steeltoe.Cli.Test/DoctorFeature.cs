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
    [Label("doctor")]
    public class DoctorFeature : FeatureSpecs
    {
        [Scenario]
        [Label("help")]
        public void DoctorHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("dotnet_help"),
                when => the_developer_runs_cli_command("doctor --help"),
                then => the_cli_should_output("Check for potential problems.")
            );
        }

        [Scenario]
        public void DoctorTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_too_many_args"),
                when => the_developer_runs_cli_command("doctor arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void DoctorVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_version"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output("Steeltoe Developer Tools version ... 1.0.0")
            );
        }

        [Scenario]
        public void DoctorDotnetVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_dotnet_version"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output("DotNet ... dotnet version ")
            );
        }

        [Scenario]
        public void DoctorUninitializedProject()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_uninitialized_project"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output($"initialized ... !!! no (run '{Program.Name} init' to initialize)")
            );
        }

        [Scenario]
        public void DoctorInitializedProject()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("doctor_initialized_project"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output("initialized ... yes")
            );
        }

        [Scenario]
        public void DoctorTarget()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_target"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output(
                    $"target deployment environment ... !!! not set (run '{Program.Name} target <env>' to set)"),
                when => the_developer_runs_cli_command("target dummy-env"),
                and => the_developer_runs_cli_command("doctor"),
                then => the_cli_should_output("target deployment environment ... dummy-env"),
                and => the_cli_should_output("dummy tool version ... 0.0.0")
            );
        }
    }
}
