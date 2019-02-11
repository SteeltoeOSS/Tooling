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
    /// A workflow to remove an application or service from the Steeltoe Tooling configuration.
    /// </summary>
    [RequiresInitialization]
    public class RemoveExecutor : AppOrServiceExecutor
    {
        /// <summary>
        /// Create a new workflow to remove an application or service from the Steeltoe Tooling configuration.
        /// </summary>
        /// <param name="appOrServiceName">Application or service name.</param>
        public RemoveExecutor(string appOrServiceName) : base(appOrServiceName)
        {
        }

        internal override void ExecuteForApp()
        {
            Context.Configuration.RemoveApp(AppOrServiceName);
            Context.Console.WriteLine($"Removed app '{AppOrServiceName}'");
        }

        internal override void ExecuteForService()
        {
            var svcInfo = Context.Configuration.GetServiceInfo(AppOrServiceName);
            Context.Configuration.RemoveService(AppOrServiceName);
            Context.Console.WriteLine($"Removed {svcInfo.ServiceType} service '{AppOrServiceName}'");
        }
    }
}
