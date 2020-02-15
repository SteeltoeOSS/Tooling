// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents the lifecycle or an application or application service.
    /// </summary>
    public class Lifecycle
    {
        /// <summary>
        /// Represents the states of a lifecycle.
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// An application or application that is fully undeployed.
            /// </summary>
            Offline,

            /// <summary>
            /// The state of an application or application service in the process of being deployed.  A transitional state from Offline to Online.
            /// </summary>
            Starting,

            /// <summary>
            /// An application or application that is fully deployed and ready to be used.
            /// </summary>
            Online,

            /// <summary>
            /// The state of an application or application service in the process of being undeployed.  A transitional state from Online to Offline.
            /// </summary>
            Stopping,

            /// <summary>
            /// Represents a undetermined state.
            /// </summary>
            Unknown,
        }

        private readonly string _name;

        /// <summary>
        /// Creates a new lifecycle for the named application or application service.
        /// </summary>
        /// <param name="context">Steeltoe Tooling context.</param>
        /// <param name="name">Application or application service name.</param>
        public Lifecycle(Context context, string name)
        {
            _name = name;
        }

        /// <summary>
        /// Returns this lifecycle's current status.
        /// </summary>
        /// <returns></returns>
        public Status GetStatus()
        {
            throw new NotImplementedException();
        }

        /*
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

        private abstract class State
        {
            internal virtual void Deploy(string name, DriverBridge bridge)
            {
                throw new ToolingException($"Cannot deploy '{name}' because it is {this}");
            }

            internal virtual void Undeploy(string name, DriverBridge bridge)
            {
                throw new ToolingException($"Cannot undeploy '{name}' because it is {this}");
            }
        }

        private class OfflineState : State
        {
            internal override void Deploy(string name, DriverBridge bridge)
            {
                bridge.Deploy(name);
            }

            internal override void Undeploy(string name, DriverBridge bridge)
            {
            }

            public override string ToString()
            {
                return "offline";
            }
        }

        private class StartingState : State
        {
            public override string ToString()
            {
                return "starting";
            }
        }

        private class OnlineState : State
        {
            internal override void Deploy(string name, DriverBridge bridge)
            {
            }

            internal override void Undeploy(string name, DriverBridge bridge)
            {
                bridge.Undeploy(name);
            }

            public override string ToString()
            {
                return "online";
            }
        }

        private class StoppingState : State
        {
            public override string ToString()
            {
                return "stopping";
            }
        }
        */
    }
}
