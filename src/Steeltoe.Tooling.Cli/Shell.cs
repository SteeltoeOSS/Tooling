namespace Steeltoe.Tooling.Cli
{
    public abstract class Shell
    {
        public abstract Result Run(string command, string arguments = null, string workingDirectory = null);

        public struct Result
        {
            public int Id { get;  set; }
            public string Command { get; set; }
            public string Arguments { get; set; }
            public string WorkingDirectory { get; set; }
            public int ExitCode { get; set; }
            public string Out { get; set; }
            public string Error { get; set; }
        }
    }
}
