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

using System.Linq;

namespace Steeltoe.Tooling.Executor
{
    public class ListEnvironmentsExecutor : ListExecutor
    {
        public ListEnvironmentsExecutor(bool verbose = false) : base(verbose)
        {
        }

        protected override void ExecuteList(Context context)
        {
            foreach (var envName in Registry.EnvironmentNames)
            {
                context.Console.WriteLine(envName);
            }
        }

        protected override void ExecuteListVerbose(Context context)
        {
            var envNames = Registry.EnvironmentNames;
            var max = envNames.Max(n => n.Length);
            var format = "{0,-" + max + "}  {1}";
            foreach (var envName in Registry.EnvironmentNames)
            {
                var env = Registry.GetEnvironment(envName);
                context.Console.WriteLine(format, env.Name, env.Description);
            }
        }
    }
}
