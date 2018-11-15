// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Shouldly;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class ConfigurationTest : ToolingTest
    {
        private readonly Configuration _cfg;

        private readonly MyListener _listener;

        public ConfigurationTest()
        {
            _cfg = new Configuration();
            _listener = new MyListener();
            _cfg.AddListener(_listener);
        }

        [Fact]
        public void TestAddApp()
        {
            _cfg.AddApp("my-app");
            _cfg.Apps.Count.ShouldBe(1);
            _cfg.Apps.Keys.ShouldContain("my-app");
            _listener.ReceivedCount.ShouldBe(1);
        }

        [Fact]
        public void TestAddAlreadyExistingApp()
        {
            _cfg.AddApp("preexisting-app");
            var e = Assert.Throws<ItemExistsException>(
                () => _cfg.AddApp("preexisting-app")
            );
            e.Name.ShouldBe("preexisting-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestRemoveApp()
        {
            _cfg.AddApp("my-app");
            _cfg.RemoveApp("my-app");
            _cfg.Services.Count.ShouldBe(0);
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestRemoveUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.RemoveApp("no-such-app")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestGetApps()
        {
            _cfg.GetApps().Count.ShouldBe(0);
            _cfg.AddApp("my-app");
            _cfg.AddApp("another-app");
            var apps = _cfg.GetApps();
            apps.Remove("my-app").ShouldBeTrue();
            apps.Remove("another-app").ShouldBeTrue();
            apps.Count.ShouldBe(0);
        }

        [Fact]
        public void TestGetAppInfo()
        {
            _cfg.AddApp("my-app");
            var info = _cfg.GetAppInfo("my-app");
            info.App.ShouldBe("my-app");
        }

        [Fact]
        public void TestGetAppInfoUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetAppInfo("no-such-app")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestSetAppArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.SetAppArgs("my-app", "arg1 arg2");
            _cfg.Apps["my-app"].Args.ShouldBe("arg1 arg2");
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestSetAppTargetArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.SetAppArgs("my-app", "dummy-target", "arg1 arg2");
            _cfg.Apps["my-app"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestSetAppArgsUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetAppArgs("no-such-app", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestSetAppTargetArgsUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetAppArgs("no-such-app", "dummy-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestSetAppTargetArgsUnknownTarget()
        {
            _cfg.AddApp("my-app");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetAppArgs("my-app", "no-such-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestGetAppArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.SetAppArgs("my-app", "arg1 arg2");
            _cfg.GetAppArgs("my-app").ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetAppTargetArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.SetAppArgs("my-app", "dummy-target", "arg1 arg2");
            _cfg.GetAppArgs("my-app", "dummy-target").ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetAppNoArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.GetAppArgs("my-app").ShouldBe(null);
        }

        [Fact]
        public void TestGetAppTargetNoArgs()
        {
            _cfg.AddApp("my-app");
            _cfg.GetAppArgs("my-app", "dummy-target").ShouldBe(null);
        }

        [Fact]
        public void TestGetAppArgsUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetAppArgs("no-such-app")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestGetAppTargetArgsUnknownApp()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetAppArgs("no-such-app", "dummy-target")
            );
            e.Name.ShouldBe("no-such-app");
            e.Description.ShouldBe("app");
        }

        [Fact]
        public void TestGetAppTargetArgsUnknownTarget()
        {
            _cfg.AddApp("my-app");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetAppArgs("my-app", "no-such-target")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestAddServiceUnknownServiceType()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.AddService("my-service", "no-such-service-type")
            );
            e.Name.ShouldBe("no-such-service-type");
            e.Description.ShouldBe("service type");
        }

        [Fact]
        public void TestAddService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.Services.Count.ShouldBe(1);
            _cfg.Services.Keys.ShouldContain("my-service");
            _cfg.Services["my-service"].ServiceTypeName.ShouldBe("dummy-svc");
            _listener.ReceivedCount.ShouldBe(1);
        }

        [Fact]
        public void TestAddAlreadyExistingService()
        {
            _cfg.AddService("preexisting-service", "dummy-svc");
            var e = Assert.Throws<ItemExistsException>(
                () => _cfg.AddService("preexisting-service", "dummy-svc")
            );
            e.Name.ShouldBe("preexisting-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestRemoveService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.RemoveService("my-service");
            _cfg.Services.Count.ShouldBe(0);
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestRemoveUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.RemoveService("no-such-service")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestGetServices()
        {
            _cfg.GetServices().Count.ShouldBe(0);
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.AddService("another-service", "dummy-svc");
            var svcs = _cfg.GetServices();
            svcs.Remove("my-service").ShouldBeTrue();
            svcs.Remove("another-service").ShouldBeTrue();
            svcs.Count.ShouldBe(0);
        }

        [Fact]
        public void TestGetServiceInfo()
        {
            _cfg.AddService("my-service", "dummy-svc");
            var info = _cfg.GetServiceInfo("my-service");
            info.Service.ShouldBe("my-service");
            info.ServiceType.ShouldBe("dummy-svc");
        }

        [Fact]
        public void TestGetServiceInfoUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceInfo("no-such-service")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestSetServiceArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceArgs("my-service", "arg1 arg2");
            _cfg.Services["my-service"].Args.ShouldBe("arg1 arg2");
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestSetServiceTargetArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceArgs("my-service", "dummy-target", "arg1 arg2");
            _cfg.Services["my-service"].DeployArgs["dummy-target"].ShouldBe("arg1 arg2");
            _listener.ReceivedCount.ShouldBe(2);
        }

        [Fact]
        public void TestSetServiceArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetServiceArgs("no-such-service", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestSetServiceTargetArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetServiceArgs("no-such-service", "dummy-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestSetServiceTargetArgsUnknownTarget()
        {
            _cfg.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetServiceArgs("my-service", "no-such-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestGetServiceArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceArgs("my-service", "arg1 arg2");
            _cfg.GetServiceArgs("my-service").ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceTargetArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceArgs("my-service", "dummy-target", "arg1 arg2");
            _cfg.GetServiceArgs("my-service", "dummy-target").ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceNoArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.GetServiceArgs("my-service").ShouldBe(null);
        }

        [Fact]
        public void TestGetServiceTargetNoArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.GetServiceArgs("my-service", "dummy-target").ShouldBe(null);
        }

        [Fact]
        public void TestGetServiceArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceArgs("no-such-service")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestGetServiceTargetArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceArgs("no-such-service", "dummy-target")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestGetServiceTargetArgsUnknownTarget()
        {
            _cfg.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceArgs("my-service", "no-such-target")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        private class MyListener : IConfigurationListener
        {
            private int _receivedCount;

            internal int ReceivedCount
            {
                get => _receivedCount;
                private set => _receivedCount = value;
            }

            public void ConfigurationChangeEvent()
            {
                ++ReceivedCount;
            }
        }
    }
}
