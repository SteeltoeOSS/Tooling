using System.Collections.Generic;
using Shouldly;
using System.IO;
using Steeltoe.Tooling.Drivers.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.Kubernetes
{
    public class KubernetesDeploymentConfigFileTest : ToolingTest
    {
        private readonly string _configFile;

        public KubernetesDeploymentConfigFileTest()
        {
            _configFile = Path.Join(Context.ProjectDirectory, "kubernetes-deployment-config-file");
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfgFile = new KubernetesDeploymentConfigFile(_configFile);
            cfgFile.KubernetesDeploymentConfig.ApiVersion = "apps/v1";
            cfgFile.KubernetesDeploymentConfig.Kind = "Deployment";
            cfgFile.KubernetesDeploymentConfig.MetaData = new KubernetesConfig.ConfigMetaData()
            {
                Name = "my-service-name",
                Labels = new Dictionary<string, string>
                {
                    {"label1", "value1"}
                }
            };
            cfgFile.KubernetesDeploymentConfig.Spec = new KubernetesDeploymentConfig.DeploymentSpec()
            {
                Replicas = 9,
                Selector = new KubernetesDeploymentConfig.DeploymentSpec.ServiceSelector()
                {
                    MatchLabels = new Dictionary<string, string>()
                    {
                        {"label2", "value2"}
                    }
                },
                Template = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate()
                {
                    MetaData = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.ServiceMetaData()
                    {
                        Labels = new Dictionary<string, string>
                        {
                            {
                                "label3", "value3"
                            }
                        }
                    },
                    Spec = new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec()
                    {
                        Containers =
                            new List<KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.Container>
                            {
                                {
                                    new KubernetesDeploymentConfig.DeploymentSpec.ServiceTemplate.TemplateSpec.
                                        Container()
                                        {
                                            Name = "my-container",
                                            Image = "my-container-image",
                                            Ports = new List<KubernetesConfig.ServicePorts>()
                                            {
                                                new KubernetesConfig.ServicePorts()
                                                {
                                                    ContainerPort = 1234
                                                }
                                            }
                                        }
                                }
                            }
                    }
                }
            };
            cfgFile.Store();
            File.ReadAllText(_configFile).ShouldBe(SampleConfig);
        }

        private const string SampleConfig = @"apiVersion: apps/v1
kind: Deployment
metadata:
  name: my-service-name
  labels:
    label1: value1
spec:
  replicas: 9
  selector:
    matchLabels:
      label2: value2
  template:
    metadata:
      labels:
        label3: value3
    spec:
      containers:
      - name: my-container
        image: my-container-image
        ports:
        - containerPort: 1234
";
    }
}
