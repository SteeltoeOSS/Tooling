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
    /// Defines the interface for Steeltoe Tooling drivers.  Drivers are responsible for application and service
    /// deployment and for reporting the state of applications and services.
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// Called before apps and/or services are deployed.
        /// </summary>
        void DeploySetup();

        /// <summary>
        /// Called affter apps and/or services are undeployed.
        /// </summary>
        void DeployTeardown();

        /// <summary>
        /// Deploys the named application.
        /// </summary>
        /// <param name="app">The application name.</param>
        /// <remarks>Implementations may return before the application is fully deployed.</remarks>
        void DeployApp(string app);

        /// <summary>
        /// Undeploys the named application.
        /// </summary>
        /// <param name="app">The application name.</param>
        /// <remarks>Implementations may return before the application is fully undeployed.</remarks>
        void UndeployApp(string app);

        /// <summary>
        /// Returns the named application's lifecycle status.
        /// </summary>
        /// <param name="app">The application name.</param>
        /// <returns>The named application's lifecycle status.</returns>
        Lifecycle.Status GetAppStatus(string app);

        /// <summary>
        /// Deploys the named service.
        /// </summary>
        /// <param name="service">The service name.</param>
        /// <remarks>Implementations may return before the service is fully deployed.</remarks>
        void DeployService(string service);

        /// <summary>
        /// Undeploys the named service.
        /// </summary>
        /// <param name="service">The service name.</param>
        /// <remarks>Implementations may return before the service is fully undeployed.</remarks>
        void UndeployService(string service);

        /// <summary>
        /// Returns the named service's lifecycle status.
        /// </summary>
        /// <param name="service">The service name.</param>
        /// <returns>The named service's lifecycle status.</returns>
        Lifecycle.Status GetServiceStatus(string service);
    }
}
