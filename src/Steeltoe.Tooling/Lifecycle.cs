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

        private readonly DriverBridge _bridge;

        /// <summary>
        /// Creates a new lifecycle for the named application or application service.
        /// </summary>
        /// <param name="context">Steeltoe Tooling context.</param>
        /// <param name="name">Application or application service name.</param>
        public Lifecycle(Context context, string name)
        {
            var driver = context.Driver;
            _name = name;
            if (context.Configuration.GetApps().Contains(name))
            {
                _bridge = new AppDriverBridge(driver);
            }
            else
            {
                _bridge = new ServiceDriverBridge(driver);
            }
        }

        /// <summary>
        /// Returns this lifecycle's current status.
        /// </summary>
        /// <returns></returns>
        public Status GetStatus()
        {
            return _bridge.GetStatus(_name);
        }

        /// <summary>
        /// Undeploy an application or application service in the context of this lifecycle.
        /// </summary>
        public void Undeploy()
        {
            GetState().Undeploy(_name, _bridge);
        }

        /// <summary>
        /// Deploy an application or application service in the context of this lifecycle.
        /// </summary>
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

        abstract class DriverBridge
        {
            protected readonly IDriver Driver;

            protected DriverBridge(IDriver driver)
            {
                Driver = driver;
            }

            internal abstract Status GetStatus(string name);

            internal abstract void Deploy(string name);

            internal abstract void Undeploy(string name);
        }

        private class AppDriverBridge : DriverBridge
        {
            internal AppDriverBridge(IDriver driver) : base(driver)
            {
            }

            internal override Status GetStatus(string name)
            {
                return Driver.GetAppStatus(name);
            }

            internal override void Deploy(string name)
            {
                Driver.DeployApp(name);
            }

            internal override void Undeploy(string name)
            {
                Driver.UndeployApp(name);
            }
        }

        private class ServiceDriverBridge : DriverBridge
        {
            internal ServiceDriverBridge(IDriver driver) : base(driver)
            {
            }

            internal override Status GetStatus(string name)
            {
                return Driver.GetServiceStatus(name);
            }

            internal override void Deploy(string name)
            {
                Driver.DeployService(name);
            }

            internal override void Undeploy(string name)
            {
                Driver.UndeployService(name);
            }
        }
    }
}
