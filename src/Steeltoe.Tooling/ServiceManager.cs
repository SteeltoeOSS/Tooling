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

using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.Tooling
{
    public class ServiceManager
    {
        protected internal readonly Context Context;

        public ServiceManager(Context context)
        {
            Context = context;
        }

        public IServiceBackend GetServiceBackend()
        {
            return Context.Environment.GetServiceBackend(Context);
        }

        public Service AddService(string name, string type)
        {
            if (!Registry.ServiceTypeNames.Contains(type))
            {
                throw new ToolingException($"Unknown service type '{type}'");
            }

            if (Context.ToolingConfiguration.Services.ContainsKey(name))
            {
                throw new ToolingException($"Service '{name}' already exists");
            }

            Context.ToolingConfiguration.Services[name] = new ToolingConfiguration.Service {ServiceTypeName = type};
            Context.ToolingConfiguration.NotifyChanged();
            return GetService(name);
        }

        public void RemoveService(string name)
        {
            if (!Context.ToolingConfiguration.Services.Remove(name))
            {
                throw new ServiceNotFoundException(name);
            }

            Context.ToolingConfiguration.NotifyChanged();
        }

        public Service GetService(string name)
        {
            try
            {
                var svccfg = Context.ToolingConfiguration.Services[name];
                return new Service(name, svccfg.ServiceTypeName);
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(name);
            }
        }

        public List<string> GetServiceNames()
        {
            return Context.ToolingConfiguration.Services.Keys.ToList();
        }

        public bool HasService(string name)
        {
            return Context.ToolingConfiguration.Services.ContainsKey(name);
        }

        public void SetServiceDeploymentArgs(string environmentName, string serviceName, string arguments)
        {
            Context.ToolingConfiguration.Services[serviceName].Args[environmentName] = arguments;
            Context.ToolingConfiguration.NotifyChanged();
        }

        public string GetServiceDeploymentArgs(string environmentName, string serviceName)
        {
            try
            {
                return Context.ToolingConfiguration.Services[serviceName].Args[environmentName];
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }

        public ServiceLifecycle.State GetServiceState(string name)
        {
            if (!Context.ToolingConfiguration.Services[name].Enabled)
            {
                return ServiceLifecycle.State.Disabled;
            }

            return GetServiceBackend().GetServiceLifecleState(name);
        }

        public void EnableService(string name)
        {
            new ServiceLifecycle(Context, name).Enable();
        }

        public void DisableService(string name)
        {
            new ServiceLifecycle(Context, name).Disable();
        }

        public void DeployService(string name)
        {
            new ServiceLifecycle(Context, name).Deploy();
        }

        public void UndeployService(string name)
        {
            new ServiceLifecycle(Context, name).Undeploy();
        }
    }
}
