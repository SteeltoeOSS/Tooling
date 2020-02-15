// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
    public class ProgramFeature : FeatureSpecs
    {
        [Scenario]
        public void ProgramNoArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_no_args"),
                when => the_developer_runs_cli_command(""),
                and => the_cli_should_error(ErrorCode.Argument),
                and => the_cli_output_should_include("Usage: st [options] [command]")
            );
        }

        [Scenario]
        public void ProgramHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_help"),
                when => the_developer_runs_cli_command("--help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "*",
                    "Steeltoe Developer Tools",
                    $"Usage: st [options] [command]",
                    "Options:",
                    "-V|--version Show version information",
                    $"-C|--config-file Configure tooling using the specified file instead of steeltoe.yaml",
                    "-D|--debug Enable debug output",
                    "-v|--verbose Enable verbose output",
                    "-?|-h|--help Show help information",
                    "Commands:",
                    "add-dep Adds a dependency",
                    "def-dep Adds a custom dependency definition",
                    "doctor Checks for potential problems",
                    "list-cfgs Displays a list of available configurations",
                    "list-deps Displays a list of available dependencies",
                    "list-templates Displays a list of available templates",
                    "new Creates a new project using Steeltoe Initializr",
                    "new-cfg Creates configuration files for a target",
                    "rem-dep Removes a dependency that was added using the add-dep command",
                    "run Runs the project in the local Docker environment",
                    "show Displays the project details",
                    "show-cfg Displays configuration details",
                    "show-topic Displays documentation on a topic",
                    "stop Stops the project running in the local Docker environment",
                    "undef-dep Removes a custom dependency definition",
                    $"Run 'st [command] --help' for more information about a command.",
                })
            );
        }

        [Scenario]
        public void ProgramVersion()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_version"),
                when => the_developer_runs_cli_command("--version"),
                then => the_cli_command_should_succeed()
            );
        }

        [Scenario]
        public void ProgramDebug()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_debug"),
                when => the_developer_runs_cli_command("--debug --version"),
                then => setting_should_be(Settings.DebugEnabled, true)
            );
        }

        // ========================================================================== //
        // following can not be run in suite since the config file property is static //
        // ========================================================================== //

//        [Scenario]
//        [Label("version")]
//        public void CustomConfigurationFile()
//        {
//            Runner.RunScenario(
//                given => a_dotnet_project("program_config_file"),
//                when => the_developer_runs_cli_command("-C my-cfgfile init"),
//                then => the_file_should_exist("my-cfgfile")
//            );
//        }
    }
}
