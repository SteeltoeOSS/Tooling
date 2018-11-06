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
    public class SetArgsExecutor : ServiceExecutor
    {
        private readonly string _target;

        private readonly string _arguments;

        private readonly bool _force;

        public SetArgsExecutor(string service, string target, string arguments, bool force = false) :
            base(service)
        {
            _target = target;
            _arguments = arguments;
            _force = force;
        }

        protected override void Execute()
        {
            base.Execute();
            var args = Context.Configuration.GetServiceDeploymentArgs(Service, _target);
            if (args != null && !_force)
            {
                throw new ToolingException(
                    $"Service '{Service}' args for target '{_target}' already set to '{args}'");
            }

            Context.Configuration.SetServiceDeploymentArgs(Service, _target, _arguments);
            Context.Console.WriteLine($"Service '{Service}' args for target '{_target}' set to '{_arguments}'");
        }
    }
}
