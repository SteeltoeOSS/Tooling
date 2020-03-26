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
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// Helper for building a Project.
    /// </summary>
    public class ProjectBuilder
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<ProjectBuilder>();

        private static readonly List<Protocol> DefaultProtocols = new List<Protocol>();

        static ProjectBuilder()
        {
            DefaultProtocols.Add(new Protocol("http", 8080));
        }

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
            Logger.LogDebug($"loading project file: {projectFile}");
            if (!File.Exists(projectFile))
            {
                throw new ToolingException($"project file not found: {projectFile}");
            }

            var project = new Project
            {
                Name = Path.GetFileNameWithoutExtension(projectFile),
                File = Path.GetFileName(projectFile),
                Framework = GetFramework(projectFile),
                Protocols = GetProtocols(projectFile)
            };
            return project;
        }

        private string GetFramework(string projectFile)
        {
            XmlDocument projectDoc = new XmlDocument();
            projectDoc.Load(projectFile);
            XmlNodeList nodes = projectDoc.SelectNodes("/Project/PropertyGroup/TargetFramework");
            if (nodes.Count > 0)
            {
                return nodes[0].InnerText;
            }

            nodes = projectDoc.SelectNodes("/Project/PropertyGroup/TargetFrameworks");
            if (nodes.Count > 0)
            {
                return nodes[0].InnerText.Split(';')[0];
            }

            throw new ToolingException("could not determine framework");
        }

        private List<Protocol> GetProtocols(string projectFile)
        {
            var launchSettingsPath =
                Path.Join(Path.GetDirectoryName(projectFile), "Properties", "launchSettings.json");
            if (!File.Exists(launchSettingsPath))
            {
                return DefaultProtocols;
            }

            Logger.LogDebug($"loading launch settings: {launchSettingsPath}");
            var yaml = new YamlStream();
            using (var reader = new StreamReader(launchSettingsPath))
            {
                yaml.Load(reader);
            }

            var root = (YamlMappingNode) yaml.Documents[0].RootNode;
            var profiles = (YamlMappingNode) root.Children[new YamlScalarNode("profiles")];
            var profile =
                (YamlMappingNode) profiles.Children[new YamlScalarNode(Path.GetFileNameWithoutExtension(projectFile))];
            if (!profile.Children.ContainsKey(new YamlScalarNode("applicationUrl")))
            {
                return DefaultProtocols;
            }

            var urlSpec = profile.Children[new YamlScalarNode("applicationUrl")];
            var urls = $"{urlSpec}".Split(';');
            if (urls.Length == 0)
            {
                return DefaultProtocols;
            }

            var protocols = urls.Select(url => new Uri(url)).Select(uri => new Protocol(uri.Scheme, uri.Port)).ToList();
            protocols.Sort();
            return protocols;
        }
    }
}
