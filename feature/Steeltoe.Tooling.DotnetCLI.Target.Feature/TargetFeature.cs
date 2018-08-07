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

using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

[assembly: LightBddScope]

namespace Steeltoe.Tooling.Test
{
	[Label("target")]
	[FeatureDescription(
@"In order to easily test code in a variety of environments
As a developer
I want to simple tell Steeltoe Tooling what enviornment in which I want to run")]
	public partial class TargetFeature
	{
		[Scenario]
		public void RunTarget()
		{
            Runner.RunScenario(
                given => a_blank_project(),
                when => the_developer_runs("target"),
                then => the_command_succeeds(),
                and => the_developer_sees_help_message_for_target_command());
		}
	}
}
