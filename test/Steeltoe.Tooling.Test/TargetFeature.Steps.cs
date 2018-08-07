using LightBDD.Framework;
using LightBDD.XUnit2;
using Shouldly;

namespace Steeltoe.Tooling.Test
{
	public partial class TargetFeature: FeatureFixture
	{
		private void a_blank_project()
		{
		}

		private void the_developer_runs(string command)
		{
            command.ShouldBe("target");
		}

        private void the_developer_sees_help_message_for_target_command()
        {
        }

        private void the_command_succeeds()
        {
        }


	}
}