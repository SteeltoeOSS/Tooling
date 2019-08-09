// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text.RegularExpressions;

namespace Steeltoe.Tooling.Kubernetes
{
    internal class KubernetesTarget : Target
    {
        internal KubernetesTarget(TargetConfiguration configuration) : base(configuration)
        {
        }

        public override IDriver GetDriver(Context context)
        {
            return new KubernetesDriver(context);
        }

        public override bool IsHealthy(Context context)
        {
            var cli = new KubernetesCli(context.Shell);
            try
            {
                context.Console.Write($"Kubernetes ... ");
                var versionInfo = cli.Run("version", "getting Kubernetes CLI version");
                var matcher =
                    new Regex(@"Client Version:.*Major:""(\d+).*Minor:""(\d+)", RegexOptions.Multiline).Match(
                        versionInfo);
                var clientVersion = $"{matcher.Groups[1]}.{matcher.Groups[2]}";
                matcher =
                    new Regex(@"Server Version:.*Major:""(\d+).*Minor:""(\d+)", RegexOptions.Multiline).Match(
                        versionInfo);
                var serverVersion = $"{matcher.Groups[1]}.{matcher.Groups[2]}";
                context.Console.WriteLine($"kubectl client version {clientVersion}, server version {serverVersion}");
            }
            catch (ShellException)
            {
                context.Console.WriteLine($"!!! {cli.Command} command not found");
                return false;
            }

            try
            {
                context.Console.Write("current context ... ");
                var contextInfo = cli.Run("config get-contexts", "getting Kubernetes context");
                var matcher = new Regex(@"\*\s+(\S+)", RegexOptions.Multiline).Match(contextInfo);
                context.Console.WriteLine(matcher.Groups[1]);
            }
            catch (ToolingException)
            {
                context.Console.WriteLine("!!!");
                return false;
            }

            return true;
        }
    }
}
