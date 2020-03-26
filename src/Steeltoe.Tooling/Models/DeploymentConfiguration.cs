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

using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// A model of a Docker deployment.
    /// </summary>
<<<<<<< HEAD:src/Steeltoe.Tooling/Models/DeploymentConfiguration.cs
    public class DeploymentConfiguration
=======
    public class Deployment
>>>>>>> Revamp of CLI for .0.7.0-rc1 release:src/Steeltoe.Tooling/Models/Deployment.cs
    {
        /// <summary>
        /// Deployment name.
        /// </summary>
<<<<<<< HEAD:src/Steeltoe.Tooling/Models/DeploymentConfiguration.cs
        [YamlMember(Alias = "configuration")]
=======
        [YamlMember(Alias = "name")]
>>>>>>> Revamp of CLI for .0.7.0-rc1 release:src/Steeltoe.Tooling/Models/Deployment.cs
        public string Name { get; set; }

        /// <summary>
        /// Project to be deployed.
        /// </summary>
        [YamlMember(Alias = "project")]
        public Project Project { get; set; }
    }
}
