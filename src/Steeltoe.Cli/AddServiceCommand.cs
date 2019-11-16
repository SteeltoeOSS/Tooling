﻿// Copyright 2018 the original author or authors.
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
    [Command(Description = "Add a service")]
    public class AddServiceCommand : Command
    {
        public const string Name = "add-service";

        [Required(ErrorMessage = "Service type not specified")]
        [Argument(0, Name = "type", Description = "Service type")]
        private string ServiceType { get; } = null;

        [Required(ErrorMessage = "Service name not specified")]
        [Argument(1, Name = "name", Description = "Service name")]
        private string ServiceName { get; } = null;

        public AddServiceCommand(IConsole console) : base(console)
        {
        }

        protected override Executor GetExecutor()
        {
            return new AddServiceExecutor(ServiceName, ServiceType);
        }
    }
}