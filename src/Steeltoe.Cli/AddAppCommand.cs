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

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executors;

namespace Steeltoe.Cli
{
    [Command(Description = "Add an app")]
    public class AddAppCommand : Command
    {
        public const string Name = "add-app";

        [Required(ErrorMessage = "App name not specified")]
        [Argument(1, Name = "name", Description = "App name")]
        private string AppName { get; } = null;

        [Option("-f|--framework", Description = "Target framework")]
        private string Framework { get; } = "netcoreapp2.1";

        [Option("-r|--runtime", Description = "Target runtime")]
        private string Runtime { get; } = "win10-x64";

        public AddAppCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            return new AddAppExecutor(AppName, Framework, Runtime);
        }
    }
}
