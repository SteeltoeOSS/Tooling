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

using System;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Controllers;

namespace Steeltoe.Cli
{
    [Command(Description = "Displays documentation on a topic",
        ExtendedHelpText = @"
Overview:
  Displays documentation for various topics.  Run with no arguments for a list of available topics.

Examples:
  Display documentation for autodetection:
  $ st show-topic autodetection"
    )]
    public class ShowTopicCommand : Command
    {
        public const string CommandName = "show-topic";

        [Argument(0, Name = "topic", Description = "Topic")]
        private string Topic { get; set; }

        public ShowTopicCommand(IConsole console) : base(console)
        {
        }

        protected override Controller GetController()
        {
            throw new NotImplementedException();
        }
    }
}
