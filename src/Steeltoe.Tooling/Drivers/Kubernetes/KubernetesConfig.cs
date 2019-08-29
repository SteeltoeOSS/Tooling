using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    /// <summary>
    /// Represents a Kubernetes resource configuration.
    /// </summary>
    public class KubernetesConfig
    {
        /// <summary>
        /// API version.
        /// </summary>
        [YamlMember(Alias = "apiVersion", Order = -2)]
        public string ApiVersion { get; set; }

        /// <summary>
        /// Resource kind, e.g. Service or Deployment.
        /// </summary>
        [YamlMember(Alias = "kind", Order = -2)]
        public string Kind { get; set; }

        /// <summary>
        /// Resource metadata.
        /// </summary>
        [YamlMember(Alias = "metadata", Order = -2)]
        public ConfigMetaData MetaData { get; set; }

        /// <summary>
        /// Resource metadata structure.
        /// </summary>
        public class ConfigMetaData
        {
            /// <summary>
            /// Resource name.
            /// </summary>
            [YamlMember(Alias = "name")] public string Name { get; set; }

            /// <summary>
            /// Resource labels, e.g. "tier" -> "frontend".
            /// </summary>
            [YamlMember(Alias = "labels")]
            public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
        }

        /// <summary>
        /// Resource service ports structure.
        /// </summary>
        public class ServicePorts
        {
            /// <summary>
            /// Service port.
            /// </summary>
            [YamlMember(Alias = "port")] public int Port { get; set; }

            /// <summary>
            /// Service target port.
            /// </summary>
            [YamlMember(Alias = "targetPort")] public int TargetPort { get; set; }

            /// <summary>
            /// Service container port.
            /// </summary>
            [YamlMember(Alias = "containerPort")] public int ContainerPort { get; set; }
        }
    }
}
