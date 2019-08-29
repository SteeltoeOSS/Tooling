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
using Steeltoe.Tooling.Drivers.Kubernetes;
using Xunit;

namespace Steeltoe.Tooling.Test.Drivers.Kubernetes
{
    public class KubernetesTargetTest : ToolingTest
    {
        private readonly KubernetesTarget _target;

        public KubernetesTargetTest()
        {
            _target = Registry.GetTarget("kubernetes") as KubernetesTarget;
        }

        [Fact]
        public void TestGetName()
        {
            _target.Name.ShouldBe("kubernetes");
        }

        [Fact]
        public void TestGetDescription()
        {
            _target.Description.ShouldBe("Kubernetes");
        }

        [Fact]
        public void TestGetDriver()
        {
            _target.GetDriver(Context).ShouldBeOfType<KubernetesDriver>();
        }

        [Fact]
        public void TestIsHealthy()
        {
            Shell.AddResponse(@"Client Version: version.Info{Major:""9"", Minor:""87"", ...
Server Version: version.Info{Major:""6"", Minor:""54"", ...");
            Shell.AddResponse(@"CURRENT   NAME       CLUSTER    AUTHINFO   NAMESPACE
*         context1   cluster1   authinfo1
          context2   cluster2   authinfo2
");
            var healthy = _target.IsHealthy(Context);
            healthy.ShouldBeTrue();
            var expected = new[]
            {
                "kubectl version",
                "kubectl config get-contexts",
            };
            Shell.Commands.Count.ShouldBe(expected.Length);
            for (int i = 0; i < expected.Length; ++i)
            {
                Shell.Commands[i].ShouldBe(expected[i]);
            }
            Console.ToString().ShouldContain("Kubernetes ... kubectl client version 9.87, server version 6.54");
            Console.ToString().ShouldContain("current context ... context1");
        }
    }
}
