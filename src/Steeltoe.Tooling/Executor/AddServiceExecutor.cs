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
    [RequiresInitialization]
    public class AddServiceExecutor : IExecutor
    {
        private string _name;

        private string _type;

        public AddServiceExecutor(string serviceName, string serviceType)
        {
            _name = serviceName;
            _type = serviceType;
        }

        public void Execute(Context context)
        {
            context.ServiceManager.AddService(_name, _type);
            context.ServiceManager.EnableService(_name);
            context.Shell.Console.WriteLine($"Added {_type} service '{_name}'");
        }
    }
}
