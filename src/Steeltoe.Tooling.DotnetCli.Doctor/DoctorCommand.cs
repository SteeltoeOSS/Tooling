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

using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;

namespace Steeltoe.Tooling.DotnetCli.Doctor
{
    [Command(Description = "Run a health check on your Steeltoe development environment.")]
    public class DoctorCommand
    {
        protected int OnExecute(CommandLineApplication app)
        {
            app.Out.WriteLine("*** ******************************* ***");
            app.Out.WriteLine("*** This command under construction ***");
            app.Out.WriteLine("*** ******************************* ***");
            app.Out.WriteLine();
            var healthy = true;
            healthy = CheckDotnet(app) && healthy;
            return healthy ? 0 : 1;
        }

        private static bool CheckDotnet(CommandLineApplication app)
        {
            Console.Write("checking dotnet, ");
            var pinfo = new ProcessStartInfo("dotnet", "--version")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            try
            {
                var proc = Process.Start(pinfo);
                proc.WaitForExit();
                if (proc.ExitCode == 0)
                {
                    using (var pout = proc.StandardOutput)
                    {
                        app.Out.WriteLine("found version " + pout.ReadToEnd().Trim());
                    }
                }
                else
                {
                    using (var perr = proc.StandardError)
                    {
                        app.Out.WriteLine("oops ... " + perr.ReadToEnd().Trim());
                    }
                }

                return proc.ExitCode == 0;
            }
            catch (Exception e)
            {
                app.Out.WriteLine("error: " + e.Message);
                return false;
            }
        }
    }
}
