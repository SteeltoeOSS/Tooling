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
    /// <summary>
    /// A workflow to add an application or service to the Steeltoe Tooling configuration.
    /// </summary>
    [RequiresInitialization]
    public class AddExecutor : Executor
    {
        private readonly string _name;

        private readonly string _serviceType;

        /// <summary>
        /// Create a new workflow to add an application to the Steeltoe Tooling configuration.
        /// </summary>
        /// <param name="appName">Application name.</param>
        public AddExecutor(string appName)
        {
            _name = appName;
            _serviceType = null;
        }

        /// <summary>
        /// Create a new workflow to add a service to the Steeltoe Tooling configuration.
        /// </summary>
        /// <param name="serviceName">Service name.</param>
        /// <param name="serviceType">Service type.</param>
        public AddExecutor(string serviceName, string serviceType)
        {
            _name = serviceName;
            _serviceType = serviceType;
        }

        /// <summary>
        /// Add the application or service to the Steeltoe Tooling configuration.
        /// </summary>
        protected override void Execute()
        {
            if (_serviceType == null)
            {
                Context.Configuration.AddApp(_name);
                Context.Console.WriteLine($"Added app '{_name}'");
            }
            else
            {
                Context.Configuration.AddService(_name, _serviceType);
                Context.Console.WriteLine($"Added {_serviceType} service '{_name}'");
            }
        }
    }
}
