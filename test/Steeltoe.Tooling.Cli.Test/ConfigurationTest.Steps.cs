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
    public partial class ConfigurationTest : FeatureFixture
    {
        private Configuration _config;

        private StringWriter _ostream;

        private StringReader _istream;

        //
        // Givens
        //

        private void a_tooling_configuration()
        {
            _config = new Configuration();
        }

        private void a_stream_containing(string text)
        {
            _istream = new StringReader(text);
        }

        //
        // Whens
        //

        private void the_target_is_set(string name)
        {
            _config.target = name;
        }

        private void a_service_is_added(string name, string type)
        {
            _config.services.Add(name, new Configuration.Service(type));
        }

        private void the_tooling_configuration_is_stored()
        {
            _ostream = new StringWriter();
            _config.Store(_ostream);
        }

        private void the_tooling_configuration_is_loaded()
        {
            _config = Configuration.Load(_istream);
        }

        //
        // Thens
        //

        private void the_stored_content_should_be(string content)
        {
            _ostream.ToString().ShouldBe(content);
        }

        private void the_target_should_be(string name)
        {
            _config.target.ShouldBe(name);
        }

        private void a_service_should_be(string name, string type)
        {
            _config.services.ShouldContainKey(name);
            _config.services[name].type.ShouldBe(type);
        }
    }
}
