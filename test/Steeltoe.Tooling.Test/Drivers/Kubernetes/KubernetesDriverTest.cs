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

using System.IO;
using Shouldly;
using Steeltoe.Tooling.Drivers.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.Kubernetes
{
    public class KubernetesDriverTest : ToolingTest
    {
        private readonly KubernetesDriver _driver;

        public KubernetesDriverTest()
        {
            Context.Configuration.Target = "kubernetes";
            _driver = Context.Target.GetDriver(Context) as KubernetesDriver;
        }

        [Fact]
        public void TestDeployApp()
        {
            Context.Configuration.AddApp("my-App", "dummy-framework", "dummy-runtime");
            _driver.DeployApp("my-App");
            // Dockerfile
            var dockerfileFile =
                new KubernetesDotnetAppDockerfileFile(Path.Join(Context.ProjectDirectory, "Dockerfile"));
            dockerfileFile.Exists().ShouldBeTrue();
            // service config
            var svcCfgFile =
                new KubernetesServiceConfigFile(Path.Join(Context.ProjectDirectory, "my-App-service.yaml"));
            svcCfgFile.Exists().ShouldBeTrue();
            // commands
            Shell.Commands.Count.ShouldBe(3);
            Shell.Commands[0].ShouldBe("docker build --tag my-app .");
            Shell.Commands[1].ShouldBe("kubectl apply --filename my-App-deployment.yaml");
            Shell.Commands[2].ShouldBe("kubectl apply --filename my-App-service.yaml");
        }

        [Fact]
        public void TestUndeployApp()
        {
            Context.Configuration.AddApp("my-App", "dummy-framework", "dummy-runtime");
            _driver.UndeployApp("my-App");
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("kubectl delete --filename my-App-service.yaml");
            Shell.Commands[1].ShouldBe("kubectl delete --filename my-App-deployment.yaml");
        }

        [Fact]
        public void TestDeployService()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            _driver.DeployService("my-Service");
            // deployment config
            var deployCfgFile =
                new KubernetesDeploymentConfigFile(Path.Join(Context.ProjectDirectory, "my-Service-deployment.yaml"));
            deployCfgFile.Exists().ShouldBeTrue();
            var deployCfg = deployCfgFile.KubernetesDeploymentConfig;
            // deployCfg.ApiVersion.ShouldBe("apps/v1");
            // deployCfg.Kind.ShouldBe("Deployment");
            // deployCfg.MetaData.Name.ShouldBe("my-service");
            // deployCfg.MetaData.Labels["app"].ShouldBe("my-service");
            // deployCfg.Spec.Selector.MatchLabels["app"].ShouldBe("my-service");
            // deployCfg.Spec.Template.MetaData.Labels["app"].ShouldBe("my-service");
            // deployCfg.Spec.Template.Spec.Containers[0].Name.ShouldBe("my-service");
            // service config
            var svcCfgFile =
                new KubernetesServiceConfigFile(Path.Join(Context.ProjectDirectory, "my-Service-service.yaml"));
            svcCfgFile.Exists().ShouldBeTrue();
            // var svcCfg = new KubernetesServiceConfigFile("my-Service-service.yaml").KubernetesServiceConfig;
            // svcCfg.ApiVersion.ShouldBe("v1");
            // svcCfg.Kind.ShouldBe("Service");
            // svcCfg.MetaData.Name.ShouldBe("my-service");
            // svcCfg.Spec.Selector["app"].ShouldBe("my-service");
            // commands
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("kubectl apply --filename my-Service-deployment.yaml");
            Shell.Commands[1].ShouldBe("kubectl apply --filename my-Service-service.yaml");
        }

        [Fact]
        public void TestDeployConfigServer()
        {
            Context.Configuration.AddService("my-service", "config-server");
            _driver.DeployService("my-service");
            var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/config-server:2.0");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(8888);
            var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(8888);
        }

        [Fact]
        public void TestDeployEurekaServer()
        {
            Context.Configuration.AddService("my-service", "eureka-server");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/eureka-server:2.0");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(8761);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(8761);
        }

        [Fact]
        public void TestDeployHystrixDashboard()
        {
            Context.Configuration.AddService("my-service", "hystrix-dashboard");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/hystrix-dashboard:1.4");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(7979);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(7979);
        }

        [Fact]
        public void TestDeployMicrosoftSqlServer()
        {
            Context.Configuration.AddService("my-service", "mssql");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(1433);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(1433);
            // _driver.DeployService("my-service", "linux");
            // deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/mssql-amd64-linux:2017-CU11");
            // _driver.DeployService("my-service", "windows");
            // deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/mssql-amd64-windows:2017-CU1");
        }

        [Fact]
        public void TestDeployMySql()
        {
            Context.Configuration.AddService("my-service", "mysql");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/mysql:5.7");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(3306);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(3306);
        }

        [Fact]
        public void TestDeployPostgreSql()
        {
            Context.Configuration.AddService("my-service", "postgresql");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/postgresql:10.8");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(5432);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(5432);
        }

        [Fact]
        public void TestDeployRabbitMQ()
        {
            Context.Configuration.AddService("my-service", "rabbitmq");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/rabbitmq:3.7");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(5672);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(5672);
        }

        [Fact]
        public void TestDeployRedis()
        {
            Context.Configuration.AddService("my-service", "redis");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(6379);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(6379);
            // _driver.DeployService("my-service", "linux");
            // deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/redis-amd64-linux:4.0.11");
            // _driver.DeployService("my-service", "windows");
            // deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/redis-amd64-windows:3.0.504");
        }

        [Fact]
        public void TestDeployZipkin()
        {
            Context.Configuration.AddService("my-service", "zipkin");
            _driver.DeployService("my-service");
            // var deployCfg = new KubernetesDeploymentConfigFile("my-service-deployment.yaml").KubernetesDeploymentConfig;
            // deployCfg.Spec.Template.Spec.Containers[0].Image.ShouldBe("steeltoeoss/zipkin:2.11");
            // deployCfg.Spec.Template.Spec.Containers[0].Ports.Count.ShouldBe(1);
            // deployCfg.Spec.Template.Spec.Containers[0].Ports[0].ContainerPort.ShouldBe(9411);
            // var svcCfg = new KubernetesServiceConfigFile("my-service-service.yaml").KubernetesServiceConfig;
            // svcCfg.Spec.Ports[0].Port.ShouldBe(9411);
        }


        [Fact]
        public void TestUndeployService()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            _driver.UndeployService("my-Service");
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("kubectl delete --filename my-Service-service.yaml");
            Shell.Commands[1].ShouldBe("kubectl delete --filename my-Service-deployment.yaml");
        }

        [Fact]
        public void TestCheckApp()
        {
            Context.Configuration.AddApp("my-App", "dummy-framework", "dummy-runtime");
            _driver.GetAppStatus("my-App");
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("kubectl get pods --selector app=my-app");
            Shell.Commands[1].ShouldBe("kubectl get services my-app");
        }

        [Fact]
        public void TestCheckService()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            Shell.AddResponse("Running");
            _driver.GetServiceStatus("my-Service");
            Shell.Commands.Count.ShouldBe(1);
            Shell.Commands[0].ShouldBe("kubectl get pods --selector app=my-service");
//            Shell.Commands[1].ShouldBe("kubectl get services my-service");
        }

        [Fact]
        public void TestServiceStarting()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            Shell.AddResponse("Pending");
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Starting);
            Shell.AddResponse("ContainerCreating");
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Starting);
        }

        [Fact]
        public void TestServiceStopping()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            Shell.AddResponse("Terminating");
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Stopping);
            Shell.AddResponse("");
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Stopping);
        }

        [Fact]
        public void TestAppOnline()
        {
            Context.Configuration.AddApp("my-App", "dummy-framework", "dummy-runtime");
            Shell.AddResponse("Running");
            _driver.GetAppStatus("my-App").ShouldBe(Lifecycle.Status.Online);
        }

        [Fact]
        public void TestServiceOnline()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            Shell.AddResponse("Running");
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Online);
        }

        [Fact]
        public void TestServiceOffline()
        {
            Context.Configuration.AddService("my-Service", "dummy-svc");
            Shell.AddResponse("");
            Shell.AddResponse("", -1);
            _driver.GetServiceStatus("my-Service").ShouldBe(Lifecycle.Status.Offline);
        }
    }
}
