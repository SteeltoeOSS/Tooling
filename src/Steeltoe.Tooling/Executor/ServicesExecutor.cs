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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steeltoe.Tooling.Executor
{
    [RequiresInitialization]
    public abstract class ServicesExecutor : IExecutor
    {
        protected List<string> ServiceNames { get; private set; }

        public virtual void Execute(Context context)
        {
            ServiceNames = context.ServiceManager.GetServiceNames();
            if (ServiceNames.Count == 0)
            {
                return;
            }

            Parallel.ForEach(ServiceNames, svcName =>
            {
                Execute(context, svcName);
            });
        }

        protected abstract void Execute(Context context, string serviceName);
    }
}
