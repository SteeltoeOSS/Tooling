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

namespace Steeltoe.Cli.Test
{
    public class DoctorFeature : FeatureSpecs
    {
        [Scenario]
        public void DoctorHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_help"),
                when => the_developer_runs_cli_command("doctor --help"),
                then => the_cli_should_output(new[]
                {
                    "Check for potential problems",
                    $"Usage: {Program.Name} doctor [options]",
                    "Options:",
                    "-?|-h|--help Show help information",
                })
            );
        }

        [Scenario]
        public void DoctorTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_too_many_args"),
                when => the_developer_runs_cli_command("doctor arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }

        [Scenario]
        public void DoctorUninitialized()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_uninitialized"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include(
                    $"initialized ... !!! no (run '{Program.Name} init' to initialize)")
            );
        }

        [Scenario]
        public void DoctorInitialized()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("doctor_initialized"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include("initialized ... yes")
            );
        }

        [Scenario]
        public void DoctorVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_version"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include("Steeltoe Developer Tools version ... 1.0.0")
            );
        }

        [Scenario]
        public void DoctorDotnetVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_dotnet_version"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include("DotNet ... dotnet version ")
            );
        }

        [Scenario]
        public void DoctorTarget()
        {
            Runner.RunScenario(
                given => a_steeltoe_project("doctor_target"),
                when => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include("target ... dummy-target")
            );
        }

        [Scenario]
        public void DoctorNoTarget()
        {
            Runner.RunScenario(
                given => a_dotnet_project("doctor_no_target"),
                when => the_developer_runs_cli_command("init"),
                and => the_developer_runs_cli_command("doctor"),
                then => the_cli_output_should_include(
                    $"target ... !!! not set (run '{Program.Name} target <env>' to set)")
            );
        }
    }
}
