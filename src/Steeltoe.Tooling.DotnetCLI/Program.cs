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

namespace Steeltoe.Tooling.DotnetCLI
{
    [Command(Name = "steeltoe", Description = "Steeltoe Developer Tools")]
    [Subcommand("doctor", typeof(Doctor.DoctorCommand))]
    [Subcommand("create-service", typeof(Service.CreateServiceCommand))]
    [Subcommand("set-target", typeof(Target.SetTargetCommand))]
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