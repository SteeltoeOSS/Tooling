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
using System.Threading;

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// A workflow to deploy applications and dependent services to the current target.
    /// </summary>
    [RequiresTarget]
    public class DeployExecutor : GroupExecutor
    {
        /// <summary>
        /// Create a new workflow to deploy applications and dependent services to the current target.
        /// </summary>
        public DeployExecutor() : base(false)
        {
        }

        /// <summary>
        /// Deploys apps then blocks until all apps online.
        /// </summary>
        /// <param name="apps">Apps to deploy.</param>
        protected override void ExecuteForApps(List<string> apps)
        {
            base.ExecuteForApps(apps);
            WaitUntilAllTransitioned(apps, Context.Driver.GetAppStatus, Lifecycle.Status.Online);
        }

        /// <summary>
        /// Deploys services then blocks until all services online.
        /// </summary>
        /// <param name="services">Services to deploy.</param>
        protected override void ExecuteForServices(List<string> services)
        {
            base.ExecuteForServices(services);
            WaitUntilAllTransitioned(services, Context.Driver.GetServiceStatus, Lifecycle.Status.Online);
        }


        /// <summary>
        /// Deploy the app.
        /// </summary>
        /// <param name="app">App name.</param>
        protected override void ExecuteForApp(string app)
        {
            Context.Console.WriteLine($"Deploying app '{app}'");
            new Lifecycle(Context, app).Deploy();
        }

        /// <summary>
        /// Deploy the service.
        /// </summary>
        /// <param name="service">Service name.</param>
        protected override void ExecuteForService(string service)
        {
            Context.Console.WriteLine($"Deploying service '{service}'");
            new Lifecycle(Context, service).Deploy();
        }
    }
}
