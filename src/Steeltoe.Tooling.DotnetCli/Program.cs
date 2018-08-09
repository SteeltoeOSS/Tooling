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

using McMaster.Extensions.CommandLineUtils;

namespace Steeltoe.Tooling.DotnetCli
{
    [Command(Name = "steeltoe", Description = "Steeltoe Developer Tools")]
    [Subcommand("define-service", typeof(Service.DefineCommand))]
    [Subcommand("undefine-service", typeof(Service.UndefineCommand))]
    [Subcommand("list-services", typeof(Service.ListCommand))]
    [Subcommand("start-service", typeof(Service.StartCommand))]
    [Subcommand("set-target", typeof(Target.SetCommand))]
    [Subcommand("list-targets", typeof(Target.ListCommand))]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}
