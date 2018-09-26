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

namespace Steeltoe.Tooling
{
    public class ServiceLifecycle
    {
        public enum State
        {
            Disabled,
            Offline,
            Starting,
            Online,
            Stopping,
            Unknown,
        }

        public Context Context { get; }

        public string Name { get; }

        public ServiceLifecycle(Context context, string name)
        {
            Context = context;
            Name = name;
        }

        public void Enable()
        {
            GetStateManager().Enable();
        }

        public void Disable()
        {
            GetStateManager().Disable();
        }

        public void Deploy()
        {
            GetStateManager().Deploy();
        }

        public void Undeploy()
        {
            GetStateManager().Undeploy();
        }

        private StateManager GetStateManager()
        {
            var state = Context.ServiceManager.GetServiceState(Name);
            switch (state)
            {
                case State.Disabled:
                    return new ServiceDisabled(Context, Name, state);
                case State.Offline:
                    return new ServiceOffline(Context, Name, state);
                case State.Starting:
                    return new ServiceStarting(Context, Name, state);
                case State.Online:
                    return new ServiceOnline(Context, Name, state);
                case State.Stopping:
                    return new ServiceStopping(Context, Name, state);
            }

            throw new ToolingException($"Unhandled service state '{state.ToString().ToLower()}'");
        }

        private class StateManager
        {
            internal Context Context { get; }

            internal string Name { get; }

            internal State State { get; }

            internal StateManager(Context context, string name, State state)
            {
                Context = context;
                Name = name;
                State = state;
            }

            internal virtual void Enable()
            {
                throw new ServiceLifecycleException(State);
            }

            internal virtual void Disable()
            {
                throw new ServiceLifecycleException(State);
            }

            internal virtual void Deploy()
            {
                throw new ServiceLifecycleException(State);
            }

            internal virtual void Undeploy()
            {
                throw new ServiceLifecycleException(State);
            }
        }

        private class ServiceDisabled : StateManager
        {
            internal ServiceDisabled(Context context, string name, State state) : base(context, name, state)
            {
            }

            internal override void Enable()
            {
                Context.ProjectConfiguration.Services[Name].Enabled = true;
                Context.ProjectConfiguration.NotifyListeners();
            }

            internal override void Disable()
            {
            }
        }

        private class ServiceOffline : StateManager
        {
            internal ServiceOffline(Context context, string name, State state) : base(context, name, state)
            {
            }

            internal override void Enable()
            {
            }

            internal override void Disable()
            {
                Context.ProjectConfiguration.Services[Name].Enabled = false;
                Context.ProjectConfiguration.NotifyListeners();
            }

            internal override void Deploy()
            {
                Context.ServiceManager.GetServiceBackend()
                    .DeployService(Name, Context.ProjectConfiguration.Services[Name].ServiceTypeName);
            }

            internal override void Undeploy()
            {
            }
        }

        private class ServiceOnline : StateManager
        {
            internal ServiceOnline(Context context, string name, State state) : base(context, name, state)
            {
            }

            internal override void Deploy()
            {
            }

            internal override void Undeploy()
            {
                Context.ServiceManager.GetServiceBackend().UndeployService(Name);
            }
        }

        private class ServiceStarting : StateManager
        {
            internal ServiceStarting(Context context, string name, State state) : base(context, name, state)
            {
            }

            internal override void Undeploy()
            {
                Context.ServiceManager.GetServiceBackend().UndeployService(Name);
            }
        }

        private class ServiceStopping : StateManager
        {
            internal ServiceStopping(Context context, string name, State state) : base(context, name, state)
            {
            }
        }
    }
}
