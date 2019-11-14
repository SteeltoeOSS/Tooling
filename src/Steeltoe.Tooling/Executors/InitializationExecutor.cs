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
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.Scanners;

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// A workflow to initialize the Steeltoe Tooling configuration.
    /// </summary>
    public class InitializationExecutor : Executor
    {
        private readonly string _path;

        private readonly bool _autodetect;

        private readonly bool _force;

        /// <summary>
        /// Create a workflow to initialize the Steeltoe Tooling configuration file.
        /// If the <paramref name="path"/> is null, the default path is used.
        /// If the <paramref name="path"/> is a directory, the path is the directory joined with the default file name.
        /// </summary>
        /// <param name="path">Path to the Steeltoe Configuration file.</param>
        /// <param name="autodetect">Detect apps when initializing.</param>
        /// <param name="force">Forces the overwriting of an existing configuration file.</param>
        /// <seealso cref="ConfigurationFile"/>.
        public InitializationExecutor(string path = null, bool autodetect = false, bool force = false)
        {
            _path = path;
            _autodetect = autodetect;
            _force = force;
        }

        /// <summary>
        /// Initialize the Steeltoe Configuration file.
        /// </summary>
        /// <exception cref="ToolingException">If an error occurs when initialization the Steeltoe Configuration file.</exception>
        protected override void Execute()
        {
            var path = _path == null
                ? Context.ProjectDirectory
                : Path.Combine(Context.ProjectDirectory, _path);

            var cfgFile = new ConfigurationFile(path);
            if (cfgFile.Exists())
            {
                if (!_force)
                {
                    throw new ToolingException("Steeltoe Developer Tools already initialized");
                }
                File.Delete(cfgFile.File);
                cfgFile = new ConfigurationFile(path);
            }

            Context.Configuration = cfgFile.Configuration;

            if (_autodetect)
            {
                foreach (var appInfo in new AppScanner().Scan(Context.ProjectDirectory))
                {
                    new AddExecutor(appInfo.App).Execute(Context);
                }
            }

            cfgFile.Store();
            Context.Console.WriteLine("Initialized Steeltoe Developer Tools");
        }
    }
}
