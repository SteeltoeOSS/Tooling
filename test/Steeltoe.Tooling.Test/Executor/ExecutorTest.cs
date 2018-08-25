using System.IO;
using Steeltoe.Tooling.Test.System;

namespace Steeltoe.Tooling.Test.Executor
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
