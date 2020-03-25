using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling.Controllers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Tooling registry.
    /// </summary>
    public class Registry
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Registry>();

        public Dictionary<string, string> Images { get; private set; }

        /// <summary>
        /// Load the registry from the specified directory.
        /// </summary>
        /// <param name="directory"></param>
        public void Load(string directory)
        {
            Logger.LogDebug($"loading registry: {directory}");
            var deserializer = new DeserializerBuilder().Build();
            using (var reader = new StreamReader(Path.Join(directory, "images.yml")))
            {
                Images = deserializer.Deserialize<Dictionary<string, string>>(reader);
            }
        }
    }
}
