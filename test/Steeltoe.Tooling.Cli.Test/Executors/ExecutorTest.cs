using System.IO;

namespace Steeltoe.Tooling.Cli.Test.Executors
{
    public abstract class ExecutorTest
    {
        protected Configuration Config;

        protected MockShell Shell;

        protected StringWriter Output;

        protected ExecutorTest()
        {
            Config = new Configuration();
            Shell = new MockShell();
            Output = new StringWriter();
        }
    }
}
