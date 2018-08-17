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
using System.Runtime.InteropServices;
using LightBDD.XUnit2;
using Shouldly;

namespace Steeltoe.Tooling.DotnetCli.Test
{
    public partial class ToolingConfigurationFeature : FeatureFixture
    {
        private ToolingConfiguration Config { get; set; }

        private StringWriter ostream;

        private StringReader istream;

        //
        // Givens
        //

        private void a_tooling_configuration()
        {
            Config = new ToolingConfiguration();
        }

        private void a_stream_containing(string text)
        {
            istream = new StringReader(text);
        }

        //
        // Whens
        //

        private void the_target_is_set(string name)
        {
            Config.target = name;
        }

        private void a_service_is_added(string name, string type)
        {
            Config.services.Add(name, new ToolingConfiguration.Service(type));
        }

        private void the_tooling_configuration_is_stored()
        {
            ostream = new StringWriter();
            Config.Store(ostream);
        }

        private void the_tooling_configuration_is_loaded()
        {
            Config = ToolingConfiguration.Load(istream);
        }

        //
        // Thens
        //

        private void the_stored_content_should_be(string content)
        {
            ostream.ToString().ShouldBe(content);
        }

        private void the_target_should_be(string name)
        {
            Config.target.ShouldBe(name);
        }

        private void a_service_should_be(string name, string type)
        {
            Config.services.ShouldContainKey(name);
            Config.services[name].type.ShouldBe(type);
        }
    }
}
