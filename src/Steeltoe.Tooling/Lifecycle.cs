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

        private readonly IBackend _backend;

        private readonly string _name;

        private readonly BackendBridge _bridge;

        public Lifecycle(Context context, string name)
        {
            _backend = context.Backend;
            _name = name;
            _bridge = new ServiceBackendBridge();
        }

        public Status GetStatus()
        {
            return _bridge.GetStatus(_name, _backend);
        }

        public void Undeploy()
        {
            GetState().Undeploy(_name, _backend);
        }

        public void Deploy()
        {
            GetState().Deploy(_name, _backend);
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
            internal virtual void Deploy(string name, IBackend backend)
            {
            }

            internal virtual void Undeploy(string name, IBackend backend)
            {
            }
        }

        class OfflineState : State
        {
            internal override void Deploy(string name, IBackend backend)
            {
                backend.DeployService(name);
            }
        }

        class StartingState : State
        {
        }

        class OnlineState : State
        {
            internal override void Undeploy(string name, IBackend backend)
            {
                backend.UndeployService(name);
            }
        }

        class StoppingState : State
        {
        }

        abstract class BackendBridge
        {
            internal abstract Status GetStatus(string name, IBackend backend);
        }

        class ServiceBackendBridge : BackendBridge
        {
            internal override Status GetStatus(string name, IBackend backend)
            {
                return backend.GetServiceStatus(name);
            }
        }
    }
}
