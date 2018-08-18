using System.IO;

namespace Steeltoe.Tooling.DotnetCli
{
    public interface IExecutor
    {
        void Execute(TextWriter output);
    }
}
