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

using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling;

namespace Steeltoe.Cli
{
    [Command(Name = Name, Description = "Steeltoe Developer Tools")]
    [VersionOptionFromMember("-V|--version", MemberName = nameof(GetVersion))]
    [Subcommand(HelpCommand.CommandName, typeof(HelpCommand))]
    [Subcommand(RunCommand.CommandName, typeof(RunCommand))]
    [Subcommand(ShowCommand.CommandName, typeof(ShowCommand))]
    [Subcommand(StopCommand.CommandName, typeof(StopCommand))]
    public class Program
    {
        public const string Name = "st";

        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        public static string GetVersion()
        {
            var versionString = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            if (!versionString.Contains('-')) return versionString;
            var version = versionString.Split('-')[0];
            var build = versionString.Split('-')[1];
            return
                $"{version} (build {build} -> https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/results?buildId={build})";
        }

        [Option("-D|--debug", Description = "Enable debug output")]
        public static bool DebugEnabled
        {
            get => Settings.DebugEnabled;
            set => Settings.DebugEnabled = value;
        }

        // ReSharper disable once UnusedMember.Local
        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
