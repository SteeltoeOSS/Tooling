// Copyright 2020 the original author or authors.
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

namespace Steeltoe.Cli.Test
{
    public class NewConfigurationFeature : FeatureSpecs
    {
        [Scenario]
        public void NewConfigurationHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("generate_configuration_help"),
                when => the_developer_runs_cli_command("new-cfg --help"),
                then => the_cli_should_output(new[]
                {
                    "Creates configuration files for a target",
                    $"Usage: {Program.Name} new-cfg [options]",
                    "Options:",
                    "-T|--target <target> Sets the configuration target (one of: ‘Docker’, ‘Kubernetes’, ‘Cloud-Foundry’); default is 'Docker'",
                    "-n|--name <cfgname> Sets the configuration name; default is the project name",
                    "-o|--output <path> Sets the location to place the generated configuration; default is the current directory",
                    "-f|--framework <framework> Sets the framework; default is the project framework",
                    "-a|--arg <arg> Sets a command line argument for the application",
                    "-e|--env <name>=<value> Sets an environment variable for the application; may be specified multiple times",
                    "--dep-arg <depname>:<arg> Sets a command line argument for the named dependency",
                    "--dep-env <depname>:<name>=<value> Sets an environment variable for the named dependency; may be specified multiple times",
                    "-F|--force Forces configuration to be generated even if it would change existing files",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Creates configuration files for a target. By default, files are generated for the Docker target.",
                    "Docker configurations can subsequently be used by the run command.",
                    "Examples:",
                    "Create the default configuration:",
                    "$ st new-cfg",
                    "Re-create the default configuration for a project in a specific directory:",
                    "$ st new-cfg --force --project-dir src/MyProj",
                    "Create a configuration for Kubernetes using the netcoreapp2.1 framework:",
                    "$ st new-cfg --target k8s --framework netcoreapp2.1",
                    "Create a named configuration with application arguments:",
                    "$ st new-cfg --name MyCustomDockerConfig --application-arg arg1 --application-arg arg2",
                    "Create a configuration that sets an environment variable for a dependency:",
                    "$ st new-cfg --dependency-env MyDB:ACCEPT_EULA=Y",
                    "See Also:",
                    "show-cfg",
                    "run",
                })
            );
        }

        [Scenario]
        public void NewConfigurationTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("new_configuration_too_many_args"),
                when => the_developer_runs_cli_command("new-cfg arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
