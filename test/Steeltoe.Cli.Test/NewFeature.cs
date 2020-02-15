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
    public class NewFeature : FeatureSpecs
    {
        [Scenario]
        public void NewHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("new_help"),
                when => the_developer_runs_cli_command("new --help"),
                then => the_cli_should_output(new[]
                {
                    "Creates a new project using Steeltoe Initializr",
                    $"Usage: {Program.Name} new [options]",
                    "Options:",
                    "-n|--name <name> Sets the project name; default is the name of the current directory",
                    "--project-dir <path> Sets the location to place the generated project files; default is the current directory",
                    "-f|--framework <framework> Sets the project framework",
                    "-t|--template <template> Sets the Initializr template",
                    "-d|--dependency <dep>|<dep>:<depname> Adds the named Initializr dependency; may be specified multiple times",
                    "-F|--force Forces project files to be generated even if it would change existing files",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "Create a new project using Steeltoe Initializr. If the output directory exists and is not empty, --force must be specified.",
                    "Examples:",
                    "Create a new project in the current directory:",
                    "$ st new",
                    "Re-create a project in the current directory:",
                    "$ st new --force",
                    "Create a new project in a new directory:",
                    "$ st new --project-dir src/MyProj",
                    "Create a new project with dependencies on SQL Server and Redis:",
                    "$ st new --dependency sqlserver --dep redis",
                    "Create a new project with dependencies on a SQL Server service named MyDB:",
                    "$ st new --dependency sqlserver:MyDB",
                    "Create a new project with a custom name and using the Steeltoe-React template:",
                    "$ st new --name MyCompany.MySample --template Steeltoe-React",
                    "Create a new project for netcoreapp2.2:",
                    "$ st new --framework netcoreapp2.2",
                    "See Also:",
                    "show",
                    "list-templates",
                    "list-deps",
                })
            );
        }

        [Scenario]
        public void NewTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("new_too_many_args"),
                when => the_developer_runs_cli_command("new arg1"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg1'")
            );
        }
    }
}
