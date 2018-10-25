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
using Steeltoe.Tooling;

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
                then => the_cli_should_output(new[]
                {
                    "1.0.0",
                    "Steeltoe Developer Tools",
                    $"Usage: {Program.Name} [options] [command]",
                    "Options:",
                    "-V|--version Show version information",
                    "-C|--config-file Configure tooling using the specified file instead of .steeltoe.tooling.yml",
                    "-D|--debug Enable debug output",
                    "-S|--no-parallel Disable parallel execution",
                    "-?|-h|--help Show help information",
                    "Commands:",
                    "add Add a service.",
                    "args Set or get the deployment environment arguments for a service.",
                    "deploy Deploy enabled services to the targeted deployment environment.",
                    "disable Disable a service.",
                    "doctor Check for potential problems.",
                    "enable Enable a service.",
                    "init Initialize a project for Steeltoe Developer Tools.",
                    "list List services, service types, or deployment environments.",
                    "remove Remove a service.",
                    "status Show service statuses.",
                    "target Set or get the targeted deployment environment.",
                    "undeploy Undeploy enabled services from the targeted deployment environment.",
                    $"Run '{Program.Name} [command] --help' for more information about a command.",
                })
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

        [Scenario]
        [Label("version")]
        public void ProgramDebug()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_debug"),
                when => the_developer_runs_cli_command("--debug --version"),
                then => setting_should_be(Settings.DebugEnabled, true)
            );
        }

        [Scenario]
        [Label("version")]
        public void ProgramNoParallel()
        {
            Runner.RunScenario(
                given => a_dotnet_project("program_parallel"),
                when => the_developer_runs_cli_command("--no-parallel --version"),
                then => setting_should_be(Settings.ParallelExecutionEnabled, false)
            );
        }

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
