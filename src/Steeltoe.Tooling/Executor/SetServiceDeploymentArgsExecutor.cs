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
    public class SetServiceDeploymentArgsExecutor : ServiceExecutor
    {
        private readonly string _environmentName;

        private readonly string _arguments;

        private readonly bool _force;

        public SetServiceDeploymentArgsExecutor(string environmentName, string serviceName, string arguments,
            bool force = false) :
            base(serviceName)
        {
            _environmentName = environmentName;
            _arguments = arguments;
            _force = force;
        }

        public override void Execute(Context context)
        {
            base.Execute(context);
            var args = context.ServiceManager.GetServiceDeploymentArgs(_environmentName, ServiceName);
            if (args != null && !_force)
            {
                throw new ToolingException(
                    $"'{_environmentName}' deployment environment arguments for service '{ServiceName}' already set.");
            }

            context.ServiceManager.SetServiceDeploymentArgs(_environmentName, ServiceName, _arguments);
            context.Console.WriteLine(
                $"Set the '{_environmentName}' deployment environment arguments for service '{ServiceName}' to '{_arguments}'");
        }
    }
}
