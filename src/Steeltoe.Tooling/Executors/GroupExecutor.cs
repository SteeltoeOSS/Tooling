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

using System;
using System.Collections.Generic;

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// An abstract workflow operating on a group of applications and dependent services.
    /// </summary>
    public abstract class GroupExecutor : Executor
    {
        private readonly bool _appsFirst;

        /// <summary>
        /// Create a workflow to operate on a group of applications and dependent services.
        /// </summary>
        /// <param name="appsFirst">Whether applications or services are operated on first.</param>
        protected GroupExecutor(Boolean appsFirst)
        {
            _appsFirst = appsFirst;
        }

        /// <summary>
        /// Execute this workflow on the applications and dependent services.
        /// </summary>
        protected override void Execute()
        {
            var services = Context.Configuration.GetServices();
            var apps = Context.Configuration.GetApps();

            if (_appsFirst)
            {
                ExecuteForApps(apps);
                ExecuteForServices(services);
            }
            else
            {
                ExecuteForServices(services);
                ExecuteForApps(apps);
            }
        }

        /// <summary>
        /// Calls <see cref="ExecuteForApp"/> for each app.
        /// </summary>
        /// <param name="apps">Apps on which to be executed</param>
        protected virtual void ExecuteForApps(List<string> apps)
        {
            foreach (var app in apps)
            {
                ExecuteForApp(app);
            }
        }

        /// <summary>
        /// Calls <see cref="ExecuteForService"/> for each service.
        /// </summary>
        /// <param name="services">Services on which to be executed</param>
        protected virtual void ExecuteForServices(List<string> services)
        {
            foreach (var service in services)
            {
                ExecuteForService(service);
            }
        }

        /// <summary>
        /// Subclasses implement their business logic for executing on an app.
        /// </summary>
        /// <param name="app">App on which to be executed.</param>
        protected abstract void ExecuteForApp(string app);

        /// <summary>
        /// Subclasses implement their business logic for executing on a service.
        /// </summary>
        /// <param name="service">Service on which to be executed.</param>
        protected abstract void ExecuteForService(string service);
    }
}
