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

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// Helper for building a Deployment.
    /// </summary>
    public class DeploymentBuilder
    {
        /// <summary>
        /// Returns a deployment for the specified directory.
        /// </summary>
        /// <returns>deployment model</returns>
        public Deployment BuildDeployment(string directory)
        {
            var name = Path.GetFileName(directory);
            var deployment = new Deployment
            {
                Name = name,
                Project = new ProjectBuilder().BuildProject(Path.Join(directory, $"{name}.csproj"))
            };
            return deployment;
        }
    }
}
