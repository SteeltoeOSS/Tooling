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
    public class Lifecycle
    {
        public enum Status
        {
            Offline,
            Starting,
            Online,
            Stopping,
            Unknown,
        }

        private readonly string _name;

        private readonly BackendBridge _bridge;

        public Lifecycle(Context context, string name)
        {
            var backend = context.Backend;
            _name = name;
            if (context.Configuration.GetApps().Contains(name))
            {
                _bridge = new AppBackendBridge(backend);
            }
            else
            {
                _bridge = new ServiceBackendBridge(backend);
            }

        }

        public Status GetStatus()
        {
            return _bridge.GetStatus(_name);
        }

        public void Undeploy()
        {
            GetState().Undeploy(_name, _bridge);
        }

        public void Deploy()
        {
            GetState().Deploy(_name, _bridge);
        }

        private State GetState()
        {
            var status = GetStatus();
            switch (status)
            {
                case Status.Offline:
                    return new OfflineState();
                case Status.Starting:
                    return new StartingState();
                case Status.Online:
                    return new OnlineState();
                case Status.Stopping:
                    return new StoppingState();
                default:
                    throw new ToolingException($"Unhandled lifecycle status: {status}");
            }
        }

        abstract class State
        {
            internal virtual void Deploy(string name, BackendBridge bridge)
            {
            }

            internal virtual void Undeploy(string name, BackendBridge bridge)
            {
            }
        }

        class OfflineState : State
        {
            internal override void Deploy(string name, BackendBridge bridge)
            {
                bridge.Deploy(name);
            }
        }

        class StartingState : State
        {
        }

        class OnlineState : State
        {
            internal override void Undeploy(string name, BackendBridge bridge)
            {
                bridge.Undeploy(name);
            }
        }

        class StoppingState : State
        {
        }

        abstract class BackendBridge
        {
            protected readonly IBackend Backend;

            internal BackendBridge(IBackend backend)
            {
                Backend = backend;
            }

            internal abstract Status GetStatus(string name);

            internal abstract void Deploy(string name);

            internal abstract void Undeploy(string name);
        }

        class AppBackendBridge : BackendBridge
        {
            internal AppBackendBridge(IBackend backend) : base(backend)
            {
            }

            internal override Status GetStatus(string name)
            {
                return Backend.GetAppStatus(name);
            }

            internal override void Deploy(string name)
            {
                Backend.DeployApp(name);
            }

            internal override void Undeploy(string name)
            {
                Backend.UndeployApp(name);
            }

        }

        class ServiceBackendBridge : BackendBridge
        {
            internal ServiceBackendBridge(IBackend backend) : base(backend)
            {
            }

            internal override Status GetStatus(string name)
            {
                return Backend.GetServiceStatus(name);
            }

            internal override void Deploy(string name)
            {
                Backend.DeployService(name);
            }

            internal override void Undeploy(string name)
            {
                Backend.UndeployService(name);
            }
        }
    }
}
