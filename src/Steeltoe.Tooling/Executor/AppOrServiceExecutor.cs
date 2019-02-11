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
    /// An abstract class representing a workflow relating to an application or service.
    /// </summary>
    public abstract class AppOrServiceExecutor : Executor
    {
        /// <summary>
        /// The name of the related application or service.
        /// </summary>
        protected string AppOrServiceName { get; private set; }

        /// <summary>
        /// Create a workflow relating to an application or service.
        /// </summary>
        /// <param name="appOrServiceName"></param>
        protected AppOrServiceExecutor(string appOrServiceName)
        {
            AppOrServiceName = appOrServiceName;
        }

        /// <summary>
        /// Execute this workflow.
        /// </summary>
        /// <exception cref="ItemDoesNotExistException">If the application or service relating to this workflow does not exist.</exception>
        protected override void Execute()
        {
            if (Context.Configuration.GetApps().Contains(AppOrServiceName))
            {
                ExecuteForApp();
            }
            else if (Context.Configuration.GetServices().Contains(AppOrServiceName))
            {
                ExecuteForService();
            }
            else
            {
                throw new ItemDoesNotExistException(AppOrServiceName, "app or service");
            }
        }

        internal abstract void ExecuteForApp();

        internal abstract void ExecuteForService();
    }
}
