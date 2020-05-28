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

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Settings.DebugEnabled = true;
        }

        //
        // Givens
        //

        protected void an_empty_directory(string name)
        {
            Logger.LogInformation($"creating empty directory '{ProjectDirectory}'");
            ProjectDirectory = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "sandboxes"), name);
            if (Directory.Exists(ProjectDirectory))
            {
                Directory.Delete(ProjectDirectory, true);
            }

            Directory.CreateDirectory(ProjectDirectory);
        }

        protected void a_dotnet31_project(string name)
        {
            an_empty_directory(name);
            Logger.LogInformation($"rigging a dotnet project '{name}'");
            var p = new Process
            {
                StartInfo = new ProcessStartInfo("dotnet", "new web --framework netcoreapp3.1")
                {
                    WorkingDirectory = ProjectDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
            };
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                throw new CommandException(p.ExitCode, p.StandardError.ReadToEnd());
            }
        }

        protected void a_steeltoe_project(string name)
        {
            a_dotnet31_project(name);
            Logger.LogInformation($"enabling steeltoe developer tools");
            // var cfgFile = new ConfigurationFile(ProjectDirectory);
            // cfgFile.Store();
        }

        //
        // Whens
        //

        protected void the_developer_runs_cli_command(string command)
        {
            Logger.LogInformation($"developer runs cli command '{command}'");
            var svcs = new ServiceCollection()
                .AddSingleton<IConsole>(Console)
                .BuildServiceProvider();
            var stdout = Console.Out as StringWriter;
            stdout?.GetStringBuilder().Clear();
            var stderr = Console.Error as StringWriter;
            stderr?.GetStringBuilder().Clear();
            var app = new CommandLineApplication<Program>(Console, ProjectDirectory, true);
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(svcs);

            CommandException = null;
            var args = command.Split();
            if (args.Length == 1 && args[0].Length == 0)
            {
                args = new string[0];
            }

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

            var actualMessages = actual.ToArray();

            // '*' denotes we don't care about this line
            // '***' denotes we don't care about any further lines
            for (var i = 0; i < messages.Length; ++i)
            {
                if (messages[i] == "***")
                {
                    Array.Resize(ref messages, i);
                    Array.Resize(ref actualMessages, i);
                    break;
                }

                if (messages[i] == "*")
                {
                    actualMessages[i] = "*";
                }
            }

            actualMessages.ShouldBe(messages);
        }

        protected void the_cli_output_should_include(string message)
        {
            Logger.LogInformation($"checking the cli output includes '{message}'");
            NormalizeString(Console.Out.ToString()).ShouldContain(message);
        }

        protected void the_cli_should_error(ErrorCode code)
        {
            Logger.LogInformation($"checking the command failed with {(int) code}");
            CommandExitCode.ShouldBe((int) code);
        }

        protected void the_cli_should_error(ErrorCode code, string error)
        {
            the_cli_should_error(code);
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

        private void CreateProjectDirectory(string name)
        {
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

    public class CommandException : Exception
    {
        private readonly int _exitCode;
        private readonly string _errorMessage;

        public CommandException(int exitCode, string error)
        {
            _exitCode = exitCode;
            _errorMessage = error;
        }

        public override string Message => $"{_exitCode}: {_errorMessage}";
    }
}
