namespace Steeltoe.Tooling.Cli.Test
{
    public class MockShell : Shell
    {
        public string LastCommand { get; private set; }

        public string NextResponse { get; set; }

        public override Result Run(string command, string arguments = null, string workingDirectory = null)
        {
            LastCommand = $"{command} {arguments}";
            var result = new Result();
            result.Out = NextResponse;
            return result;
        }
    }
}
