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

using System.IO;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents the context in which Steeltoe Tooling operations are performed.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Steeltoe Tooling project directory.
        /// </summary>
        public string ProjectDirectory { get; }

        /// <summary>
        /// Steeltoe Tooling configuration.
        /// </summary>
        public Configuration Configuration { get; }

        /// <summary>
        /// Steeltoe Tooling console.  Typically the console is used to display messages to the user.
        /// </summary>
        public TextWriter Console { get; }

        /// <summary>
        /// Shell which which to run system commands.
        /// </summary>
        public Shell Shell { get; }

        /// <summary>
        /// Steeltoe Tooling deployment target.
        /// </summary>
        /// <exception cref="ToolingException">Throw if the target has not been set.</exception>
        public Target Target
        {
            get
            {
                if (Configuration?.Target == null)
                {
                    throw new ToolingException("Target not set");
                }

                return Registry.GetTarget(Configuration.Target);
            }
        }

        /// <summary>
        /// Steeltoe Tooling driver.  The driver is used to deploy applications and their services.
        /// </summary>
        public IDriver Driver => Target.GetDriver(this);

        /// <summary>
        /// Creates a new Steeltoe Tooling Content.
        /// </summary>
        /// <param name="dir">Project directory.</param>
        /// <param name="config">Project configuration.{</param>
        /// <param name="console">User console.</param>
        /// <param name="shell">Command shell.</param>
        public Context(string dir, Configuration config, TextWriter console, Shell shell)
        {
            ProjectDirectory = dir;
            Configuration = config;
            Console = console;
            Shell = shell;
        }
    }
}
