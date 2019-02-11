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
    /// A workflow to get the arguments of an application or service.
    /// </summary>
    [RequiresInitialization]
    public class GetArgsExecutor : AppOrServiceExecutor
    {
        private readonly string _target;

        /// <summary>
        /// A workflow to display the arguments for an application or service.
        /// If <code>target</code> is null, display the application or service arguments.
        /// If <code>target</code> is not null, display the arguments for deploying the application or service.
        /// </summary>
        /// <param name="appOrServiceName">Application or service name.</param>
        /// <param name="target">Deployment target name (can be null).</param>
        public GetArgsExecutor(string appOrServiceName, string target = null) : base(appOrServiceName)
        {
            _target = target;
        }

        internal override void ExecuteForApp()
        {
            ShowArgs(_target == null
                ? Context.Configuration.GetAppArgs(AppOrServiceName)
                : Context.Configuration.GetAppArgs(AppOrServiceName, _target));
        }

        internal override void ExecuteForService()
        {
            ShowArgs(_target == null
                ? Context.Configuration.GetServiceArgs(AppOrServiceName)
                : Context.Configuration.GetServiceArgs(AppOrServiceName, _target));
        }

        private void ShowArgs(string args)
        {
            if (args != null)
            {
                Context.Console.WriteLine(args);
            }
        }
    }
}
