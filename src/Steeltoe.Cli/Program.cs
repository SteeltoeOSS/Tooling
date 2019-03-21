// Copyright 2018 the original author or authors.
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

using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling;

namespace Steeltoe.Cli
{
    [Command(Name = "steeltoe", Description = "Steeltoe Developer Tools")]
    [VersionOptionFromMember("-V|--version", MemberName = nameof(GetVersion))]
    [Subcommand("init", typeof(InitCommand))]
    [Subcommand("target", typeof(TargetCommand))]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("disable", typeof(DisableCommand))]
    [Subcommand("enable", typeof(EnableCommand))]
    [Subcommand("deploy", typeof(DeployCommand))]
    [Subcommand("undeploy", typeof(UndeployCommand))]
    [Subcommand("status", typeof(StatusCommand))]
    [Subcommand("args", typeof(ArgsCommand))]
    [Subcommand("list", typeof(ListCommand))]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        public static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

        [Option("-C|--config-file", Description =
            "Configure tooling using the specified file instead of .steeltoe.tooling.yml")]
        public static string ConfigurationPath { get; set; } = Directory.GetCurrentDirectory();

        [Option("-D|--debug", Description = "Enable debug output")]
        public static bool DebugEnabled
        {
            get => Settings.DebugEnabled;
            set => Settings.DebugEnabled = value;
        }

        private static ConfigurationFile _configurationFile;

        public static ConfigurationFile ConfigurationFile =>
            _configurationFile ?? (_configurationFile = new ConfigurationFile(ConfigurationPath));

        // ReSharper disable once UnusedMember.Local
        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
