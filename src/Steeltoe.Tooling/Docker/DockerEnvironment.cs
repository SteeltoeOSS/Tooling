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
        public DockerEnvironment() : base("docker", "Docker")
        {
        }

        public override IServiceBackend GetServiceBackend(Context context)
        {
            return new DockerServiceBackend(context.Shell);
        }

        public override bool IsSane(Shell shell)
        {
            try
            {
                shell.Console.Write("checking Docker version ... ");
                var dockerInfo = new DockerCli(shell).Run("info");
                Regex exp = new Regex(@"Server Version:\s*(\S+)", RegexOptions.Multiline);
                Match match = exp.Match(dockerInfo);
                shell.Console.WriteLine(match.Groups[1].ToString());
                return true;
            }
            catch (CliException e)
            {
                shell.Console.WriteLine($"ERROR: {e.Message}");
                return false;
            }
        }
    }
}
