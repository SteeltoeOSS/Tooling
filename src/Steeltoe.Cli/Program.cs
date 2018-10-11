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

using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling;

namespace Steeltoe.Cli
{
    [Command(Name = Name, Description = "Steeltoe Developer Tools")]
    [VersionOptionFromMember("-V|--version", MemberName = nameof(GetVersion))]
    [Subcommand(InitCommand.Name, typeof(InitCommand))]
    [Subcommand(TargetCommand.Name, typeof(TargetCommand))]
    [Subcommand(AddCommand.Name, typeof(AddCommand))]
    [Subcommand(RemoveCommand.Name, typeof(RemoveCommand))]
    [Subcommand(DisableCommand.Name, typeof(DisableCommand))]
    [Subcommand(EnableCommand.Name, typeof(EnableCommand))]
    [Subcommand(DeployCommand.Name, typeof(DeployCommand))]
    [Subcommand(UndeployCommand.Name, typeof(UndeployCommand))]
    [Subcommand(StatusCommand.Name, typeof(StatusCommand))]
    [Subcommand(ArgsCommand.Name, typeof(ArgsCommand))]
    [Subcommand(ListCommand.Name, typeof(ListCommand))]
    [Subcommand(DoctorCommand.Name, typeof(DoctorCommand))]
    public class Program
    {
        public const string Name = "st";

        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        public static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

        [Option("-C|--config-file", Description =
            "Configure tooling using the specified file instead of .steeltoe.tooling.yml")]
        public static string ProjectConfigurationPath { get; }

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
