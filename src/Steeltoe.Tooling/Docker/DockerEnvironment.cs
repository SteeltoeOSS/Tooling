// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Docker
{
    public class DockerEnvironment : Environment
    {
        public DockerEnvironment(EnvironmentConfiguration config) : base(config)
        {
        }

        public override IServiceBackend GetServiceBackend(Context context)
        {
            return new DockerServiceBackend(context);
        }

        public override bool IsHealthy(Shell shell)
        {
            var console = shell.Console;
            console.Write("Docker CLI ... ");
            var cli = new DockerCli(shell);
            try
            {
                var dockerVersion = cli.Run("--version").Trim();
                console.WriteLine(dockerVersion);

                var dockerInfo = cli.Run("info");

                console.Write("Docker host OS ... ");
                shell.Console.WriteLine(new Regex(@"Operating System:\s*(.+)", RegexOptions.Multiline)
                    .Match(dockerInfo).Groups[1].ToString());

                console.Write("Docker container OS ... ");
                shell.Console.WriteLine(new Regex(@"OSType:\s*(.+)", RegexOptions.Multiline)
                    .Match(dockerInfo).Groups[1].ToString());

                return true;
            }
            catch (CliException e)
            {
                shell.Console.WriteLine($"!!! {e.Message}");
                return false;
            }
            catch (ShellException)
            {
                shell.Console.WriteLine($"!!! {cli.Command} command not found");
                return false;
            }
        }
    }
}
