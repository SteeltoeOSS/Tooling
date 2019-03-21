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

using System.Linq;

namespace Steeltoe.Tooling.Executor
{
    /// <summary>
    /// A workflow to list available deployment targets.
    /// </summary>
    public class ListTargetsExecutor : ListExecutor
    {
        /// <summary>
        /// Creates a workflow to list available deployment targets.
        /// </summary>
        /// <param name="verbose">if true, be verbose</param>
        public ListTargetsExecutor(bool verbose = false) : base(verbose)
        {
        }

        internal void ExecuteList(Context context)
        {
            foreach (var envName in Registry.Targets)
            {
                context.Console.WriteLine(envName);
            }
        }

        internal void ExecuteListVerbose(Context context)
        {
            var targets = Registry.Targets;
            var max = targets.Max(n => n.Length);
            var format = "{0,-" + max + "}  {1}";
            foreach (var target in targets)
            {
                var tgtInfo = Registry.GetTarget(target);
                context.Console.WriteLine(format, tgtInfo.Name, tgtInfo.Description);
            }
        }
    }
}
