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
using System.Net.Http.Headers;
using System.Xml;

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// A workflow to add an application or service to the Steeltoe Tooling configuration.
    /// </summary>
    [RequiresInitialization]
    public class AddAppExecutor : Executor
    {
        private readonly string _name;

        private readonly string _framework;

        private readonly string _runtime;

        /// <summary>
        /// Create a new workflow to add an application to the Steeltoe Tooling configuration.
        /// </summary>
        /// <param name="appName">Application name.</param>
        /// <param name="framework">Target framework.</param>
        /// <param name="runtime">Target runtime.</param>
        public AddAppExecutor(string appName, string framework, string runtime)
        {
            _name = appName;
            _framework = framework;
            _runtime = runtime;
        }

        /// <summary>
        /// Add the application or service to the Steeltoe Tooling configuration.
        /// </summary>
        protected override void Execute()
        {
            var framework = _framework ?? GuessFramework();
            var runtime = _runtime ?? "ubuntu.16.04-x64";
            Context.Configuration.AddApp(_name, framework, runtime);
            Context.Console.WriteLine($"Added app '{_name}' ({framework}/{runtime})");
        }

        private string GuessFramework()
        {
            var projectFile = Path.Join(Context.ProjectDirectory, $"{_name}.csproj");
            if (!File.Exists(projectFile))
            {
                throw new ToolingException($"project file does not exist: {projectFile}");
            }
            var doc = new XmlDocument();
            doc.Load(projectFile);
            var nodes = doc.GetElementsByTagName("TargetFramework");
            var frameworks = nodes[0].InnerText;
            return frameworks.Split(";")[0];
        }
    }
}
