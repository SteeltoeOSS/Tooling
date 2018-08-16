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

using Shouldly;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable InconsistentNaming

namespace Steeltoe.Tooling.System.Test
{
    public class ShellTest
    {
        private static string testDir;
        private static string aDir;
        private static string anotherDir;
        private static string listCommand;
        private static string errorCommand;

        static ShellTest()
        {
            // setup a test sandbox
            testDir =  Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "sandboxes"), "shell-test");
            Directory.CreateDirectory(testDir);
            // setup up list and error commands
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                listCommand = Path.Combine(testDir, "list.bat");
                File.WriteAllText(listCommand, "@ECHO OFF\ndir %*\n");
                errorCommand = Path.Combine(testDir, "error.bat");
                File.WriteAllText(errorCommand, "@ECHO OFF\nexit /B 1\n");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                listCommand = "ls";
                errorCommand = "/usr/bin/false";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                listCommand = "ls";
                errorCommand = "/bin/false";
            }
            else
            {
                throw new Exception("Don't know how to list a directory on " + RuntimeInformation.OSDescription);
            }

            // setup some test directories
            aDir = Path.Combine(testDir, "aDir");
            Directory.CreateDirectory(aDir);
            File.WriteAllText(Path.Combine(aDir, "aFile"), string.Empty);
            anotherDir = Path.Combine(testDir, "anotherDir");
            Directory.CreateDirectory(anotherDir);
            File.WriteAllText(Path.Combine(anotherDir, "anotherFile"), string.Empty);
        }

        [Fact]
        public void TestRun()
        {
            var result = Shell.Run(listCommand);
            result.Command.ShouldBe(listCommand);
            result.Arguments.ShouldBeNull();
            result.ExitCode.ShouldBe(0);
            result.Out.ShouldNotBeEmpty();
            result.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunWithArguments()
        {
            var result = Shell.Run(listCommand, aDir);
            result.Command.ShouldBe(listCommand);
            result.Arguments.ShouldBe(aDir);
            result.ExitCode.ShouldBe(0);
            result.Out.ShouldContain("aFile");
            result.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunWithWorkingDirectory()
        {
            var result = Shell.Run(listCommand, workingDirectory: anotherDir);
            result.Command.ShouldBe(listCommand);
            result.Arguments.ShouldBeNull();
            result.ExitCode.ShouldBe(0);
            result.Out.ShouldContain("anotherFile");
            result.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunCommandError()
        {
            var result = Shell.Run(errorCommand);
            result.ExitCode.ShouldNotBe(0);
        }

        [Fact]
        public void TestRunSystemError()
        {
            Assert.Throws<Win32Exception>(() => Shell.Run("NoSuchCommand"));
        }
    }
}
