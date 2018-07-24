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

namespace Steeltoe.Tooling
{
    [Command(Description = "Run a health check on your Steeltoe development environment")]
    internal class DoctorCommand : DotnetSteeltoeCommand
    {
        protected override int OnExecute(CommandLineApplication app)
        {
            bool healthy = true;
            healthy = CheckDotnet() && healthy;
            return healthy ? 0 : 1;
        }

        private bool CheckDotnet()
        {
            Console.Write("checking dotnet, ");
            ProcessStartInfo pinfo = new ProcessStartInfo("dotnet", "--version");
            pinfo.RedirectStandardOutput = true;
            pinfo.RedirectStandardError = true;
            Process proc;
            try
            {
                proc = Process.Start(pinfo);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                if (e.Message.Equals("The system cannot find the file specified"))
                {
                    Console.WriteLine("not found");
                }
                else
                {
                    Console.WriteLine("unexpected error: " + e.Message);

                }
                return false;
            }
            proc.WaitForExit();
            if (proc.ExitCode == 0)
            {
                using (System.IO.StreamReader pout = proc.StandardOutput)
                {
                    Console.WriteLine("found version " + pout.ReadToEnd().Trim());
                }
            }
            else
            {
                using (System.IO.StreamReader perr = proc.StandardError)
                {
                    Console.WriteLine("oops ... " + perr.ReadToEnd().Trim());
                }
            }
            return proc.ExitCode == 0;
        }
    }
}
