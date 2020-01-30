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

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    internal class KubernetesDeploymentConfig : KubernetesConfig
    {
        public DeploymentSpec Spec { get; set; }

        public class DeploymentSpec
        {
            public int Replicas { get; set; }

            public ServiceSelector Selector { get; set; }

            public ServiceTemplate Template { get; set; }

            public class ServiceSelector
            {
                public Dictionary<string, string> MatchLabels { get; set; } = new Dictionary<string, string>();
            }

            public class ServiceTemplate
            {
                public ServiceMetaData MetaData { get; set; }

                public TemplateSpec Spec { get; set; }


                public class ServiceMetaData
                {
                    public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
                }

                public class TemplateSpec
                {
                    public List<Container> Containers { get; set; } = new List<Container>();

                    public class Container
                    {
                        public string Name { get; set; }

                        public string Image { get; set; }

                        public string ImagePullPolicy { get; set; }

                        public List<ServicePorts> Ports { get; set; }

                        public List<NameValuePair> Env { get; set; }

                        public class NameValuePair
                        {
                            public string Name { get; set; }

                            public string Value { get; set; }
                        }
                    }
                }
            }
        }
    }
}
