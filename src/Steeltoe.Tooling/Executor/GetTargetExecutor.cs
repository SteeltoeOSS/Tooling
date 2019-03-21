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

namespace Steeltoe.Tooling.Executor
{
    public class GetTargetExecutor : IExecutor
    {
        public void Execute(Context context)
        {
            if (context.Configuration.EnvironmentName == null)
            {
                throw new ToolingException("Target deployment environment not set");
            }

            context.Shell.Console.WriteLine(
                $"Target deployment environment set to '{context.Configuration.EnvironmentName}'.");
        }
    }
}
