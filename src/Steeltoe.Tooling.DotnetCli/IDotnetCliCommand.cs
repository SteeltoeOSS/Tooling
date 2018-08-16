using System.IO;

namespace Steeltoe.Tooling.DotnetCli
{
    public interface IDotnetCliCommand
    {
        void Execute(TextWriter output);
    }
}
