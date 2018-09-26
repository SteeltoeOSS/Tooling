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

namespace Steeltoe.Tooling.CloudFoundry
{
    public class CloudFoundryEnvironment : Environment
    {
        public CloudFoundryEnvironment(EnvironmentConfiguration config) : base(config)
        {
        }

        public override IServiceBackend GetServiceBackend(Context context)
        {
            return new CloudFoundryServiceBackend(context);
        }

        public override bool IsHealthy(Shell shell)
        {
            var cli = new CloudFoundryCli(shell);
            try
            {
                shell.Console.Write($"Cloud Foundry ... ");
                shell.Console.WriteLine(cli.Run("--version").Trim());
            }
            catch (ShellException)
            {
                shell.Console.WriteLine($"!!! {cli.Command} command not found");
                return false;
            }

            try
            {
                shell.Console.Write("logged into Cloud Foundry ... ");
                cli.Run("target");
                shell.Console.WriteLine("yes");
            }
            catch (ToolingException)
            {
                shell.Console.WriteLine("!!! no");
                return false;
            }

            return true;
        }
    }
}
