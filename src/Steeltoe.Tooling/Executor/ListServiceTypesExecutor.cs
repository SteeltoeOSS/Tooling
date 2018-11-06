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
    public class ListServiceTypesExecutor : ListExecutor
    {
        public ListServiceTypesExecutor(bool verbose = false) : base(verbose)
        {
        }

        protected void ExecuteList(Context context)
        {
            foreach (var svcType in Registry.GetServiceTypes())
            {
                context.Console.WriteLine(svcType);
            }
        }

        protected void ExecuteListVerbose(Context context)
        {
            var svcTypeNames = Registry.GetServiceTypes();
            var max = svcTypeNames.Max(n => n.Length);
            var format = "{0,-" + max + "}  {1,5}  {2}";
            foreach (var svcTypeName in svcTypeNames)
            {
                var svcTypeInfo = Registry.GetServiceTypeInfo(svcTypeName);
                context.Console.WriteLine(format, svcTypeInfo.Name, svcTypeInfo.Port, svcTypeInfo.Description);
            }
        }
    }
}
