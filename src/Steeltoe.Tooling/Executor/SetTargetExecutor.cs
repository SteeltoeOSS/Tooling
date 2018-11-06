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
    [RequiresInitialization]
    public class SetTargetExecutor : Executor
    {
        private readonly string _target;

        private readonly bool _force;

        public SetTargetExecutor(string target, bool force = false)
        {
            _target = target;
            _force = force;
        }

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
            Context.Configuration.NotifyChanged();
            Context.Console.WriteLine($"Target set to '{_target}'");
        }
    }
}
