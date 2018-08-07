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

namespace Steeltoe.Tooling.System.Test
{
    public class ShellTest
    {
        private static string testDir;
        private static string aDir;
        private static string anotherDir;
        private static string listCommand;
        private static string errorCommand;

        private Shell sh;

        static ShellTest()
        {
            // setup a test sandbox
            testDir = Path.Combine(Directory.GetCurrentDirectory(), "shelltest");
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
                errorCommand = "/usr/false";
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

        public ShellTest()
        {
            sh = new Shell();
        }

        [Fact]
        public void TestConstructor()
        {
            sh.Command.ShouldBeNull();
            sh.Arguments.ShouldBeNull();
            sh.ExitCode.ShouldBe(0);
            sh.Out.ShouldBeNull();
            sh.Error.ShouldBeNull();
        }

        [Fact]
        public void TestRun()
        {
            sh.Run(listCommand);
            sh.Command.ShouldBe(listCommand);
            sh.Arguments.ShouldBeNull();
            sh.ExitCode.ShouldBe(0);
            sh.Out.ShouldNotBeEmpty();
            sh.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunWithArguments()
        {
            sh.Run(listCommand, aDir);
            sh.Command.ShouldBe(listCommand);
            sh.Arguments.ShouldBe(aDir);
            sh.ExitCode.ShouldBe(0);
            sh.Out.ShouldContain("aFile");
            sh.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunWithWorkingDirectory()
        {
            sh.Run(listCommand, workingDirectory: anotherDir);
            sh.Command.ShouldBe(listCommand);
            sh.Arguments.ShouldBeNull();
            sh.ExitCode.ShouldBe(0);
            sh.Out.ShouldContain("anotherFile");
            sh.Error.ShouldBeEmpty();
        }

        [Fact]
        public void TestRunCommandError()
        {
            sh.Run(errorCommand);
            sh.ExitCode.ShouldNotBe(0);
        }

        [Fact]
        public void TestRunSystemError()
        {
            Assert.Throws<Win32Exception>(() => sh.Run("NoSuchCommand"));
        }
    }
}
