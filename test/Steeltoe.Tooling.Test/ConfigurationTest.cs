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
        public void TestAddService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.Services.Count.ShouldBe(1);
            _cfg.Services.Keys.ShouldContain("my-service");
            _cfg.Services["my-service"].ServiceTypeName.ShouldBe("dummy-svc");
        }

        [Fact]
        public void TestListenAddService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _listener.ReceivedEvent.ShouldBeTrue();
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
        public void TestAddServiceUnknownServiceType()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.AddService("my-service", "no-such-service-type")
            );
            e.Name.ShouldBe("no-such-service-type");
            e.Description.ShouldBe("service type");
        }

        [Fact]
        public void TestRemoveService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.RemoveService("my-service");
            _cfg.Services.Count.ShouldBe(0);
        }

        [Fact]
        public void TestListenRemoveService()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _listener.ReceivedEvent = false;
            _cfg.RemoveService("my-service");
            _listener.ReceivedEvent.ShouldBeTrue();
        }

        [Fact]
        public void TestRemoveUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.RemoveService("no-such-service")
            );
            e.Name.ShouldBe("no-such-service");
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
        }

        [Fact]
        public void TestGetServiceNames()
        {
            _cfg.GetServices().Count.ShouldBe(0);
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.AddService("another-service", "dummy-svc");
            var names = _cfg.GetServices();
            names.Remove("my-service").ShouldBeTrue();
            names.Remove("another-service").ShouldBeTrue();
            names.Count.ShouldBe(0);
        }

        [Fact]
        public void TestSetServiceDeploymentArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceDeploymentArgs("my-service", "dummy-target", "arg1 arg2");
            _cfg.Services["my-service"].Args["dummy-target"].ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestListenSetServiceDeploymentArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _listener.ReceivedEvent = false;
            _cfg.SetServiceDeploymentArgs("my-service", "dummy-target", "arg1 arg2");
            _listener.ReceivedEvent.ShouldBeTrue();
        }

        [Fact]
        public void TestSetServiceDeploymentArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetServiceDeploymentArgs("no-such-service", "dummy-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestSetServiceDeploymentArgsUnknownEnvironment()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.SetServiceDeploymentArgs("my-service", "no-such-target", "arg1 arg2")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestGetServiceDeploymentArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.SetServiceDeploymentArgs("my-service", "dummy-target", "arg1 arg2");
            _cfg.GetServiceDeploymentArgs("my-service", "dummy-target").ShouldBe("arg1 arg2");
        }

        [Fact]
        public void TestGetServiceDeploymentNoArgs()
        {
            _cfg.AddService("my-service", "dummy-svc");
            _cfg.GetServiceDeploymentArgs("my-service", "dummy-target").ShouldBe(null);
        }

        [Fact]
        public void TestGetServiceDeploymentArgsUnknownService()
        {
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceDeploymentArgs("no-such-service", "dummy-target")
            );
            e.Name.ShouldBe("no-such-service");
            e.Description.ShouldBe("service");
        }

        [Fact]
        public void TestGetServiceDeploymentArgsUnknownTarget()
        {
            _cfg.AddService("my-service", "dummy-svc");
            var e = Assert.Throws<ItemDoesNotExistException>(
                () => _cfg.GetServiceDeploymentArgs("my-service", "no-such-target")
            );
            e.Name.ShouldBe("no-such-target");
            e.Description.ShouldBe("target");
        }

        [Fact]
        public void TestChangeEvent()
        {
            var cfg = new Configuration();
            var listener = new MyListener();
            cfg.AddListener(listener);
            cfg.NotifyChanged();
            listener.ReceivedEvent.ShouldBeTrue();
        }

        internal class MyListener : IConfigurationListener
        {
            internal bool ReceivedEvent { get; set; }

            public void ConfigurationChangeEvent()
            {
                ReceivedEvent = true;
            }
        }
    }
}
