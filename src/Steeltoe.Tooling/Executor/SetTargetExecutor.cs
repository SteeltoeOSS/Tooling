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
    public class SetTargetExecutor : IExecutor
    {
        private readonly string _environment;

        private readonly bool _force;

        public SetTargetExecutor(string environment, bool force = false)
        {
            _environment = environment;
            _force = force;
        }

        public void Execute(Context context)
        {
            var envName = _environment.ToLower();
            Environment env = EnvironmentRegistry.ForName(envName);

            if (!env.IsSane(context.Shell))
            {
                if (!_force)
                {
                    context.Shell.Console.WriteLine("Fix errors above or re-run with '-f|--force'");
                    throw new ToolingException($"Environment '{envName}' not sane");
                }
            }

            context.Configuration.EnvironmentName = envName;
            context.Configuration.NotifyListeners();
            context.Shell.Console.WriteLine($"Target deployment environment set to '{envName}'.");
        }
    }
}
