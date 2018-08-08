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

using System;
using System.IO;
using LightBDD.XUnit2;
using Shouldly;
using Steeltoe.Tooling.System;

namespace Steeltoe.Tooling.DotnetCLI.Base.Feature
{
    public class DotnetCLIFeatureFixture : FeatureFixture
    {
	    private string ProjectDirectory { get; } = Path.GetFullPath(Path.Combine(
		    Directory.GetCurrentDirectory(),
		    "../../../../../src/Steeltoe.Tooling.DotnetCLI"));

	    private string ProjectSandbox { get; set; }

	    private Shell Shell { get; } = new Shell();

		protected void a_blank_project(string name)
		{
			ProjectSandbox = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "features"), name);
			Log("Setting project sandbox to " + ProjectSandbox);
			Directory.CreateDirectory(ProjectSandbox);
			Directory.SetCurrentDirectory(ProjectSandbox);
			Log("Project directory is " + ProjectDirectory);
		}

		protected void the_developer_runs_steeltoe_(string command)
		{
            Shell.Run("dotnet", $"run --project {ProjectDirectory} -- {command}");
		}

        protected void the_command_fails()
        {
	        Shell.ExitCode.ShouldNotBe(0);
        }

	    protected void the_developer_sees_the_error_message(string message)
	    {
		    Shell.Error.ShouldContain(message);
	    }

	    private static void Log(string message)
	    {
		    Console.WriteLine("--> " + message);
	    }
    }
}
