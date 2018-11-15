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
    public abstract class AppOrServiceExecutor : Executor
    {
        protected string AppOrServiceName { get; private set; }

        protected AppOrServiceExecutor(string appOrServiceName)
        {
            AppOrServiceName = appOrServiceName;
        }

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

        protected abstract void ExecuteForApp();

        protected abstract void ExecuteForService();
    }
}
