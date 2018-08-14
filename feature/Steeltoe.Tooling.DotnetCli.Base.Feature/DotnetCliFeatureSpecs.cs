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
using Steeltoe.Tooling.Base;
using Steeltoe.Tooling.System;

namespace Steeltoe.Tooling.DotnetCli.Base.Feature
{
    public class DotnetCliFeatureSpecs : FeatureFixture
    {
        private static ILogger Logger { get; } = Logging.LoggerFactory.CreateLogger<DotnetCliFeatureSpecs>();

        private string DotnetCliProjectDirectory { get; } = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Steeltoe.Tooling.DotnetCli"));

        private string ProjectDirectory { get; set; }

        private Shell.Result LastCommandResult { get; set; }

        //
        // Givens
        //

        protected void a_dotnet_project(string name)
        {
            Logger.LogInformation($"creating dotnet project '{name}'");
            ProjectDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "feature-tests"), name);
            if (Directory.Exists(ProjectDirectory))
            {
                Directory.Delete(ProjectDirectory, true);
            }

            Directory.CreateDirectory(ProjectDirectory);
            Logger.LogInformation($"creating dotnet project '{name}' at {ProjectDirectory}");
            var result = Shell.Run("dotnet", "new classlib", ProjectDirectory);
            result.ExitCode.ShouldBe(0);
        }

        //
        // Whens
        //

        protected void the_developer_runs_steeltoe_(string command)
        {
            Logger.LogInformation($"running 'steeltoe {command}'");
            LastCommandResult = Shell.Run("dotnet", $"run --project {DotnetCliProjectDirectory} -- {command}",
                ProjectDirectory);
        }

        //
        // Thens
        //

        protected void the_command_succeeds()
        {
            Logger.LogInformation($"checking the command succeeded");
            LastCommandResult.ExitCode.ShouldBe(0);
        }

        protected void the_command_fails()
        {
            Logger.LogInformation($"checking the command failed");
            LastCommandResult.ExitCode.ShouldNotBe(0);
        }

        protected void the_developer_sees_(string message)
        {
            Logger.LogInformation($"checking the developer saw '{message}'");
            LastCommandResult.Out.ShouldMatch($".*{message}.*");
        }

        protected void the_developer_sees_the_error_(string error)
        {
            Logger.LogInformation($"checking the developer saw the error '{error}'");
            LastCommandResult.Error.ShouldMatch($".*{error}.*");
        }

        protected void the_target_environment_is_(string environment)
        {
            Logger.LogInformation($"checking target environment is '{environment}'");
            var cfg = ToolingConfiguration.Load(ProjectDirectory);
            cfg.target.ShouldBe(environment);
        }
    }
}
