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

using System;
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executor;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Steeltoe.Cli
{
    [Command(Description = "List services, service types, or deployment environments.",
        ExtendedHelpText = "If run with no options, list services.")]
    public class ListCommand : Command
    {
        [Option("-e|--environments", Description = "List deployment environments")]
        private bool ListEnvironments { get; }

        [Option("-s|--services", Description = "List services")]
        private bool ListServices { get; }

        [Option("-t|--service-types", Description = "List service types")]
        private bool ListServiceTypes { get; }

        [Option("-v|--verbose", Description = "Verbose")]
        private bool Verbose { get; }

        protected override IExecutor GetExecutor()
        {
            var optionCount = 0;
            IExecutor executor = new ListServicesExecutor(Verbose);
            if (ListServices)
            {
                ++optionCount;
            }

            if (ListServiceTypes)
            {
                executor = new ListServiceTypesExecutor(Verbose);
                ++optionCount;
            }

            if (ListEnvironments)
            {
                executor = new ListEnvironmentsExecutor(Verbose);
                ++optionCount;
            }

            if (optionCount > 1)
            {
                throw new ArgumentException(
                    "Specify at most one of: -e|--environments, -s|--services, -t|--service-types");
            }

            return executor;
        }
    }
}
