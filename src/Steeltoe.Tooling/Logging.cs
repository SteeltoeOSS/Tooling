using Microsoft.Extensions.Logging;

namespace Steeltoe.Tooling
{
    public static class Logging
    {
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory().AddConsole(LogLevel.Debug);
    }
}
