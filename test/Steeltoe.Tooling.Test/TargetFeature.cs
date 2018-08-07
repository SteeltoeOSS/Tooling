using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
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