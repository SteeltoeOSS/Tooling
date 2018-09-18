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

        protected Shell _shell = new CommandShell();

        protected Shell.Result _shellResult;

        protected string _shellOut;

        protected string _shellError;

        protected string _projectDirectory;

        //
        // Givens
        //

        protected void a_dotnet_project(string name)
        {
            Logger.LogInformation($"rigging a dotnet project '{name}'");
            _projectDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "sandboxes"), name);
            if (Directory.Exists(_projectDirectory))
            {
                Directory.Delete(_projectDirectory, true);
            }

            Directory.CreateDirectory(_projectDirectory);
            _shell.Run("dotnet", "new classlib", _projectDirectory).ExitCode.ShouldBe(0);
            File.Create(Path.Combine(_projectDirectory, ".steeltoe.dummies")).Dispose();
        }

        protected void a_steeltoe_project(string name)
        {
            a_dotnet_project(name);
            Logger.LogInformation($"enabling steeltoe developer tools");
            var cfg = new ConfigurationFile(_projectDirectory);
            cfg.EnvironmentName = "dummy-env";
            cfg.Store();
        }

        //
        // Whens
        //

        protected void the_developer_runs_cli_command(string command)
        {
            Logger.LogInformation($"checking the developer runs cli command '{command}'");
            _shellResult = _shell.Run("dotnet", $"run --project {CliProjectDirectory} -- {command}",
                _projectDirectory);
            _shellOut = string.Join(" ", _shellResult.Out.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
            _shellError = string.Join(" ",
                _shellResult.Error.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
        }

        //
        // Thens
        //

        protected void the_cli_command_should_succeed()
        {
            Logger.LogInformation($"checking the command succeeded");
            _shellResult.ExitCode.ShouldBe(0);
        }

        protected void the_cli_should_output(string message)
        {
            the_cli_command_should_succeed();
            Logger.LogInformation($"checking the cli output '{message}'");
            _shellOut.ShouldContain(message);
        }

        protected void the_cli_should_error(ErrorCode code, string error)
        {
            Logger.LogInformation($"checking the command failed with {(int) code}");
            _shellResult.ExitCode.ShouldBe((int) code);
            Logger.LogInformation($"checking the cli errored '{error}'");
            _shellError.ShouldContain(error);
        }
    }
}
