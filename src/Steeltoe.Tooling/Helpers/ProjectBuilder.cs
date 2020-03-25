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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steeltoe.Tooling.Models;
using YamlDotNet.RepresentationModel;

namespace Steeltoe.Tooling.Helpers
{
    /// <summary>
    /// Helper for building a Project.
    /// </summary>
    public class ProjectBuilder
    {
        /// <summary>
        /// Project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Returns a Project representation of the ProjectFile.
        /// </summary>
        /// <returns>project model</returns>
        public Project BuildProject(string projectFile)
        {
            var project = new Project
            {
                Name = Path.GetFileNameWithoutExtension(projectFile),
                File = Path.GetFileName(projectFile),
            };
            project.Ports = DiscoverPorts(projectFile);
            return project;
        }

        private List<Port> DiscoverPorts(string projectFile)
        {
            var launchSettingsPath =
                Path.Join(Path.GetDirectoryName(projectFile), "Properties", "launchSettings.json");
            var yaml = new YamlStream();
            using (var reader = new StreamReader(launchSettingsPath))
            {
                yaml.Load(reader);
            }

            var root = (YamlMappingNode) yaml.Documents[0].RootNode;
            var profiles = (YamlMappingNode) root.Children[new YamlScalarNode("profiles")];
            var profile =
                (YamlMappingNode) profiles.Children[new YamlScalarNode(Path.GetFileNameWithoutExtension(projectFile))];
            var urlSpec = profile.Children[new YamlScalarNode("applicationUrl")];
            var urls = $"{urlSpec}".Split(';');
            if (urls.Length == 0)
            {
                return null;
            }

            var ports = urls.Select(url => new Uri(url)).Select(uri => new Port(uri.Port)).ToList();
            ports.Sort();
            return ports;
        }
    }
}
