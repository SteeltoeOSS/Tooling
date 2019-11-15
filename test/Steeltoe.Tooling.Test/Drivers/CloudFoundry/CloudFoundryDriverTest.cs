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
using Steeltoe.Tooling.Drivers.CloudFoundry;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.CloudFoundry
{
    public class CloudFoundryDriverTest : ToolingTest
    {
        private readonly CloudFoundryDriver _driver;

        public CloudFoundryDriverTest()
        {
            Context.Configuration.Target = "cloud-foundry";
            _driver = Context.Target.GetDriver(Context) as CloudFoundryDriver;
        }

        [Fact]
        public void TestDeployAppWindows()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddApp("my-app", "dummy-framework", "win");
            _driver.DeployApp("my-app");
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("dotnet publish -f dummy-framework -r win");
            Shell.Commands[1].ShouldBe("cf push -f manifest-steeltoe.yml -p bin/Debug/dummy-framework/win/publish");
            var manifestFile =
                new CloudFoundryManifestFile(Path.Combine(Context.ProjectDirectory, "manifest-steeltoe.yml"));
            manifestFile.Exists().ShouldBeTrue();
            manifestFile.CloudFoundryManifest.Applications.Count.ShouldBe(1);
            var app = manifestFile.CloudFoundryManifest.Applications[0];
            app.Name.ShouldBe("my-app");
            app.Command.ShouldBe($"cmd /c .\\{Path.GetFileName(Context.ProjectDirectory)}");
            app.BuildPacks.Count.ShouldBe(1);
            app.BuildPacks[0].ShouldBe("hwc_buildpack");
            app.Stack.ShouldBe("windows");
            app.Memory.ShouldBe("512M");
            app.Environment["ASPNETCORE_ENVIRONMENT"].ShouldBe("development");
            app.ServiceNames[0].ShouldBe("my-service");
            app.ServiceNames.Count.ShouldBe(1);
        }

        [Fact]
        public void TestDeployAppUbuntu()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.AddApp("my-app", "dummy-framework", "ubuntu");
            _driver.DeployApp("my-app");
            Shell.Commands.Count.ShouldBe(2);
            Shell.Commands[0].ShouldBe("dotnet publish -f dummy-framework -r ubuntu");
            Shell.Commands[1].ShouldBe("cf push -f manifest-steeltoe.yml -p bin/Debug/dummy-framework/ubuntu/publish");
            var manifestFile =
                new CloudFoundryManifestFile(Path.Combine(Context.ProjectDirectory, "manifest-steeltoe.yml"));
            manifestFile.Exists().ShouldBeTrue();
            manifestFile.CloudFoundryManifest.Applications.Count.ShouldBe(1);
            var app = manifestFile.CloudFoundryManifest.Applications[0];
            app.Name.ShouldBe("my-app");
            app.Command.ShouldBe($"cd ${{HOME}} && ./{Path.GetFileName(Context.ProjectDirectory)}");
            app.BuildPacks.Count.ShouldBe(1);
            app.BuildPacks[0].ShouldBe("dotnet_core_buildpack");
            app.Stack.ShouldBeNull();
            app.Memory.ShouldBe("512M");
            app.Environment["ASPNETCORE_ENVIRONMENT"].ShouldBe("development");
            app.ServiceNames[0].ShouldBe("my-service");
            app.ServiceNames.Count.ShouldBe(1);
        }

        [Fact]
        public void TestUndeployApp()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            _driver.UndeployApp("my-app");
            Shell.LastCommand.ShouldBe("cf delete my-app -f");
        }

        [Fact]
        public void TestDeployService()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service dummy-server dummy-plan my-service");
        }

        [Fact]
        public void TestDeployServiceWithArgs()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Context.Configuration.SetServiceArgs("my-service", "cloud-foundry", "arg1 \"arg2\"");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service dummy-server dummy-plan my-service arg1 \"\"\"arg2\"\"\"");
        }

        [Fact]
        public void TestDeployConfigServer()
        {
            Context.Configuration.AddService("my-service", "config-server");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p-config-server standard my-service");
        }

        [Fact]
        public void TestDeployEurekaServer()
        {
            Context.Configuration.AddService("my-service", "eureka-server");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p-service-registry standard my-service");
        }

        [Fact]
        public void TestDeployHystrixDashboard()
        {
            Context.Configuration.AddService("my-service", "hystrix-dashboard");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p-circuit-breaker-dashboard standard my-service");
        }

        [Fact]
        public void TestDeployMySql()
        {
            Context.Configuration.AddService("my-service", "mysql");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p.mysql db-small my-service");
        }

        [Fact]
        public void TestDeployPostgreSql()
        {
            Context.Configuration.AddService("my-service", "postgresql");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service postgresql-10-odb standalone my-service");
        }

        [Fact]
        public void TestDeployRabbitMq()
        {
            Context.Configuration.AddService("my-service", "rabbitmq");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p-rabbitmq standard my-service");
        }

        [Fact]
        public void TestDeployRedis()
        {
            Context.Configuration.AddService("my-service", "redis");
            _driver.DeployService("my-service");
            Shell.LastCommand.ShouldBe("cf create-service p-redis shared-vm my-service");
        }

        [Fact]
        public void TestDeployUnavailableService()
        {
            Context.Configuration.AddService("my-service", "zipkin");
            var e = Assert.Throws<ToolingException>(
                () => _driver.DeployService("my-service")
            );
            e.Message.ShouldBe("No Cloud Foundry service available for 'my-service' [zipkin]");
        }

        [Fact]
        public void TestUndeployService()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            _driver.UndeployService("my-service");
            Shell.LastCommand.ShouldBe("cf delete-service my-service -f");
        }

        [Fact]
        public void TestCheckApp()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            _driver.GetAppStatus("my-app");
            Shell.LastCommand.ShouldBe("cf app my-app");
        }

        [Fact]
        public void TestCheckService()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            _driver.GetServiceStatus("my-service");
            Shell.LastCommand.ShouldBe("cf service my-service");
        }

        [Fact]
        public void TestAppOnline()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            Shell.AddResponse("#0   running   2018-11-02T16:32:37Z   16.9%   129.2M of 512M   124.8M of 1G");
            var status = _driver.GetAppStatus("my-app");
            status.ShouldBe(Lifecycle.Status.Online);
        }

        [Fact]
        public void TestAppOffline()
        {
            Context.Configuration.AddApp("my-app", "dummy-framework", "dummy-runtime");
            Shell.AddResponse("App my-app not found.", -1);
            _driver.GetAppStatus("my-app").ShouldBe(Lifecycle.Status.Offline);
            Shell.AddResponse("App 'my-app' not found.", -1);
            _driver.GetAppStatus("my-app").ShouldBe(Lifecycle.Status.Offline);
        }

        [Fact]
        public void TestServiceStarting()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Shell.AddResponse("status:    create in progress");
            var status = _driver.GetServiceStatus("my-service");
            status.ShouldBe(Lifecycle.Status.Starting);
        }

        [Fact]
        public void TestServiceOnline()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Shell.AddResponse("status:    create succeeded");
            var status = _driver.GetServiceStatus("my-service");
            status.ShouldBe(Lifecycle.Status.Online);
        }

        [Fact]
        public void TestServiceOffline()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Shell.AddResponse("Service instance my-service not found", 1);
            var status = _driver.GetServiceStatus("my-service");
            status.ShouldBe(Lifecycle.Status.Offline);
        }

        [Fact]
        public void TestServiceStopping()
        {
            Context.Configuration.AddService("my-service", "dummy-svc");
            Shell.AddResponse("status:    delete in progress");
            var status = _driver.GetServiceStatus("my-service");
            status.ShouldBe(Lifecycle.Status.Stopping);
        }
    }
}
