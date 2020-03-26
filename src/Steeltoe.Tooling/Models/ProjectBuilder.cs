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

        private Context _context;
        private readonly string _projectFile;
        private readonly string _launchSettingsFile;
        private XmlDocument _projectDoc;

        /// <summary>
        /// Project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Create a ProjectBuilder for the specified project file.
        /// </summary>
        public ProjectBuilder(Context context, string projectFile)
        {
            _context = context;
            _projectFile = projectFile;
            _launchSettingsFile = Path.Join(Path.GetDirectoryName(projectFile), "Properties", "launchSettings.json");
        }

        /// <summary>
        /// Returns a Project representation of the ProjectFile.
        /// </summary>
        /// <returns>project model</returns>
        public Project BuildProject()
        {
            Logger.LogDebug($"loading project file: {_projectFile}");
            if (!File.Exists(_projectFile))
            {
                throw new ToolingException($"project file not found: {_projectFile}");
            }

            _projectDoc = new XmlDocument();
            _projectDoc.Load(_projectFile);
            var project = new Project
            {
                Name = Path.GetFileNameWithoutExtension(_projectFile),
                File = Path.GetFileName(_projectFile)
            };
            project.Framework = GetFramework();
            if (_context.Registry.DotnetImages.TryGetValue(project.Framework, out var image))
            {
                project.Image = image;
            }
            else
            {
                throw new ToolingException($"no image for framework: {project.Framework}");
            }

            project.Protocols = GetProtocols();
            project.Services = GetServices();
            return project;
        }

        private string GetFramework()
        {
            XmlNodeList nodes = _projectDoc.SelectNodes("/Project/PropertyGroup/TargetFramework");
            if (nodes.Count > 0)
            {
                return nodes[0].InnerText;
            }

            nodes = _projectDoc.SelectNodes("/Project/PropertyGroup/TargetFrameworks");
            if (nodes.Count > 0)
            {
                return nodes[0].InnerText.Split(';')[0];
            }

            throw new ToolingException("could not determine framework");
        }

        private List<Protocol> GetProtocols()
        {
            var protocols = new List<Protocol>();
            if (File.Exists(_launchSettingsFile))
            {
                Logger.LogDebug($"loading launch settings: {_launchSettingsFile}");
                var yaml = new YamlStream();
                using (var reader = new StreamReader(_launchSettingsFile))
                {
                    yaml.Load(reader);
                }

                var root = (YamlMappingNode) yaml.Documents[0].RootNode;
                var profiles = (YamlMappingNode) root.Children[new YamlScalarNode("profiles")];
                foreach (var entry in profiles.Children)
                {
                    var profile = (YamlMappingNode) entry.Value;
                    if (profile.Children.ContainsKey(new YamlScalarNode("applicationUrl")))
                    {
                        var urlSpec = profile.Children[new YamlScalarNode("applicationUrl")];
                        var urls = $"{urlSpec}".Split(';');
                        foreach (var url in urls)
                        {
                            var uri = new Uri(url);
                            protocols.Add(new Protocol(uri.Scheme, uri.Port));
                        }
                    }
                }
            }

            // ensure a default protocol
            if (protocols.Count == 0)
            {
                protocols.Add(new Protocol("http", 8080));
            }

            protocols.Sort();
            return protocols;
        }

        private List<Service> GetServices()
        {
            List<Service> services = new List<Service>();
            foreach (var svcspec in _context.Registry.ServiceSpecifications)
            {
                foreach (var nuget in svcspec.NuGets)
                {
                    var xpath = $"/Project/ItemGroup/PackageReference[@Include='{nuget}']";
                    if (_projectDoc.SelectNodes(xpath).Count == 0)
                    {
                        xpath = $"/Project/ItemGroup/Reference[@Include='{nuget}']";
                        if (_projectDoc.SelectNodes(xpath).Count == 0)
                        {
                            continue;
                        }
                    }

                    var service = new Service()
                    {
                        Name = svcspec.Type,
                        Image = svcspec.Image,
                        Port = svcspec.Port
                    };

                    services.Add(service);
                }
            }

            if (services.Count == 0)
            {
                return null;
            }

            return services;
        }
    }
}
