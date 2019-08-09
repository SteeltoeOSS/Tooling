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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Kubernetes
{
    internal class KubernetesDriver : IDriver
    {
        private readonly Context _context;

        private readonly Cli _kubectlCli;

        private readonly Cli _dockerCli;

        private const string ServiceKind = "Service";

        private const string HardCodedServiceApiVersion = "v1";

        private const string DeploymentKind = "Deployment";

        private const string NodePort = "NodePort";

        private const string ImagePullNever = "Never";

        private const string HardCodedDeploymentApiVersion = "apps/v1";

        internal KubernetesDriver(Context context)
        {
            _context = context;
            _kubectlCli = new KubernetesCli(context.Shell);
            _dockerCli = new DockerCli(context.Shell);
        }

        public void DeployApp(string app)
        {
            KubernetesDotnetAppDockerfileFile dockerfileFile = new KubernetesDotnetAppDockerfileFile("Dockerfile");
            var dockerfile = dockerfileFile.KubernetesDotnetAppDockerfile;
            dockerfile.App = app;
            dockerfile.BaseImage = "steeltoeoss/dotnet-runtime:2.1";
            dockerfile.BuildPath = "bin/Debug/netcoreapp2.1/publish";
            dockerfileFile.Store();
            _dockerCli.Run($"build --tag {app.ToLower()} .", "building Docker image for app");

            void PreSaveAction(KubernetesDeploymentConfig deployCfg, KubernetesServiceConfig svcCfg)
            {
                deployCfg.Spec.Template.Spec.Containers[0].ImagePullPolicy = ImagePullNever;
                deployCfg.Spec.Template.Spec.Containers[0].Env =
                    new List<KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.Container.
                        NameValuePair>
                    {
                        new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.Container.
                            NameValuePair() {Name = "GET_HOSTS_FROM", Value = "dns"}
                    };
                svcCfg.Spec.Type = NodePort;
            }

            // TODO: get 'port' from somewhere else
            // TODO: get 'image' from somewhere else
            DeployKubernetesService(app, app.ToLower(), 80, PreSaveAction);
        }

        public void UndeployApp(string app)
        {
            UndeployKubernetesService(app);
        }

        public Lifecycle.Status GetAppStatus(string app)
        {
            return GetKubernetesServiceStatus(app);
        }

        public void DeployService(string service)
        {
            // TODO: get 'os' from from docker command
            DeployService(service, "linux");
        }

        public void UndeployService(string service)
        {
            UndeployKubernetesService(service);
        }

        public Lifecycle.Status GetServiceStatus(string service)
        {
            return GetKubernetesServiceStatus(service);
        }

        internal void DeployService(string service, string os)
        {
            var svcInfo = _context.Configuration.GetServiceInfo(service);
            var port = Registry.GetServiceTypeInfo(svcInfo.ServiceType).Port;
            var image = LookupImage(svcInfo.ServiceType, os);
            DeployKubernetesService(service, image, port);
        }

        private void DeployKubernetesService(String name, String image, int port,
            Action<KubernetesDeploymentConfig, KubernetesServiceConfig> preSaveAction = null)
        {
            var deployCfgFile = new KubernetesDeploymentConfigFile($"{name}-deployment.yml");
            var deployCfg = deployCfgFile.KubernetesDeploymentConfig;
            deployCfg.ApiVersion = HardCodedDeploymentApiVersion;
            deployCfg.Kind = DeploymentKind;
            deployCfg.MetaData = new KubernetesConfig.ConfigMetaData()
            {
                Name = name.ToLower(),
                Labels = new Dictionary<string, string>()
                {
                    {"app", name.ToLower()}
                }
            };
            deployCfg.Spec = new KubernetesDeploymentConfig.DeploymentSpec()
            {
                Selector = new KubernetesDeploymentConfig.DeploymentSpec.ServiceSelector()
                {
                    MatchLabels = new Dictionary<string, string>()
                    {
                        {"app", name.ToLower()}
                    }
                },
                Template = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate()
                {
                    Metadata = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.ServiceMetaData()
                    {
                        Labels = new Dictionary<string, string>()
                        {
                            {"app", name.ToLower()}
                        }
                    },
                    Spec = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec()
                    {
                        Containers =
                            new List<KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.Container>()
                            {
                                new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.Container()
                                {
                                    Name = name.ToLower(),
                                    Image = image,
                                    Ports = new List<KubernetesConfig.ServicePorts>()
                                    {
                                        new KubernetesConfig.ServicePorts()
                                        {
                                            ContainerPort = port
                                        }
                                    }
                                }
                            }
                    }
                }
            };

            var svcCfgFile = new KubernetesServiceConfigFile($"{name}-service.yml");
            var svcCfg = svcCfgFile.KubernetesServiceConfig;
            svcCfg.ApiVersion = HardCodedServiceApiVersion;
            svcCfg.Kind = ServiceKind;
            svcCfg.MetaData = new KubernetesConfig.ConfigMetaData()
            {
                Name = name.ToLower()
            };
            svcCfg.Spec = new KubernetesServiceConfig.ServiceSpec()
            {
                Ports = new List<KubernetesConfig.ServicePorts>()
                {
                    new KubernetesConfig.ServicePorts()
                    {
                        Port = port
                    }
                },
                Selector = new Dictionary<string, string>()
                {
                    {"app", name.ToLower()}
                }
            };

            preSaveAction?.Invoke(deployCfg, svcCfg);
            deployCfgFile.Store();
            svcCfgFile.Store();
            _kubectlCli.Run($"apply --filename {deployCfgFile.File}", "applying Kubernetes deployment configuration");
            _kubectlCli.Run($"apply --filename {svcCfgFile.File}", "applying Kubernetes service configuration");
        }

        private void UndeployKubernetesService(string name)
        {
            try
            {
                _kubectlCli.Run($"delete --filename {name}-service.yml", "deleting Kubernetes service");
            }
            catch (CliException)
            {
                _context.Console.WriteLine($"hmm, Kubernetes service doesn't seem to exist: {name}");
            }

            _kubectlCli.Run($"delete --filename {name}-deployment.yml", "deleting Kubernetes deployment");
        }

        private Lifecycle.Status GetKubernetesServiceStatus(String name)
        {
            var podInfo = _kubectlCli.Run($"get pods --selector app={name.ToLower()}",
                "getting Kubernetes deployment status");
            if (podInfo.Contains("Running"))
            {
//                _kubectlCli.Run($"get services {name.ToLower()}");
                return Lifecycle.Status.Online;
            }
            else if (podInfo.Contains("Pending") || podInfo.Contains("ContainerCreating"))
            {
                return Lifecycle.Status.Starting;
            }
            else if (podInfo.Contains("Terminating"))
            {
                return Lifecycle.Status.Stopping;
            }
            else if (string.IsNullOrEmpty(podInfo))
            {
                try
                {
                    _kubectlCli.Run($"get services {name.ToLower()}", "getting Kubernetes service status");
                    return Lifecycle.Status.Stopping;
                }
                catch (CliException)
                {
                    return Lifecycle.Status.Offline;
                }
            }

            return Lifecycle.Status.Unknown;
        }

        private string LookupImage(string type, string os)
        {
            var images = _context.Target.Configuration.ServiceTypeProperties[type];
            return images.TryGetValue($"image-{os}", out var image) ? image : images["image"];
        }
    }

    internal class KubernetesCli : Cli
    {
        internal KubernetesCli(Shell shell) : base("kubectl", shell)
        {
        }
    }

    internal class DockerCli : Cli
    {
        public DockerCli(Shell shell) : base("docker", shell)
        {
        }
    }
}
