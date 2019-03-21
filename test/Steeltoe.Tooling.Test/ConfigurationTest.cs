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
    public class ConfigurationTest
    {
        [Fact]
        public void TestChangeEvent()
        {
            var cfg = new Configuration();
            var listener = new MyListener();
            cfg.AddListener(listener);
            cfg.NotifyListeners();
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
