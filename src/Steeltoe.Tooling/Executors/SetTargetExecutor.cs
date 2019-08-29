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

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// A workflow to set the current deployment target.
    /// </summary>
    [RequiresInitialization]
    public class SetTargetExecutor : Executor
    {
        private readonly string _target;

        private readonly bool _force;

        /// <summary>
        /// Create a workflow to set the current deployment target.
        /// </summary>
        /// <param name="target">Deployment target name.</param>
        /// <param name="force">Overwrite an existing deployment target.</param>
        public SetTargetExecutor(string target, bool force = false)
        {
            _target = target;
            _force = force;
        }

        /// <summary>
        /// Set the current deployment target.
        /// </summary>
        /// <exception cref="ToolingException">If an error occurs setting the deployment target.</exception>
        protected override void Execute()
        {
            Target tgt = Registry.GetTarget(_target);

            if (!tgt.IsHealthy(Context))
            {
                if (!_force)
                {
                    Context.Console.WriteLine("Fix errors above or re-run with '-F|--force'");
                    throw new ToolingException($"Target '{_target}' does not appear healthy");
                }

                Context.Console.WriteLine("Ignoring poor health report above :-(");
            }

            Context.Configuration.Target = _target;
            Context.Console.WriteLine($"Target set to '{_target}'");
        }
    }
}
