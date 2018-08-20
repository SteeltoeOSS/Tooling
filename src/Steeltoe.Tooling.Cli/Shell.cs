namespace Steeltoe.Tooling.Cli
{
    public abstract class Shell
    {
        public abstract Result Run(string command, string arguments = null, string workingDirectory = null);

        public struct Result
        {
            public int Id { get; internal set; }
            public string Command { get; internal set; }
            public string Arguments { get; internal set; }
            public string WorkingDirectory { get; internal set; }
            public int ExitCode { get; internal set; }
            public string Out { get; set; }
            public string Error { get; internal set; }
        }
    }
}
