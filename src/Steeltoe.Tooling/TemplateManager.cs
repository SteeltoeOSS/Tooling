using System;
using System.IO;
using System.Reflection;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Compiler;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Acts as a facade for template implementations.
    /// </summary>
    public class TemplateManager
    {
        private static readonly object Locker = new object();

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
                lock (Locker) // StringTemplate doesn't seem to be tread safe
                {
                    return new AntlrStringTemplate(template);
                }
            }
            catch (Exception e)
            {
                throw new ToolingException($"failed to load template '{name}': {e.Message}");
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
