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
    /// A workflow to undeploy applications and dependent services from the current target.
    /// </summary>
    [RequiresTarget]
    public class UndeployExecutor : GroupExecutor
    {
        /// <summary>
        /// Creates a workflow undeploy applications and dependent services from the current target.
        /// </summary>
        public UndeployExecutor() : base(true)
        {
        }

        /// <summary>
        /// Undeploys apps then blocks until all apps offline.
        /// </summary>
        /// <param name="apps">Apps to undeploy.</param>
        protected override void ExecuteForApps(List<string> apps)
        {
            base.ExecuteForApps(apps);
            foreach (var app in apps)
            {
                var count = 0;
                while (true)
                {
                    ++count;
                    if (Settings.MaxChecks >= 0 && ++count > Settings.MaxChecks)
                    {
                        throw new ToolingException($"max check exceeded ({Settings.MaxChecks})");
                    }

                    var startTicks = DateTime.Now.Ticks;
                    if (Context.Driver.GetAppStatus(app) == Lifecycle.Status.Offline)
                    {
                        break;
                    }

                    const int ticksPerMillis = 10000;
                    var elapsedMillis = (DateTime.Now.Ticks - startTicks) / ticksPerMillis;
                    const int oneSecondMillis = 1000;
                    var waitMillis = oneSecondMillis - elapsedMillis;
                    if (waitMillis > 0L)
                    {
                        Thread.Sleep((int) waitMillis);
                    }

                    Context.Console.WriteLine($"Waiting for app '{app}' to go offline ({count})");
                }
            }
        }

        /// <summary>
        /// Undeploys services then blocks until all apps offline.
        /// </summary>
        /// <param name="services">Services to undeploy.</param>
        protected override void ExecuteForServices(List<string> services)
        {
            base.ExecuteForServices(services);
            foreach (var service in services)
            {
                var count = 0;
                while (true)
                {
                    ++count;
                    if (Settings.MaxChecks >= 0 && ++count > Settings.MaxChecks)
                    {
                        throw new ToolingException($"max check exceeded ({Settings.MaxChecks})");
                    }

                    var startTicks = DateTime.Now.Ticks;
                    if (Context.Driver.GetServiceStatus(service) == Lifecycle.Status.Offline)
                    {
                        break;
                    }

                    const int ticksPerMillis = 10000;
                    var elapsedMillis = (DateTime.Now.Ticks - startTicks) / ticksPerMillis;
                    const int oneSecondMillis = 1000;
                    var waitMillis = oneSecondMillis - elapsedMillis;
                    if (waitMillis > 0L)
                    {
                        Thread.Sleep((int) waitMillis);
                    }

                    Context.Console.WriteLine($"Waiting for service '{service}' to go offline ({count})");
                }
            }
        }

        /// <summary>
        /// Undeploys an application to the current target.
        /// </summary>
        /// <param name="app">Application name.</param>
        protected override void ExecuteForApp(string app)
        {
            Context.Console.WriteLine($"Undeploying app '{app}'");
            new Lifecycle(Context, app).Undeploy();
        }

        /// <summary>
        /// Undeploys a service to the current target.
        /// </summary>
        /// <param name="service">Service name.</param>
        protected override void ExecuteForService(string service)
        {
            Context.Console.WriteLine($"Undeploying service '{service}'");
            new Lifecycle(Context, service).Undeploy();
        }
    }
}
