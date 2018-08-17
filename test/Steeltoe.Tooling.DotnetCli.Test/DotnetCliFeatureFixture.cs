using LightBDD.XUnit2;
using Shouldly;
using System.IO;

namespace Steeltoe.Tooling.DotnetCli.Test
{
    public class DotnetCliFeatureFixture : FeatureFixture
    {
        protected IDotnetCliCommand Command { get; set; }

        protected ToolingConfiguration Config { get; set; }

        protected TextWriter OutStream { get; set; }

        //
        // Givens
        //

        protected void a_tooling_configuration()
        {
            Config = new ToolingConfiguration();
            OutStream = new StringWriter();
        }

        protected void the_output_should_be(string text)
        {
            OutStream.ToString().Trim().ShouldBe(text);
        }

        protected void the_target_should_be(string name)
        {
            Config.target.ShouldBe(name);
        }
    }
}
