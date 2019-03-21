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

using Shouldly;
using Xunit;

namespace Steeltoe.Tooling.Test
{
    public class ServiceManagerTest : ToolingTest
    {
        private readonly ServiceManager _serviceMgr;

        public ServiceManagerTest()
        {
            _serviceMgr = new ServiceManager(Context);
        }

        [Fact]
        public void TestAddService()
        {
            var svc = _serviceMgr.AddService("a-service", "dummy-svc");
            svc.Name.ShouldBe("a-service");
            svc.Type.ShouldBe("dummy-svc");
            _serviceMgr	.GetServiceNames().Count.ShouldBe(1);
            _serviceMgr	.GetServiceNames()[0].ShouldBe("a-service");
        }

        [Fact]
        public void TestRemoveService()
        {
            _serviceMgr.AddService("a-service", "dummy-svc");
            _serviceMgr.RemoveService("a-service");
            _serviceMgr.HasService("a-service").ShouldBeFalse();
        }

        [Fact]
        public void TestRemoveNonExistentService()
        {
            Assert.Throws<ServiceNotFoundException>(
                () => _serviceMgr.RemoveService("non-existent-service")
            );
        }

        [Fact]
        public void TestGetService()
        {
            _serviceMgr.AddService("a-service", "dummy-svc");
            _serviceMgr.GetService("a-service").Name.ShouldBe("a-service");
        }

        [Fact]
        public void TestGetNonExistentService()
        {
            Assert.Throws<ServiceNotFoundException>(
                () => _serviceMgr.GetService("non-existent-service")
            );
        }

        [Fact]
        public void TestGetServiceNames()
        {
            _serviceMgr.AddService("a-service", "dummy-svc");
            _serviceMgr.AddService("another-service", "dummy-svc");
            var names = _serviceMgr.GetServiceNames();
            names.Remove("a-service").ShouldBeTrue();
            names.Remove("another-service").ShouldBeTrue();
            names.Count.ShouldBe(0);
        }

        [Fact]
        public void TestHasService()
        {
            _serviceMgr.HasService("a-service").ShouldBeFalse();
            _serviceMgr.AddService("a-service", "dummy-svc");
            _serviceMgr.HasService("a-service").ShouldBeTrue();
        }

        [Fact]
        public void TestServiceDeploymentArgs()
        {
            _serviceMgr.AddService("a-service", "dummy-svc");
            _serviceMgr.SetServiceDeploymentArgs("dummy-env", "a-service", "arg1 arg2");
            _serviceMgr.GetServiceDeploymentArgs("dummy-env", "a-service").ShouldBe("arg1 arg2");

        }
    }
}
