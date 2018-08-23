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

namespace Steeltoe.Tooling.Cli.Environments.Docker
{
    internal class DockerCli
    {
        private readonly Shell _shell;

        internal string Command { get; } = "docker";

        internal DockerCli(Shell shell)
        {
            _shell = shell;
        }

        public string GetVersion()
        {
            return RunCommand("--version").Trim();
        }

        internal void StartContainer(string name, string image)
        {
            RunCommand($"run --detach --rm --name {name} {image}");
        }

        internal void StopContainer(string name)
        {
            RunCommand($"stop {name}");
        }

        internal string GetContainerInfo(string name)
        {
            return RunCommand($"ps --no-trunc --filter name=^/{name}$");
        }

        private string RunCommand(string arguments)
        {
            var result = _shell.Run(Command, arguments);
            if (result.ExitCode == 0) return result.Out;
            var error = result.Error.Trim();
            if (string.IsNullOrEmpty(error))
            {
                error = result.Out.Trim();
            }

            throw new CliException($"{Command} error: {error}");

        }
    }
}
