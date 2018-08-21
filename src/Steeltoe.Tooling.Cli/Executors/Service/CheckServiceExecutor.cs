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
using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Cli.Executors.Service
{
    public class CheckServiceExecutor : ServiceExecutor
    {
        public CheckServiceExecutor(string name) : base(name)
        {
        }

        public override bool Execute(Configuration config, Shell shell, TextWriter output)
        {
            if (!config.services.ContainsKey(Name))
            {
                throw new CommandException($"Unknown service '{Name}'");
            }

            var result = shell.Run("cf", $"service {Name}");
            var status = "offline";
            if (result.Out != null)
            {
                Regex exp = new Regex(@"^status:\s+(.*)$", RegexOptions.Multiline);
                Match match = exp.Match(result.Out);
                if (match.Groups[1].ToString().TrimEnd().Equals("create succeeded"))
                {
                    status = "online";
                }
            }

            output.WriteLine(status);
            return false;
        }
    }
}
