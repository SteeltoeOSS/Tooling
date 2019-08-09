using System.Collections.Generic;
using Shouldly;
using System.IO;
using Steeltoe.Tooling.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Kubernetes
{
    public class KubernetesServiceConfigFileTest : ToolingTest
    {
        private readonly string _configFile;

        public KubernetesServiceConfigFileTest()
        {
            _configFile = Path.Join(Context.ProjectDirectory, "kubernetes-service-config-file");
        }

        [Fact]
        public void TestLoadFromFile()
        {
            File.WriteAllText(_configFile, SampleConfig);
            var cfgFile = new KubernetesServiceConfigFile(_configFile);
            cfgFile.KubernetesServiceConfig.ApiVersion.ShouldBe("v1");
            cfgFile.KubernetesServiceConfig.Kind.ShouldBe("Service");
            cfgFile.KubernetesServiceConfig.MetaData.Name.ShouldBe("my-service");
            cfgFile.KubernetesServiceConfig.MetaData.Labels.Count.ShouldBe(1);
            cfgFile.KubernetesServiceConfig.MetaData.Labels["label1"].ShouldBe("value1");
            cfgFile.KubernetesServiceConfig.Spec.Type.ShouldBe("ServiceType");
            cfgFile.KubernetesServiceConfig.Spec.Selector.Count.ShouldBe(1);
            cfgFile.KubernetesServiceConfig.Spec.Selector["key1"].ShouldBe("value1");
            cfgFile.KubernetesServiceConfig.Spec.Ports.Count.ShouldBe(1);
            cfgFile.KubernetesServiceConfig.Spec.Ports[0].Port.ShouldBe(1234);
            cfgFile.KubernetesServiceConfig.Spec.Ports[0].TargetPort.ShouldBe(4321);
        }

        [Fact]
        public void TestStoreToFile()
        {
            var cfgFile = new KubernetesServiceConfigFile(_configFile);
            cfgFile.KubernetesServiceConfig.ApiVersion = "v1";
            cfgFile.KubernetesServiceConfig.Kind = "Service";
            cfgFile.KubernetesServiceConfig.MetaData = new KubernetesConfig.ConfigMetaData()
            {
                Name = "my-service",
                Labels = new Dictionary<string, string>()
                {
                    {"label1", "value1"}
                }
            };
            cfgFile.KubernetesServiceConfig.Spec = new KubernetesServiceConfig.ServiceSpec()
            {
                Type = "ServiceType",
                Selector = new Dictionary<string, string>()
                {
                    {"key1", "value1"}
                },
                Ports = new List<KubernetesConfig.ServicePorts>()
                {
                    new KubernetesConfig.ServicePorts()
                    {
                        Port = 1234,
                        TargetPort = 4321
                    }
                }
            };
            cfgFile.Store();
            File.ReadAllText(_configFile).ShouldBe(SampleConfig);
        }

        private const string SampleConfig = @"apiVersion: v1
kind: Service
metadata:
  name: my-service
  labels:
    label1: value1
spec:
  type: ServiceType
  selector:
    key1: value1
  ports:
  - port: 1234
    targetPort: 4321
";
    }
}
