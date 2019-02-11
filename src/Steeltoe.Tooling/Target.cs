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
    /// <summary>
    /// Represents a deployment target.
    /// </summary>
    public abstract class Target
    {
        /// <summary>
        /// Deployment target configuration.
        /// </summary>
        public TargetConfiguration Configuration { get; }

        /// <summary>
        /// Deployment target name.
        /// </summary>
        public string Name => Configuration.Name;

        /// <summary>
        /// Deployment target description.
        /// </summary>
        public string Description => Configuration.Description;

        /// <summary>
        /// Creates a new Target.
        /// </summary>
        /// <param name="configuration">Deployment target configuration.</param>
        protected Target(TargetConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Return the deployment target driver.
        /// </summary>
        /// <param name="context">Steeltoe Tooling project context.</param>
        /// <returns></returns>
        public abstract IDriver GetDriver(Context context);

        /// <summary>
        /// Tests if the deployment target is healthy.  E.g., is the target available.
        /// </summary>
        /// <param name="context">Steeltoe Tooling project context.</param>
        /// <returns></returns>
        public abstract bool IsHealthy(Context context);

        /// <summary>
        /// Return a human-readable representation of this Target.
        /// </summary>
        /// <returns>A human readable representation.</returns>
        public override string ToString()
        {
            return $"Target[name={Name},desc=\"{Description}\"]";
        }

        /// <summary>
        /// Return the named property value.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Property value.</returns>
        public string GetProperty(string name)
        {
            return Configuration.Properties[name];
        }
    }
}
