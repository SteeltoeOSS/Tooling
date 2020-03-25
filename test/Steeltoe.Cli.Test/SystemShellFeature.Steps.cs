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
using System.IO;
using System.Runtime.InteropServices;
using LightBDD.XUnit2;
using Shouldly;
using Steeltoe.Tooling;

namespace Steeltoe.Cli.Test
{
    public partial class SystemShellFeature : FeatureFixture
    {
        private static readonly string ListCommand;

        private static readonly string ErrorCommand;

        private Shell _shell = new CommandShell();

        private Shell.Result _result;

        private Exception _exception;

        static SystemShellFeature()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var binDir = Path.Combine(Directory.GetCurrentDirectory(), "sandboxes/shell.bin");
                Directory.CreateDirectory(binDir);
                ListCommand = Path.Combine(binDir, "list.bat");
                File.WriteAllText(ListCommand, "@ECHO OFF\ndir %*\n");
                ErrorCommand = Path.Combine(binDir, "error.bat");
                File.WriteAllText(ErrorCommand, "@ECHO OFF\nexit /B 1\n");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ListCommand = "/bin/ls";
                ErrorCommand = "/usr/bin/false";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ListCommand = "/bin/ls";
                ErrorCommand = "/bin/false";
            }
            else
            {
                throw new Exception("Don't know how to setup commands on " + RuntimeInformation.OSDescription);
            }
        }

        //
        // Givens
        //

        protected void a_directory(string path)
        {
            Directory.CreateDirectory(path);
        }

        protected void a_file(string path)
        {
            File.WriteAllText(path, string.Empty);
        }

        //
        // Whens
        //

        protected void the_command_is_run(string command)
        {
            try
            {
                _result = _shell.Run(command);
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        protected void the_command_is_run_with_args(string command, string args)
        {
            _result = _shell.Run(command, args);
        }

        protected void the_command_is_run_with_working_directory(string command, string path)
        {
            _result = _shell.Run(command, workingDirectory: path);
        }

        //
        // Thens
        //

        protected void the_command_should_succeed()
        {
            _result.ExitCode.ShouldBe(0);
        }

        protected void the_command_should_fail()
        {
            _result.ExitCode.ShouldNotBe(0);
        }

        protected void the_command_output_should_not_be_empty()
        {
            _result.Out.ShouldNotBeNullOrEmpty();
        }

        protected void the_command_output_should_contain(string text)
        {
            _result.Out.ShouldContain(text);
        }

        protected void the_command_should_raise_exception<T>()
        {
            _exception.ShouldNotBeNull();
            _exception.ShouldBeOfType<T>();
        }
    }
}
