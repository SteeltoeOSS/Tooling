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

using System;
using System.IO;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using Shouldly;
using Steeltoe.Tooling.System;

namespace Steeltoe.Tooling.DotnetCli.Base.Feature
{
    public class DotnetCliFeatureFixture : FeatureFixture
    {
        private static ILogger Logger { get; } = Logging.LoggerFactory.CreateLogger<DotnetCliFeatureFixture>();

        private string DotnetCliProjectDirectory { get; } = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Steeltoe.Tooling.DotnetCli"));

        private string ProjectDirectory { get; set; }

        private Shell.Result LastCommandResult { get; set; }

        protected void a_dotnet_project(string name)
        {
            ProjectDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "features"), name);
            if (Directory.Exists(ProjectDirectory))
            {
                Directory.Delete(ProjectDirectory, true);
            }

            Directory.CreateDirectory(ProjectDirectory);
            Logger.LogInformation($"Creating dotnet project '{name}' at {ProjectDirectory}");
            var result = Shell.Run("dotnet", "new classlib", ProjectDirectory);
            result.ExitCode.ShouldBe(0);
        }

        protected void the_developer_runs_steeltoe_(string command)
        {
            Logger.LogInformation($"Running 'steeltoe {command}'");
            LastCommandResult = Shell.Run("dotnet", $"run --project {DotnetCliProjectDirectory} -- {command}",
                ProjectDirectory);
        }

        protected void the_command_succeeds()
        {
            LastCommandResult.ExitCode.ShouldBe(0);
        }

        protected void the_command_fails()
        {
            LastCommandResult.ExitCode.ShouldNotBe(0);
        }

        protected void the_developer_sees_the_output(string message)
        {
            LastCommandResult.Out.ShouldContain(message);
        }

        protected void the_developer_sees_the_error(string message)
        {
            LastCommandResult.Error.ShouldContain(message);
        }
    }
}
