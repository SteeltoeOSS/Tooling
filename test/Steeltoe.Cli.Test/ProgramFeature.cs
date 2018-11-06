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
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
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
        public void ProgramHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_help"),
                when => the_developer_runs_cli_command("--help"),
                then => the_cli_should_output(new[]
                {
                    "1.0.0",
                    "Steeltoe Developer Tools",
                    $"Usage: st [options] [command]",
                    "Options:",
                    "-V|--version Show version information",
                    $"-C|--config-file Configure tooling using the specified file instead of steeltoe.yml",
                    "-D|--debug Enable debug output",
                    "-S|--no-parallel Disable parallel execution",
                    "-?|-h|--help Show help information",
                    "Commands:",
                    "add Add an app or service",
                    "args Set or get the deployment arguments for an app or service",
                    "deploy Deploy apps and services to the target",
                    "doctor Check for potential problems",
                    "init Initialize Steeltoe Developer Tools",
                    "list List apps and services",
                    "remove Remove an app or service",
                    "status Show app and service statuses",
                    "target Set or get the deployment target",
                    "undeploy Undeploy apps and services from the target",
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
                then => the_cli_should_output("1.0.0")
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

        [Scenario]
        public void ProgramNoParallel()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_noparallel"),
                when => the_developer_runs_cli_command("--no-parallel --version"),
                then => setting_should_be(Settings.ParallelExecutionEnabled, false)
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
