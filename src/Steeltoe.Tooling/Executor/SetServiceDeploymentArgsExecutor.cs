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

        private string _arguments;

        public SetServiceDeploymentArgsExecutor(string environmentName, string serviceName, string arguments) :
            base(serviceName)
        {
            _environmentName = environmentName;
            _arguments = arguments;
        }

        public override void Execute(Context context)
        {
            var env = EnvironmentRegistry.ForName(_environmentName);
            base.Execute(context);
            context.ServiceManager.SetServiceDeploymentArgs(_environmentName, ServiceName, _arguments);
            context.Shell.Console.WriteLine(
                $"Set the '{env.Name}' deployment environment argument(s) for service '{ServiceName}' to '{_arguments}'");
        }
    }
}
