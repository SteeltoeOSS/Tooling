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
            Logger.LogInformation($"rigging a dotnet project '{name}'");
            ProjectDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "sandboxes"), name);
            if (Directory.Exists(ProjectDirectory))
            {
                Directory.Delete(ProjectDirectory, true);
            }

            Directory.CreateDirectory(ProjectDirectory);
            Shell.Run("dotnet", "new classlib", ProjectDirectory).ExitCode.ShouldBe(0);
        }

        protected void a_target(string name)
        {
            Logger.LogInformation($"rigging a target '{name}'");
            var cfg = GetProjectConfiguration();
            cfg.target = name;
            cfg.Store(ProjectDirectory);
        }

        protected void a_service(string name)
        {
            Logger.LogInformation($"rigging a service '{name}'");
            var cfg = GetProjectConfiguration();
            cfg.services.Add(name, new ToolingConfiguration.Service("foo-type"));
            cfg.Store(ProjectDirectory);
        }

        //
        // Whens
        //

        protected void the_developer_runs_steeltoe_command(string command)
        {
            Logger.LogInformation($"running 'steeltoe {command}'");
            LastCommandResult = Shell.Run("dotnet", $"run --project {DotnetCliProjectDirectory} -- {command}",
                ProjectDirectory);
        }

        //
        // Thens
        //

        protected void the_command_should_succeed()
        {
            Logger.LogInformation($"checking the command succeeded");
            LastCommandResult.ExitCode.ShouldBe(0);
        }

        protected void the_command_should_fail()
        {
            Logger.LogInformation($"checking the command failed");
            LastCommandResult.ExitCode.ShouldNotBe(0);
        }

        protected void the_developer_should_see(string message)
        {
            Logger.LogInformation($"checking the developer saw '{message}'");
            LastCommandResult.Out.ShouldMatch($".*{message}.*");
        }

        protected void the_developer_should_see_the_error(string error)
        {
            Logger.LogInformation($"checking the developer saw the error '{error}'");
            LastCommandResult.Error.ShouldMatch($".*{error}.*");
        }

        protected void the_target_config_should_exist(string name)
        {
            Logger.LogInformation($"checking the target config '{name}' exists");
            var cfg = ToolingConfiguration.Load(ProjectDirectory);
            cfg.target.ShouldBe(name);
        }

        protected void the_service_config_should_exist(string name, string type)
        {
            Logger.LogInformation($"checking the service config '{name}' exists");
            var cfg = ToolingConfiguration.Load(ProjectDirectory);
            cfg.services.ShouldContainKey(name);
            cfg.services[name].type.ShouldBe(type);
        }

        protected void the_service_config_should_not_exist(string name)
        {
            Logger.LogInformation($"checking the service config '{name}' does not exist");
            var cfg = ToolingConfiguration.Load(ProjectDirectory);
            cfg.services.ShouldNotContainKey(name);
        }

        // utilities

        private ToolingConfiguration GetProjectConfiguration()
        {
            try
            {
                return ToolingConfiguration.Load(ProjectDirectory);

            }
            catch (FileNotFoundException)
            {
                return new ToolingConfiguration();
            }
        }
    }
}
