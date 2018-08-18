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
using LightBDD.XUnit2;
using Shouldly;

namespace Steeltoe.Tooling.Cli.Test
{
    public class CliFeatureFixture : FeatureFixture
    {
        protected IExecutor Executor { get; set; }

//        protected ToolingConfiguration Config { get; set; }

        protected ToolingConfiguration Config { get; set; }

        protected TextWriter OutStream { get; set; }

        //
        // Givens
        //

        protected void a_project()
        {
            Config = new ToolingConfiguration();
            OutStream = new StringWriter();
        }

        protected void the_output_should_be(string text)
        {
            OutStream.ToString().Trim().ShouldBe(text);
        }

        protected void the_target_should_be(string name)
        {
            Config.target.ShouldBe(name);
        }
    }
}
