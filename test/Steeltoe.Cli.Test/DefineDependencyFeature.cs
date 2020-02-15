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
    public class DefineDependencyFeature : FeatureSpecs
    {
        [Scenario]
        public void DefineDependencyHelp()
        {
            Runner.RunScenario(
                given => a_dotnet_project("define_dependency_help"),
                when => the_developer_runs_cli_command("def-dep --help"),
                then => the_cli_command_should_succeed(),
                and => the_cli_should_output(new[]
                {
                    "Adds a custom dependency definition",
                    $"Usage: {Program.Name} def-dep [arguments] [options]",
                    "Arguments:",
                    "dep Dependency",
                    "image Docker image",
                    "Options:",
                    "-p|--port <port> Sets a network port; may be specified multiple times",
                    "--nuget-package <name> Sets a NuGet package name for autodetection; may be specified multiple times",
                    "--scope <scope> Sets the dependency definition scope (one of: project, global); default is project",
                    "-?|-h|--help Show help information",
                    "Overview:",
                    "*** under construction ***",
                    "Examples:",
                    "Add a dependency definition for a service that listens on a couple of network ports:",
                    "$ st def-dep MyService myrepo/myimage --port 9876 --port 9877",
                    "Add a dependency definition for a service that can used in all projects:",
                    "$ st def-dep MyService myrepo/myimage --scope global",
                    "Add a dependency definition for a service that can be autodetected:",
                    "$ st def-dep MyService myrepo/myimage --nuget-package My.Service.NuGet",
                    "See Also:",
                    "undef-dep",
                    "list-deps",
                })
            );
        }

        [Scenario]
        public void DefineDependencyNotEnoughArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("define_dependency_not_enough_args"),
                when => the_developer_runs_cli_command("def-dep"),
                then => the_cli_should_error(ErrorCode.Argument, "Dependency not specified"),
                when => the_developer_runs_cli_command("def-dep arg1"),
                then => the_cli_should_error(ErrorCode.Argument, "Docker image not specified")
            );
        }

        [Scenario]
        public void RemoveDependencyTooManyArgs()
        {
            Runner.RunScenario(
                given => a_dotnet_project("define_dependency_too_many_args"),
                when => the_developer_runs_cli_command("def-dep arg1 arg2 arg3"),
                then => the_cli_should_fail_parse("Unrecognized command or argument 'arg3'")
            );
        }
    }
}
