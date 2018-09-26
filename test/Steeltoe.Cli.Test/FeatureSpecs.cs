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
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
    public class FeatureSpecs : FeatureFixture
    {
        private string CliProjectDirectory { get; } = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Steeltoe.Cli"));

        protected enum ErrorCode
        {
            Argument = 1,
            Tooling = 2
        }

        protected static ILogger Logger { get; } = Logging.LoggerFactory.CreateLogger<FeatureSpecs>();

        protected readonly Shell Shell = new CommandShell();

        protected Shell.Result ShellResult;

        protected string ShellOut;

        protected string ShellError;

        protected string ProjectDirectory;

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
            File.Create(Path.Combine(ProjectDirectory, ".steeltoe.dummies")).Dispose();
        }

        protected void a_steeltoe_project(string name)
        {
            a_dotnet_project(name);
            Logger.LogInformation($"enabling steeltoe developer tools");
            var cfgFile = new ProjectConfigurationFile(ProjectDirectory);
            cfgFile.ProjectConfiguration.EnvironmentName = "dummy-env";
            cfgFile.Store();
        }

        //
        // Whens
        //

        protected void the_developer_runs_cli_command(string command)
        {
            Logger.LogInformation($"checking the developer runs cli command '{command}'");
            ShellResult = Shell.Run("dotnet", $"run --project {CliProjectDirectory} -- {command}",
                ProjectDirectory);
            ShellOut = string.Join(" ", ShellResult.Out.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
            ShellError = string.Join(" ",
                ShellResult.Error.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
        }

        //
        // Thens
        //

        protected void the_cli_command_should_succeed()
        {
            Logger.LogInformation($"checking the command succeeded");
            ShellResult.ExitCode.ShouldBe(0);
        }

        protected void the_cli_should_output(string message)
        {
            the_cli_command_should_succeed();
            Logger.LogInformation($"checking the cli output '{message}'");
            ShellOut.ShouldContain(message);
        }

        protected void the_cli_should_output_nothing()
        {
            the_cli_command_should_succeed();
            Logger.LogInformation($"checking the cli output nothing");
            ShellOut.ShouldBeEmpty();
        }

        protected void the_cli_should_error(ErrorCode code, string error)
        {
            Logger.LogInformation($"checking the command failed with {(int) code}");
            ShellResult.ExitCode.ShouldBe((int) code);
            Logger.LogInformation($"checking the cli errored '{error}'");
            ShellError.ShouldContain(error);
        }

        protected void the_file_should_exist(string path)
        {
            Logger.LogInformation($"checking the file {path} exists");
            File.Exists(Path.Combine(ProjectDirectory, path)).ShouldBeTrue();
        }

        protected void the_cli_should_list(string[] messages)
        {
            the_cli_command_should_succeed();
            var reader = new StringReader(ShellResult.Out);
            foreach (string message in messages)
            {
                var line = reader.ReadLine();
                line.ShouldBe(message);
            }

            reader.ReadLine().ShouldBeNullOrEmpty();
        }

        protected void the_configuration_should_target(string env)
        {
            Logger.LogInformation($"checking the target config '{env}' exists");
            new ProjectConfigurationFile(ProjectDirectory).ProjectConfiguration.EnvironmentName.ShouldBe(env);
        }

        protected void the_configuration_should_contain_service(string service)
        {
            Logger.LogInformation($"checking the service '{service}' exists");
            new ProjectConfigurationFile(ProjectDirectory).ProjectConfiguration.Services.Keys.ShouldContain(service);
        }

        protected void the_configuration_should_not_contain_service(string service)
        {
            Logger.LogInformation($"checking the service '{service}' does not exist");
            new ProjectConfigurationFile(ProjectDirectory).ProjectConfiguration.Services.Keys.ShouldNotContain(service);
        }

        protected void the_configuration_service_should_be_enabled(string service)
        {
            Logger.LogInformation($"checking the service '{service}' is enabled");
            new ProjectConfigurationFile(ProjectDirectory).ProjectConfiguration.Services[service].Enabled.ShouldBeTrue();
        }

        protected void the_configuration_service_should_not_be_enabled(string service)
        {
            Logger.LogInformation($"checking the service '{service}' is not enabled");
            new ProjectConfigurationFile(ProjectDirectory).ProjectConfiguration.Services[service].Enabled.ShouldBeFalse();
        }
    }
}
