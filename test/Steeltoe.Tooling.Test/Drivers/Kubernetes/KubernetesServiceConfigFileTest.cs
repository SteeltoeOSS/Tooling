using System.Collections.Generic;
using Shouldly;
using System.IO;
using Steeltoe.Tooling.Drivers.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.Kubernetes
{
    public class KubernetesServiceConfigFileTest : ToolingTest
    {
        private readonly string _configFile;

        public KubernetesServiceConfigFileTest()
        {
            _configFile = Path.Join(Context.ProjectDirectory, "kubernetes-service-config-file");
        }

        // [Fact]
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
                    {"key1", "value1"},
                },
                Ports = new List<KubernetesConfig.ServicePorts>()
                {
                    new KubernetesConfig.ServicePorts()
                    {
                        Port = 1234,
                        TargetPort = 4321,
                    },
                    new KubernetesConfig.ServicePorts()
                    {
                        Port = 5678,
                        TargetPort = 8765,
                    },
                },
            };
            cfgFile.Store();
            File.ReadAllText(_configFile).Replace("\r", "").ShouldBe(SampleConfig);
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
  - port: 5678
    targetPort: 8765
";
    }
}
