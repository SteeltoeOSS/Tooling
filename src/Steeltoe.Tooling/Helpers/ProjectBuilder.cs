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
using System.Xml;
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
            if (!File.Exists(projectFile))
            {
                throw new ToolingException($"project file not found: {projectFile}");
            }

            var project = new Project
            {
                Name = Path.GetFileNameWithoutExtension(projectFile),
                File = Path.GetFileName(projectFile),
                Framework = GetFramework(projectFile),
                Services = GetServices(projectFile)
            };
            return project;
        }

        private string GetFramework(string projectFile)
        {
            XmlDocument projectDoc = new XmlDocument();
            projectDoc.Load(projectFile);
            XmlNodeList nodes = projectDoc.SelectNodes("/Project/PropertyGroup/TargetFramework");
            if (nodes.Count == 0)
            {
                throw new ToolingException("could not determine framework");
            }

            return nodes[0].InnerText;
        }

        private List<Service> GetServices(string projectFile)
        {
            var launchSettingsPath =
                Path.Join(Path.GetDirectoryName(projectFile), "Properties", "launchSettings.json");
            if (!File.Exists(launchSettingsPath))
            {
                return null;
            }

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

            var services = urls.Select(url => new Uri(url)).Select(uri => new Service(uri.Scheme, uri.Port)).ToList();
            services.Sort();
            return services;
        }
    }
}
