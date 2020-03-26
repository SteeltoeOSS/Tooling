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
using System.IO;
using System.Reflection;
using Antlr4.StringTemplate;

<<<<<<< HEAD
namespace Steeltoe.Tooling.Templates
=======
namespace Steeltoe.Tooling.Templaters
>>>>>>> Revamp of CLI for .0.7.0-rc1 release
{
    /// <summary>
    /// Acts as a facade for template implementations.
    /// </summary>
    public class TemplateManager
    {
        private static readonly object TemplaterLock = new object();

        /// <summary>
        /// Gets the named template.
        /// </summary>
        /// <param name="name">Template name.</param>
        /// <returns></returns>
        public static ITemplate GetTemplate(string name)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "steeltoe.rc", "templates", name);
            var template = File.ReadAllText(path);
            try
            {
                lock (TemplaterLock) // AntlrStringTemplate doesn't seem to be thread safe
                {
                    return new AntlrStringTemplate(template);
                }
            }
            catch (Exception e)
            {
                throw new ToolingException($"failed to load template: {name} [{e.Message}]");
            }
        }

        class AntlrStringTemplate : ITemplate
        {
            private Template _template;

            internal AntlrStringTemplate(string template)
            {
                _template = new Template(template);
            }

            public void Bind(string name, object obj)
            {
                _template.Add(name, obj);
            }

            public string Render()
            {
                return _template.Render();
            }
        }
    }
}
