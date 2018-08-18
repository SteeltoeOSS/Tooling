using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling.Cli
{
    public static class Logging
    {
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory().AddConsole(LogLevel.Information);
    }
}
