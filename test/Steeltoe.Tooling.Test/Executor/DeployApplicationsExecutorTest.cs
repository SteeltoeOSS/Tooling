using Xunit;

namespace Steeltoe.Tooling.Test.Executor
{
    public class DeployApplicationsExecutorTest : ToolingTest
    {
        [Fact]
        public void TestDeployApplications()
        {
//            Context.ApplicationManager.AddApplication("an-app");
//            Context.ApplicationManager.AddApplication("another-app");
//            ClearConsole();
//            new DeployApplicationsExecutor().Execute(Context);
//            Console.ToString().ShouldContain("Deploying application 'an-app'");
//            Console.ToString().ShouldContain("Deploying application 'another-app'");
//            Context.ApplicationManager.GetApplicationState("an-app").ShouldBe(Lifecycle.State.Starting);
//            Context.ApplicationManager.GetApplicationState("another-app").ShouldBe(Lifecycle.State.Starting);
        }

        [Fact]
        public void TestDeployNoApplications()
        {
//            new DeployApplicationsExecutor().Execute(Context);
//            Console.ToString().Trim().ShouldBeEmpty();
        }

        [Fact]
        public void TestDeployNoTarget()
        {
//            Context.Configuration.EnvironmentName = null;
//            Context.ApplicationManager.AddApplication("an-app");
//            var e = Assert.Throws<ToolingException>(
//                () => new DeployApplicationsExecutor().Execute(Context)
//            );
//            e.Message.ShouldBe("Target deployment environment not set");
        }
    }
}
