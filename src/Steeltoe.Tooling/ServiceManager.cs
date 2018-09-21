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
            return EnvironmentRegistry.ForName(Context.Configuration.EnvironmentName).GetServiceBackend(Context);
        }

        public Service AddService(string name, string type)
        {
            if (!ServiceTypeRegistry.Names.Contains(type))
            {
                throw new ToolingException($"Unknown service type '{type}'");
            }

            if (Context.Configuration.Services.ContainsKey(name))
            {
                throw new ToolingException($"Service '{name}' already exists");
            }

            Context.Configuration.Services[name] = new Configuration.Service {ServiceTypeName = type};
            Context.Configuration.NotifyListeners();
            return GetService(name);
        }

        public void RemoveService(string name)
        {
            if (!Context.Configuration.Services.Remove(name))
            {
                throw new ServiceNotFoundException(name);
            }

            Context.Configuration.NotifyListeners();
        }

        public Service GetService(string name)
        {
            try
            {
                var svccfg = Context.Configuration.Services[name];
                return new Service(name, svccfg.ServiceTypeName);
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(name);
            }
        }

        public List<string> GetServiceNames()
        {
            return Context.Configuration.Services.Keys.ToList();
        }

        public bool HasService(string name)
        {
            return Context.Configuration.Services.ContainsKey(name);
        }

        public void SetServiceDeploymentArgs(string environmentName, string serviceName, string arguments)
        {
            Context.Configuration.Services[serviceName].Args[environmentName] = arguments;
            Context.Configuration.NotifyListeners();
        }

        public string GetServiceDeploymentArgs(string environmentName, string serviceName)
        {
            try
            {
                return Context.Configuration.Services[serviceName].Args[environmentName];
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }

        public ServiceLifecycle.State GetServiceState(string name)
        {
            if (!Context.Configuration.Services[name].Enabled)
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
