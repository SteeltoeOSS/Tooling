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
        protected Configuration Config { get; set; }

        protected TextWriter ConsoleOut { get; set; }

        protected Exception Exception { get; set; }

        //
        // Givens
        //

        protected void a_project()
        {
            Config = new Configuration();
        }

        protected void a_service(string name, string type)
        {
            Config.services.Add(name, new Configuration.Service(type));
        }

        //
        // Thens
        //

        protected void the_output_should_include(string text)
        {
            ConsoleOut.ToString().ShouldContain(text);
        }

        protected void the_output_should_be_empty()
        {
            ConsoleOut.ToString().Trim().ShouldBeEmpty();
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

        protected void the_services_should_not_include(string name)
        {
            Config.services.ShouldNotContainKey(name);
        }

        protected void an_exception_should_be_thrown<T>(string message)
        {
            Exception.ShouldBeOfType<T>();
            Exception.Message.ShouldBe(message);
        }

        //
        // utils
        //

        protected void Execute(IExecutor executor)
        {
            Exception = null;
            try
            {
                ConsoleOut = new StringWriter();
                executor.Execute(Config, ConsoleOut);
            }
            catch (CommandException e)
            {
                Exception = e;
            }
        }

    }
}
