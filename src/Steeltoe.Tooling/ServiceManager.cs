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
        protected internal readonly Context _context;

        public ServiceManager(Context context)
        {
            _context = context;
        }

        public IServiceBackend GetServiceBackend()
        {
            return EnvironmentRegistry.ForName(_context.Configuration.EnvironmentName).GetServiceBackend(_context);
        }

        public Service AddService(string name, string type)
        {
            if (!ServiceTypeRegistry.Names.Contains(type))
            {
                throw new ToolingException($"Unknown service type '{type}'");
            }

            if (_context.Configuration.Services.ContainsKey(name))
            {
                throw new ToolingException($"Service '{name}' already exists");
            }

            _context.Configuration.Services[name] = new Configuration.Service {ServiceTypeName = type};
            _context.Configuration.NotifyListeners();
            return GetService(name);
        }

        public void RemoveService(string name)
        {
            if (!_context.Configuration.Services.Remove(name))
            {
                throw new ServiceNotFoundException(name);
            }

            _context.Configuration.NotifyListeners();
        }

        public Service GetService(string name)
        {
            try
            {
                var svccfg = _context.Configuration.Services[name];
                return new Service(name, svccfg.ServiceTypeName);
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(name);
            }
        }

        public List<string> GetServiceNames()
        {
            return _context.Configuration.Services.Keys.ToList();
        }

        public bool HasService(string name)
        {
            return _context.Configuration.Services.ContainsKey(name);
        }

        public string GetServiceStatus(string name)
        {
            return new ServiceLifecycle(_context, name).GetState().ToString().ToLower();
        }

        public void EnableService(string name)
        {
            new ServiceLifecycle(_context, name).Enable();
        }

        public void DisableService(string name)
        {
            new ServiceLifecycle(_context, name).Disable();
        }

        public void DeployService(string name)
        {
            new ServiceLifecycle(_context, name).Deploy();
        }

        public void UndeployService(string name)
        {
            new ServiceLifecycle(_context, name).Undeploy();
        }
    }
}
