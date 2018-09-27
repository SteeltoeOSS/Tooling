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
    public class SetTargetExecutor : IExecutor
    {
        private readonly string _environmentName;

        private readonly bool _force;

        public SetTargetExecutor(string environmentName, bool force = false)
        {
            _environmentName = environmentName;
            _force = force;
        }

        public void Execute(Context context)
        {
            Environment env = Registry.GetEnvironment(_environmentName);

            if (!env.IsHealthy(context.Shell))
            {
                if (!_force)
                {
                    context.Shell.Console.WriteLine("Fix errors above or re-run with '-F|--force'");
                    throw new ToolingException($"Environment '{_environmentName	}' does not appear healthy");
                }

                context.Shell.Console.WriteLine("Ignoring poor health report above :-(");
            }

            context.ToolingConfiguration.EnvironmentName = _environmentName;
            context.ToolingConfiguration.NotifyChanged();
            context.Shell.Console.WriteLine($"Target deployment environment set to '{_environmentName}'.");
        }
    }
}
