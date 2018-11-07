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

namespace Steeltoe.Tooling.Executor
{
    // TODO : write unit test for init req
    [RequiresInitialization]
    public class RemoveExecutor : Executor
    {
        private readonly string _name;

        public RemoveExecutor(string name)
        {
            _name = name;
        }

        protected override void Execute()
        {
            var cfg = Context.Configuration;
            if (cfg.GetApps().Contains(_name))
            {
                Context.Configuration.RemoveApp(_name);
                Context.Console.WriteLine($"Removed app '{_name}'");
            }
            else if (cfg.GetServices().Contains(_name))
            {
                var svcInfo = cfg.GetServiceInfo(_name);
                Context.Configuration.RemoveService(_name);
                Context.Console.WriteLine($"Removed {svcInfo.ServiceType} service '{_name}'");

            }
            else
            {
                throw new ItemDoesNotExistException(_name, "app or service");
            }
        }
    }
}
