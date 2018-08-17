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

using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace Steeltoe.Tooling.DotnetCli.Test
{
    public partial class ToolingConfigurationFeature
    {
        [Scenario]
        public void StoreToStream()
        {
            Runner.RunScenario(
                given => a_tooling_configuration(),
                when => the_target_is_set("myTarget"),
                and => a_service_is_added("myService", "myServiceType"),
                and => the_tooling_configuration_is_stored(),
                then => the_stored_content_should_be(SampleConfig)
            );
        }

        [Scenario]
        public void LoadFromStream()
        {
            Runner.RunScenario(
                given => a_stream_containing(SampleConfig),
                when => the_tooling_configuration_is_loaded(),
                then => the_target_should_be("myTarget"),
                and => a_service_should_be("myService", "myServiceType")
            );
        }

        private const string SampleConfig = @"target: myTarget
services:
  myService:
    type: myServiceType
";
    }
}
