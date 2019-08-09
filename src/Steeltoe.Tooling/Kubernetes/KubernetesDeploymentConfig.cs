// Copyright 2018 the original author or authors.
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

using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Kubernetes
{
    internal class KubernetesDeploymentConfig : KubernetesConfig
    {
        [YamlMember(Alias = "spec")] public DeploymentSpec Spec { get; set; }

        public class DeploymentSpec
        {
            [YamlMember(Alias = "replicas")] public int Replicas { get; set; }

            [YamlMember(Alias = "selector")] public ServiceSelector Selector { get; set; }

            [YamlMember(Alias = "template")] public ServiceTemplate Template { get; set; }

            public class ServiceSelector
            {
                [YamlMember(Alias = "matchLabels")]
                public Dictionary<string, string> MatchLabels { get; set; } = new Dictionary<string, string>();
            }

            public class ServiceTemplate
            {
                [YamlMember(Alias = "metadata")] public ServiceMetaData Metadata { get; set; }

                [YamlMember(Alias = "spec")] public TemplateSpec Spec { get; set; }


                public class ServiceMetaData
                {
                    [YamlMember(Alias = "labels")]
                    public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
                }

                public class TemplateSpec
                {
                    [YamlMember(Alias = "containers")]
                    public List<Container> Containers { get; set; } = new List<Container>();

                    public class Container
                    {
                        [YamlMember(Alias = "name")] public string Name { get; set; }

                        [YamlMember(Alias = "image")] public string Image { get; set; }

                        [YamlMember(Alias = "imagePullPolicy")]
                        public string ImagePullPolicy { get; set; }

                        [YamlMember(Alias = "ports")]
                        public List<ServicePorts> Ports { get; set; }

                        [YamlMember(Alias = "env")]
                        public List<NameValuePair> Env { get; set; }

                        public class NameValuePair
                        {
                            [YamlMember(Alias = "name")] public string Name { get; set; }

                            [YamlMember(Alias = "value")] public string Value { get; set; }
                        }
                    }
                }
            }
        }
    }
}
