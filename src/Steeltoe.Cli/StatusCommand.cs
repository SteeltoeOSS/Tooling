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
using Steeltoe.Tooling.Executor;

namespace Steeltoe.Cli
{
    [Command(Description = "Show service statuses.")]
    public class StatusCommand : Command
    {
        public const string Name = "status";

        public StatusCommand(IConsole console) : base(console)
        {
        }

        protected override IExecutor GetExecutor()
        {
            return new StatusServicesExecutor();
        }
    }
}
