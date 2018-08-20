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

using System.IO;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace Steeltoe.Tooling.Cli.Feature
{
    public class CliFeatureSpecs : FeatureFixture
    {
        private static ILogger Logger { get; } = Logging.LoggerFactory.CreateLogger<CliFeatureSpecs>();

        private string CliProjectDirectory { get; } = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Steeltoe.Tooling.Cli"));

        private string ProjectDirectory { get; set; }

        private string ConfigFile { get; set; }

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
            ConfigFile = Path.Combine(ProjectDirectory, Configuration.DefaultFileName);
        }

        protected void a_target(string name)
        {
            Logger.LogInformation($"rigging a target '{name}'");
            var cfg = LoadProjectConfiguration();
            cfg.target = name;
            StoreProjectConfiguration(cfg);
        }

        protected void a_service(string name, string type)
        {
            Logger.LogInformation($"rigging a service '{name}'");
            var cfg = LoadProjectConfiguration();
            cfg.services.Add(name, new Configuration.Service(type));
            StoreProjectConfiguration(cfg);
        }

        //
        // Whens
        //

        protected void the_developer_runs_steeltoe_command(string command)
        {
            Logger.LogInformation($"running 'steeltoe {command}'");
            LastCommandResult = Shell.Run("dotnet", $"run --project {CliProjectDirectory} -- {command}",
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
            var cfg = LoadProjectConfiguration();
            cfg.target.ShouldBe(name);
        }

        protected void the_service_config_should_exist(string name, string type)
        {
            Logger.LogInformation($"checking the service config '{name}' exists");
            var cfg = LoadProjectConfiguration();
            cfg.services.ShouldContainKey(name);
            cfg.services[name].type.ShouldBe(type);
        }

        protected void the_service_config_should_not_exist(string name)
        {
            Logger.LogInformation($"checking the service config '{name}' does not exist");
            var cfg = LoadProjectConfiguration();
            cfg.services.ShouldNotContainKey(name);
        }

        // utilities

        private void StoreProjectConfiguration(Configuration config)
        {
            config.Store(ConfigFile);
        }

        private Configuration LoadProjectConfiguration()
        {
            return File.Exists(ConfigFile) ? Configuration.Load(ConfigFile) : new Configuration();
        }
    }
}
