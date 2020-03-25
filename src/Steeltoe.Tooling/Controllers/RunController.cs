// Copyright 2020 the original author or authors.
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
using Steeltoe.Tooling.Models;
using Steeltoe.Tooling.Templaters;

namespace Steeltoe.Tooling.Controllers
{
    /// <summary>
    /// Controls the "run" operation.
    /// </summary>
    public class RunController : Controller
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<RunController>();

        /// <summary>
        /// Runs the project in the local Docker environment.
        /// </summary>
        protected override void Execute()
        {
            var project = GetProject();
            var images = new Images(project);
            string[] files = new string[] {"Dockerfile", "docker-compose.yml"};
            foreach (var file in files)
            {
                Logger.LogDebug($"writing {file}");
                var template = TemplateManager.GetTemplate($"{file}.st");
                template.Bind("project", project);
                template.Bind("images", images);
                File.WriteAllText(file, template.Render());
            }

            var cli = new Cli("docker-compose", Context.Shell);
            cli.Run("up --build", $"running '{project.Name}' in Docker");
        }

        public class Images
        {
            public string DotnetSdk { get; }

            public Images(Project project)
            {
                if (project.Framework.Equals("netcoreapp3.1"))
                {
                    DotnetSdk = "mcr.microsoft.com/dotnet/core/sdk:3.1";
                }
                else
                {
                    throw new ToolingException($"no Docker image available for framework: {project.Framework}");
                }
            }
        }
    }
}
