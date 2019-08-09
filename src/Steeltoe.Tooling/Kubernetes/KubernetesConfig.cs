using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Kubernetes
{
    public class KubernetesConfig
    {
        [YamlMember(Alias = "apiVersion", Order = -2)]
        public string ApiVersion { get; set; }

        [YamlMember(Alias = "kind", Order = -2)]
        public string Kind { get; set; }

        [YamlMember(Alias = "metadata", Order = -2)]
        public ConfigMetaData MetaData { get; set; }

        public class ConfigMetaData
        {
            [YamlMember(Alias = "name")] public string Name { get; set; }

            [YamlMember(Alias = "labels")]
            public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
        }

        public class ServicePorts
        {
            [YamlMember(Alias = "port")] public int Port { get; set; }

            [YamlMember(Alias = "targetPort")] public int TargetPort { get; set; }

            [YamlMember(Alias = "containerPort")] public int ContainerPort { get; set; }
        }
    }
}
