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

using System.IO;

namespace Steeltoe.Tooling.Cli.Environments.Docker
{
    public class DockerEnvironment : IEnvironment
    {
        public string GetName()
        {
            return "docker";
        }

        public IServiceManager GetServiceManager()
        {
            return new DockerServiceManager();
        }

        public bool IsSane(Shell shell, TextWriter output)
        {
            var cli = new DockerCli(shell);
            try
            {
                output.Write($"checking {cli.Command} ... ");
                output.WriteLine(cli.GetVersion());
            }
            catch (ShellException e)
            {
                output.WriteLine($"ERROR: {e.Message.Trim()}");
                return false;
            }

            return true;
        }
    }
}
