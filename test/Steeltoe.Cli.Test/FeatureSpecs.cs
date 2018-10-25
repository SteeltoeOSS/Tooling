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
using System.Collections.Generic;
using System.IO;
using LightBDD.XUnit2;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
    public class FeatureSpecs : FeatureFixture
    {
        protected enum ErrorCode
        {
            Argument = 1,
            Tooling = 2
        }

        protected static ILogger Logger { get; } = Logging.LoggerFactory.CreateLogger<FeatureSpecs>();

        protected string ProjectDirectory;

        protected readonly MockConsole Console = new MockConsole();

        protected readonly Shell Shell = new CommandShell();

        protected int CommandExitCode;

        protected Exception CommandException;

        static FeatureSpecs()
        {
            Settings.DummiesEnabled = true;
        }

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

            Logger.LogInformation($"creating project directory '${ProjectDirectory}'");
            Directory.CreateDirectory(ProjectDirectory);
            var csprojFile = Path.Combine(ProjectDirectory, $"{name}.csproj");
            File.WriteAllText(csprojFile, @"<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
        <TargetFramework>netstandard2.0</TargetFramework>
    </PropertyGroup>

</Project>
");
            var sourceCodeFile = Path.Combine(ProjectDirectory, "Class1.cs");
            File.WriteAllText(sourceCodeFile, @"using System;

namespace scratch
{
    public class Class1
    {
    }
}
");
            File.Create(Path.Combine(ProjectDirectory, ".steeltoe.dummies")).Dispose();
        }

        protected void a_steeltoe_project(string name)
        {
            a_dotnet_project(name);
            Logger.LogInformation($"enabling steeltoe developer tools");
            var cfgFile = new ToolingConfigurationFile(ProjectDirectory);
            cfgFile.ToolingConfiguration.EnvironmentName = "dummy-env";
            cfgFile.Store();
        }

        //
        // Whens
        //

        protected void the_developer_runs_cli_command(string command)
        {
            Logger.LogInformation($"checking the developer runs cli command '{command}'");
            var svcs = new ServiceCollection()
                .AddSingleton<IConsole>(Console)
                .BuildServiceProvider();
            var buf = Console.Out as StringWriter;
            buf?.GetStringBuilder().Clear();
            var app = new CommandLineApplication<Program>(Console, ProjectDirectory, true);
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(svcs);

            CommandException = null;
            var args = command.Split(null);
            try
            {
                CommandExitCode = app.Execute(args);
                Logger.LogInformation($"command exited with '{CommandExitCode}'");
                if (CommandExitCode != 0)
                {
                    Logger.LogInformation($"command error message was '{Console.Error}'");
                }
            }
            catch (Exception e)
            {
                CommandException = e;
                Logger.LogInformation($"command errored with '{CommandException}'");
            }
        }

        //
        // Thens
        //

        protected void the_cli_command_should_succeed()
        {
            Logger.LogInformation($"checking the CLI command succeeded");
            CommandException.ShouldBeNull();
            CommandExitCode.ShouldBe(0);
        }

        protected void the_cli_should_output(string[] messages)
        {
            Logger.LogInformation($"checking the CLI output");
            the_cli_command_should_succeed();
            var actual = new List<string>();
            using (var reader = new StringReader(Console.Out.ToString()))
            {
                while (true)
                {
                    var line = NormalizeString(reader.ReadLine());
                    if (line == null)
                    {
                        break;
                    }

                    if (line != string.Empty)
                    {
                        actual.Add(line);
                    }
                }
            }

            messages.ShouldBe(actual.ToArray());
        }

        protected void the_cli_should_output(string message)
        {
            the_cli_should_output(new[] {message});
        }

        protected void the_cli_output_should_include(string message)
        {
            the_cli_command_should_succeed();
            Logger.LogInformation($"checking the cli output includes '{message}'");
            NormalizeString(Console.Out.ToString()).ShouldContain(message);
        }

        protected void the_cli_should_output_nothing()
        {
            the_cli_command_should_succeed();
            Logger.LogInformation($"checking the cli output nothing");
            Console.Out.ToString().Trim().ShouldBeEmpty();
        }

        protected void the_cli_should_error(ErrorCode code, string error)
        {
            Logger.LogInformation($"checking the command failed with {(int) code}");
            CommandExitCode.ShouldBe((int) code);
            Logger.LogInformation($"checking the cli errored '{error}'");
            NormalizeString(Console.Error.ToString()).ShouldBe(error);
        }

        protected void the_cli_should_fail_parse(string error)
        {
            CommandException.ShouldBeOfType<CommandParsingException>();
        }

        protected void the_file_should_exist(string path)
        {
            Logger.LogInformation($"checking the file {path} exists");
            File.Exists(Path.Combine(ProjectDirectory, path)).ShouldBeTrue();
        }

        protected void setting_should_be(bool setting, bool expected)
        {
            setting.ShouldBe(expected);
        }

        protected void the_configuration_should_target(string env)
        {
            Logger.LogInformation($"checking the target config '{env}' exists");
            new ToolingConfigurationFile(ProjectDirectory).ToolingConfiguration.EnvironmentName.ShouldBe(env);
        }

        protected void the_configuration_should_contain_service(string service)
        {
            Logger.LogInformation($"checking the service '{service}' exists");
            new ToolingConfigurationFile(ProjectDirectory).ToolingConfiguration.Services.Keys.ShouldContain(service);
        }

        protected void the_configuration_should_not_contain_service(string service)
        {
            Logger.LogInformation($"checking the service '{service}' does not exist");
            new ToolingConfigurationFile(ProjectDirectory).ToolingConfiguration.Services.Keys.ShouldNotContain(service);
        }

        protected void the_configuration_service_should_be_enabled(string service)
        {
            Logger.LogInformation($"checking the service '{service}' is enabled");
            new ToolingConfigurationFile(ProjectDirectory).ToolingConfiguration.Services[service].Enabled
                .ShouldBeTrue();
        }

        protected void the_configuration_service_should_not_be_enabled(string service)
        {
            Logger.LogInformation($"checking the service '{service}' is not enabled");
            new ToolingConfigurationFile(ProjectDirectory).ToolingConfiguration.Services[service].Enabled
                .ShouldBeFalse();
        }

        private static string NormalizeString(string s)
        {
            if (s == null)
            {
                return null;
            }

            return string.Join(" ", s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
