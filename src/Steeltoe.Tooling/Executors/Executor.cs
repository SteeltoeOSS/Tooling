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

using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// Represents a Steeltoe Tooling workflow.
    /// </summary>
    public abstract class Executor
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Executor>();

        /// <summary>
        /// The context in which the workflow executes.
        /// </summary>
        protected Context Context { get; private set; }

        /// <summary>
        /// Executes this workflow in the specified context.
        /// </summary>
        /// <param name="context">Context in which to run this workflow.</param>
        /// <exception cref="ToolingException">If an error occurs running the workflow.</exception>
        public void Execute(Context context)
        {
            var type = GetType();
            Logger.LogDebug($"executor is {type}");
            Context = context;
            Execute();
        }

        /// <summary>
        /// Sub-classes implement this to perform their specific workflow.
        /// </summary>
        protected abstract void Execute();
    }
}
