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

namespace Steeltoe.Cli
{
    [Command(Name = "steeltoe", Description = "Steeltoe Developer Tools")]
    [VersionOptionFromMember("-V|--version", MemberName = nameof(GetVersion))]
    [Subcommand("target", typeof(TargetCommand))]
    [Subcommand("add", typeof(AddCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("disable", typeof(DisableCommand))]
    [Subcommand("enable", typeof(EnableCommand))]
    [Subcommand("deploy", typeof(DeployCommand))]
    [Subcommand("undeploy", typeof(UndeployCommand))]
    [Subcommand("status", typeof(StatusCommand))]
    [Subcommand("list", typeof(ListCommand))]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        public static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        // ReSharper disable once UnusedMember.Local
        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
