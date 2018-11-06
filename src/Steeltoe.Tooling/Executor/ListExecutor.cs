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
    [RequiresInitialization]
    public class ListExecutor : GroupExecutor
    {
        private readonly bool _verbose;

        private string _format;

        public ListExecutor(bool verbose = false) : base(false)
        {
            _verbose = verbose;
        }

        protected override void Execute()
        {
            var services = Context.Configuration.GetServices();
            if (_verbose)
            {
                var max = services.Max(n => n.Length);
                _format = "{0,-" + max + "}  {1,5}  {2}";
            }
            else
            {
                _format = "{0}";
            }
            base.Execute();
        }

        protected override void ExecuteForApp(string app)
        {
            Context.Console.WriteLine(_format, app, "", "app");
        }

        protected override void ExecuteForService(string service)
        {
            var svcInfo = Context.Configuration.GetServiceInfo(service);
            var svcTypeInfo = Registry.GetServiceTypeInfo(svcInfo.ServiceType);
            Context.Console.WriteLine(_format, service, svcTypeInfo.Port, svcTypeInfo.Name);
        }
    }
}
