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

namespace Steeltoe.Tooling.Cli.Test
{
    public class CliFeatureFixture : FeatureFixture
    {

        protected Exception Exception { get; set; }

        protected Configuration Config { get; set; }

        protected TextWriter OutStream { get; set; }

        //
        // Givens
        //

        protected void a_project()
        {
            Config = new Configuration();
            OutStream = new StringWriter();
        }

        protected void a_service(string name, string type)
        {
            Config.services.Add(name, new Configuration.Service(type));
        }

        //
        // Thens
        //

        protected void the_output_should_be(string text)
        {
            OutStream.ToString().Trim().ShouldBe(text);
        }

        protected void the_target_should_be(string name)
        {
            Config.target.ShouldBe(name);
        }

        protected void the_services_should_include(string name, string type)
        {
            Config.services.ShouldContainKey(name);
            Config.services[name].type.ShouldBe(type);
        }

        protected void an_exception_should_be_thrown<T>(string message)
        {
            Exception.ShouldBeOfType<T>();
            Exception.Message.ShouldBe(message);
        }

        //
        // utils
        //

        protected void ExecuteCatchingAnyExceptions(IExecutor executor)
        {
            Exception = null;
            try
            {
                OutStream = new StringWriter();
                executor.Execute(OutStream);
            }
            catch (CommandException e)
            {
                Exception = e;
            }
        }

    }
}
